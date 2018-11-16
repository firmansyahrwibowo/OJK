using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BismaEvent;
using System;

public class Game2Manager : MonoBehaviour
{
    public Text scoreText;
    public int Score;
    public Image durationFill;
    private float timeAmount = 80;
    public float time;
    public GameObject GameOver;
    public GameObject Blade;
    public GameObject Spawner;
    public GameObject Dodo;
    public GameObject DodoWin;
    public GameObject DodoLose;
    public GameObject Nina;
    public GameObject NinaWin;
    public GameObject NinaLose;
    public GameObject Enemy;
    public GameObject EnemyWin;
    public GameObject EnemyLose;
    public CharacterType Type;
    public int CharacterPick;
    public Game1Manager GameManager1;

	public int CurrentQuestion;
	public int QuestionAnswered;
	public bool OnceAskedQuestion=false;
	public GameObject Quiz1;
    public GameObject Quiz2;
    public GameObject Quiz3;
    public QuestionHolder Question1;
    public AnswerHolder AnswerA1;
    public AnswerHolder AnswerB1;
    public AnswerHolder AnswerC1;
    public AnswerHolder AnswerD1;
	public QuestionHolder Question2;
	public AnswerHolder AnswerA2;
	public AnswerHolder AnswerB2;
	public AnswerHolder AnswerC2;
	public AnswerHolder AnswerD2;
	public QuestionHolder Question3;
	public AnswerHolder AnswerA3;
	public AnswerHolder AnswerB3;
	public AnswerHolder AnswerC3;
	public AnswerHolder AnswerD3;
    public List<Quiz> Game2Quiz;
    public List<Quiz> RandomizedQuiz;
	public GameObject SpamBlocker;
	public List<GameObject> SpawnedItem;
    public GameObject DurationBarEffect;
    public GameObject BenarPopup;
    public GameObject SalahPopup;

    bool _IsStart = false;

    FruitSpawner _Spawner;
    Blade _Blade;
    Animator m_AnimatorEnemy, m_AnimatorDodo, m_AnimatorNina;

	[Header("POP UP")]
	public GameObject SalahPopUp;
	public GameObject BenarPopUp;

	[SerializeField]
	GameObject _StartUI;
    // Use this for initialization
    private void Awake()
    {
        EventManager.AddListener<DurationCutEvent>(CutEventHandler);
        EventManager.AddListener<ScoreSetEvent>(SetScoreHandler);
        EventManager.AddListener<InitCharacterManagerEvent>(InitCharacterManager);

        _Spawner = GetComponent<FruitSpawner>();
        _Blade = GetComponentInChildren<Blade>();

        m_AnimatorDodo = Dodo.GetComponent<Animator>();
        m_AnimatorNina = Nina.GetComponent<Animator>();
        m_AnimatorEnemy = Enemy.GetComponent<Animator>();
    }
    private void Start()
    {
        PoolingObject.Instance.InitPooling();
    }
    private void SetScoreHandler(ScoreSetEvent e)
    {
        Score += e.Value;
    }

    private void CutEventHandler(DurationCutEvent e)
    {
        time -= e.Value;
    }

    public void Init()
    {
        Score = 0; 
		scoreText.text = "0";
		_IsStart = false;
        durationFill.fillAmount = 0;
		_Blade.Reset ();

		Spawner.SetActive(false);
		_Spawner.Reset();
		Blade.SetActive (false);
		_Blade.StopInit();
        
		//Reset Random Quiz
		CurrentQuestion = 0;
		QuestionAnswered = 0;
		OnceAskedQuestion = false;
        CopyQuizFromGame1();
        QuizRandomizer();
		GenerateQuestion();
		Quiz1.SetActive (true);
        Quiz2.SetActive(false);
        Quiz3.SetActive(false);
        BenarPopup.SetActive(false);
        SalahPopUp.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
		QuestionChallenge();

		if (_IsStart)
        {
            m_AnimatorDodo.SetBool("IsPlay", true);
            m_AnimatorNina.SetBool("IsPlay", true);
            m_AnimatorEnemy.SetBool("IsPlay", true);
            if (Score <= 0)
            {
                Score = 0;
            }
            scoreText.text = Score.ToString();
            if (time > 0)
            {
                time -= Time.deltaTime;
                durationFill.fillAmount = time / timeAmount;
            }
            else
            {
                _IsStart = false;
                GameEnd();
            }
        }
    }

    void GameEnd()
    {
		Spawner.SetActive(false);
		_Spawner.Reset();
		Blade.SetActive (false);
		_Blade.StopInit();
        durationFill.fillAmount = 0;
        Spawner.SetActive(false);
       
        Dodo.SetActive(false);
        Nina.SetActive(false);
        Enemy.SetActive(false);

        EventManager.TriggerEvent(new PopUpScoreEvent(Score.ToString(), false));
        if (Score>0)
        {
            //Win Condition
            if (CharacterPick==0)
            {
                DodoWin.SetActive(true);
            }

            if (CharacterPick==1)
            {
                NinaWin.SetActive(true);
            }
            EnemyLose.SetActive(true);
        }
        else
        {
            //Lose Condition
            if (CharacterPick==0)
            {
                DodoLose.SetActive(true);
            }

            if(CharacterPick==1)
            {
                NinaLose.SetActive(true);
            }
            EnemyWin.SetActive(true);
        }
    }

    void InitCharacterManager(InitCharacterManagerEvent e)
    {
        Dodo.SetActive(false);
        DodoWin.SetActive(false);
        DodoLose.SetActive(false);

        Nina.SetActive(false);
        NinaWin.SetActive(false);
        NinaLose.SetActive(false);

        Enemy.SetActive(false);
        EnemyWin.SetActive(false);
        EnemyLose.SetActive(false);

        Game2Quiz.Clear();
        RandomizedQuiz.Clear();
		SpamBlocker.SetActive (false);

        if (e.Type == 0)
        {
            Dodo.SetActive(true);
            Enemy.SetActive(true);
            m_AnimatorDodo.SetBool("IsPlay", false);
            m_AnimatorEnemy.SetBool("IsPlay", false);
            CharacterPick = 0;
        }
        else
        {
            Nina.SetActive(true);
            Enemy.SetActive(true);
            m_AnimatorNina.SetBool("IsPlay", false);
            m_AnimatorEnemy.SetBool("IsPlay", false);
            CharacterPick = 1;
        }
    }

    void QuizRandomizer()
    {
        for (int i = 0; i < 3; i++)
        {
            int temp = UnityEngine.Random.Range(0,Game2Quiz.Count);
            RandomizedQuiz.Add(Game2Quiz[temp]);
            Game2Quiz.Remove(Game2Quiz[temp]);
        }
    }

    void CopyQuizFromGame1()
    {
        for (int i = 0; i < GameManager1._quizList.Count; i++)
        {
            Game2Quiz.Add(GameManager1._quizList[i]);
        }
    }

    void GenerateQuestion()
    {
		// Question Sengaja ga di list, takutnya ada revisi minta di random lagi

		//1
		Question1.QuestionContent = RandomizedQuiz [0].question;
		Question1.QuestionText.text = RandomizedQuiz [0].question;

		AnswerA1.AnswerContent = RandomizedQuiz [0].option [0].answer;
		AnswerA1.AnswerText.text = RandomizedQuiz [0].option [0].answer;
		AnswerA1.IsTrue = RandomizedQuiz [0].option [0].isTrue;

		AnswerB1.AnswerContent = RandomizedQuiz [0].option [1].answer;
		AnswerB1.AnswerText.text = RandomizedQuiz [0].option [1].answer;
		AnswerB1.IsTrue = RandomizedQuiz [0].option [1].isTrue;

		AnswerC1.AnswerContent = RandomizedQuiz [0].option [2].answer;
		AnswerC1.AnswerText.text = RandomizedQuiz [0].option [2].answer;
		AnswerC1.IsTrue = RandomizedQuiz [0].option [2].isTrue;

		AnswerD1.AnswerContent = RandomizedQuiz [0].option [3].answer;
		AnswerD1.AnswerText.text = RandomizedQuiz [0].option [3].answer;
		AnswerD1.IsTrue = RandomizedQuiz [0].option [3].isTrue;

		//2
		Question2.QuestionContent = RandomizedQuiz [1].question;
		Question2.QuestionText.text = RandomizedQuiz [1].question;

		AnswerA2.AnswerContent = RandomizedQuiz [1].option [0].answer;
		AnswerA2.AnswerText.text = RandomizedQuiz [1].option [0].answer;
		AnswerA2.IsTrue = RandomizedQuiz [1].option [0].isTrue;

		AnswerB2.AnswerContent = RandomizedQuiz [1].option [1].answer;
		AnswerB2.AnswerText.text = RandomizedQuiz [1].option [1].answer;
		AnswerB2.IsTrue = RandomizedQuiz [1].option [1].isTrue;

		AnswerC2.AnswerContent = RandomizedQuiz [1].option [2].answer;
		AnswerC2.AnswerText.text = RandomizedQuiz [1].option [2].answer;
		AnswerC2.IsTrue = RandomizedQuiz [1].option [2].isTrue;

		AnswerD2.AnswerContent = RandomizedQuiz [1].option [3].answer;
		AnswerD2.AnswerText.text = RandomizedQuiz [1].option [3].answer;
		AnswerD2.IsTrue = RandomizedQuiz [1].option [3].isTrue;

		//3
		Question3.QuestionContent = RandomizedQuiz [2].question;
		Question3.QuestionText.text = RandomizedQuiz [2].question;

		AnswerA3.AnswerContent = RandomizedQuiz [2].option [0].answer;
		AnswerA3.AnswerText.text = RandomizedQuiz [2].option [0].answer;
		AnswerA3.IsTrue = RandomizedQuiz [2].option [0].isTrue;

		AnswerB3.AnswerContent = RandomizedQuiz [2].option [1].answer;
		AnswerB3.AnswerText.text = RandomizedQuiz [2].option [1].answer;
		AnswerB3.IsTrue = RandomizedQuiz [2].option [1].isTrue;

		AnswerC3.AnswerContent = RandomizedQuiz [2].option [2].answer;
		AnswerC3.AnswerText.text = RandomizedQuiz [2].option [2].answer;
		AnswerC3.IsTrue = RandomizedQuiz [2].option [2].isTrue;

		AnswerD3.AnswerContent = RandomizedQuiz [2].option [3].answer;
		AnswerD3.AnswerText.text = RandomizedQuiz [2].option [3].answer;
		AnswerD3.IsTrue = RandomizedQuiz [2].option [3].isTrue;
    }

	void QuestionChallenge()
	{
		if (OnceAskedQuestion==false) 
		{
			if ((CurrentQuestion==3) && (QuestionAnswered==1)) 
			{
				time = 30;
                _StartUI.SetActive(true);
                OnceAskedQuestion = true;
                StartCoroutine(StartGame());
			}
			if ((CurrentQuestion==3) && (QuestionAnswered==2)) 
			{
				time = 60;
                _StartUI.SetActive(true);
                OnceAskedQuestion = true;
                StartCoroutine(StartGame());
            }
			if ((CurrentQuestion==3) && (QuestionAnswered==3)) 
			{
				time = 80;
                _StartUI.SetActive(true);
                OnceAskedQuestion = true;
                StartCoroutine(StartGame());
            }

			if((CurrentQuestion==3) && (QuestionAnswered==0))
			{
				GameEnd ();
				OnceAskedQuestion = true;
			}

		}

	}

    public void Reset()
    {
        Dodo.SetActive(false);
        DodoWin.SetActive(false);
        DodoLose.SetActive(false);

        Nina.SetActive(false);
        NinaWin.SetActive(false);
        NinaLose.SetActive(false);

        Enemy.SetActive(false);
        EnemyWin.SetActive(false);
        EnemyLose.SetActive(false);

        Game2Quiz.Clear();
        RandomizedQuiz.Clear();
        SpamBlocker.SetActive(false);

        Spawner.SetActive(false);
        _Spawner.Reset();
        _Blade.Reset();
        Blade.SetActive(false);
        _Blade.StopInit();
        durationFill.fillAmount = 0;
        Spawner.SetActive(false);

        Score = 0;
        scoreText.text = "0";
        _IsStart = false;

        //Reset Random Quiz
        CurrentQuestion = 0;
        QuestionAnswered = 0;
        OnceAskedQuestion = false;
        CopyQuizFromGame1();
        QuizRandomizer();
        GenerateQuestion();
        Quiz1.SetActive(false);
        Quiz2.SetActive(false);
        Quiz3.SetActive(false);
        BenarPopup.SetActive(false);
        SalahPopUp.SetActive(false);

        foreach (var item in SpawnedItem)
        {
            item.SetActive(false);
            SpawnedItem.Remove(item);
        }
    }

    public IEnumerator StartGame()
    {
        yield return new WaitForSeconds(3f);
        _IsStart = true;
        Spawner.SetActive(true);
        _Spawner.InitSpawner();
        Blade.SetActive(true);
        _Blade.Init();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[System.Serializable]
public class Quiz {
    public string question;
    public List<Option> option;
}

[System.Serializable]
public class Option {
    public string answer;
    public bool isTrue;
}
public class Game1Manager : MonoBehaviour {

    [SerializeField]
    List<Quiz> _quizList = new List<Quiz>();
    [SerializeField]
    List<Quiz> _generatedQuiz = new List<Quiz>();

    [SerializeField]
    GameObject _GameStartPopUp;
    [SerializeField]
    GameObject _TriviaObject;

    [SerializeField]
    int _MaxQuiz;

    [Header("UI Trivia")]
    [SerializeField]
    Text _QuestionText;
    [SerializeField]
    GameObject _OptionABtn;
    [SerializeField]
    GameObject _OptionBBtn;
    [SerializeField]
    GameObject _OptionCBtn;
    [SerializeField]
    GameObject _OptionDBtn;

    Text _OptionAText;
    Text _OptionBText;
    Text _OptionCText;
    Text _OptionDText;

    int _QuizCount = 0, _TrueAnswer = 0, _FalseAnswer = 0;

    [Header("POP UP")]
    [SerializeField]
    GameObject _SalahPopUp;
    [SerializeField]
    GameObject _BenarPopUp;

    [Header("Score")]
    [SerializeField]
    Text _ScoreText;
    float _ScorePoint;

    [Header("Battle")]
    [SerializeField]
    RectTransform _BattleObject;
    Vector2 _DefaultPosBattle;
    
    bool _IsStart = false;
    float _TimeCount = 0;

    Tween _MoveTween;

    private void Awake()
    {
        _OptionABtn.AddComponent<Button>().onClick.AddListener(delegate {
            SelectAnswer(OptionType.OPTION_A);
        });
        _OptionBBtn.AddComponent<Button>().onClick.AddListener(delegate {
            SelectAnswer(OptionType.OPTION_B);
        });
        _OptionCBtn.AddComponent<Button>().onClick.AddListener(delegate {
            SelectAnswer(OptionType.OPTION_C);
        });
        _OptionDBtn.AddComponent<Button>().onClick.AddListener(delegate {
            SelectAnswer(OptionType.OPTION_D);
        });

        _OptionABtn.AddComponent<OptionData>();
        _OptionBBtn.AddComponent<OptionData>();
        _OptionCBtn.AddComponent<OptionData>();
        _OptionDBtn.AddComponent<OptionData>();

        _OptionAText = _OptionABtn.GetComponentInChildren<Text>();
        _OptionBText = _OptionBBtn.GetComponentInChildren<Text>();
        _OptionCText = _OptionCBtn.GetComponentInChildren<Text>();
        _OptionDText = _OptionDBtn.GetComponentInChildren<Text>();
    }

    private void Start()
    {
        _DefaultPosBattle = _BattleObject.anchoredPosition;
    }

    public void Init()
    {
        ResetData();
        _BattleObject.anchoredPosition = _DefaultPosBattle;
        GenerateQuiz();
        _GameStartPopUp.SetActive(true);
        ShowQuiz();
        InitBattle();
    }

    #region TRIVIA ROLL
    void ShuffleQuiz() {
        for (int i = 0; i < _quizList.Count; i++)
        {
            Quiz temp = _quizList[i];
            int randomIndex = Random.Range(i, _quizList.Count);
            _quizList[i] = _quizList[randomIndex];
            ShuffleAnswer(_quizList[i]);
            _quizList[randomIndex] = temp;
        }
    }

    void ShuffleAnswer(Quiz quiz)
    {
        for (int i = 0; i < quiz.option.Count; i++)
        {
            Option temp = quiz.option[i];
            int randomIndex = Random.Range(i, quiz.option.Count);
            quiz.option[i] = quiz.option[randomIndex];
            quiz.option[randomIndex] = temp;
        }
    }

    void GenerateQuiz()
    {
        ShuffleQuiz();
        _generatedQuiz = new List<Quiz>();

        for (int i = 0; i < _MaxQuiz; i++) {
            _generatedQuiz.Add(_quizList[i]);
        }
    }
    #endregion

    #region QUESTION
    void SelectAnswer(OptionType type) {
        switch (type) {
            case OptionType.OPTION_A:
                ThrowAnswer(_OptionABtn.GetComponent<OptionData>().option.isTrue);
                break;
            case OptionType.OPTION_B:
                ThrowAnswer(_OptionBBtn.GetComponent<OptionData>().option.isTrue);
                break;
            case OptionType.OPTION_C:
                ThrowAnswer(_OptionCBtn.GetComponent<OptionData>().option.isTrue);
                break;
            case OptionType.OPTION_D:
                ThrowAnswer(_OptionDBtn.GetComponent<OptionData>().option.isTrue);
                break;
        }
    }

    void ThrowAnswer(bool isTrue) {

        if (isTrue)
        {
            _TrueAnswer++;
            _BenarPopUp.SetActive(true);

            float truePoint = 100 / _TimeCount;
            _ScorePoint += Mathf.FloorToInt (truePoint);

            _ScoreText.text = "SCORE : " + Mathf.FloorToInt(_ScorePoint);

            EventManager.TriggerEvent(new SFXPlayEvent(SfxType.BENAR, false));
        }
        else
        {
            _FalseAnswer++;
            _SalahPopUp.SetActive(true);

            float falsePoint = 2 * _TimeCount;
            _ScorePoint -= Mathf.FloorToInt(falsePoint);
            if (_ScorePoint < 0)
                _ScorePoint = 0;

            _ScoreText.text = "SCORE : " + Mathf.FloorToInt(_ScorePoint);

            EventManager.TriggerEvent(new SFXPlayEvent(SfxType.SALAH, false));
        }
        CheckAction(isTrue);
        EventManager.TriggerEvent(new HoldOnEvent(true));

        StartCoroutine (NextQuiz());
    }

    void ShowQuiz()
    {
        _TriviaObject.SetActive(true);

        EventManager.TriggerEvent(new HoldOnEvent(false));
        _QuestionText.text = _generatedQuiz[_QuizCount].question;
        _OptionAText.text = _generatedQuiz[_QuizCount].option[0].answer;
        _OptionBText.text = _generatedQuiz[_QuizCount].option[1].answer;
        _OptionCText.text = _generatedQuiz[_QuizCount].option[2].answer;
        _OptionDText.text = _generatedQuiz[_QuizCount].option[3].answer;

        _OptionABtn.GetComponent<OptionData>().option = _generatedQuiz[_QuizCount].option[0];
        _OptionBBtn.GetComponent<OptionData>().option = _generatedQuiz[_QuizCount].option[1];
        _OptionCBtn.GetComponent<OptionData>().option = _generatedQuiz[_QuizCount].option[2];
        _OptionDBtn.GetComponent<OptionData>().option = _generatedQuiz[_QuizCount].option[3];
    }

    IEnumerator NextQuiz() {
        _TimeCount = 0;
        _QuizCount++;
        if (_QuizCount < _MaxQuiz)
        {
            yield return new WaitForSeconds(1);
            ShowQuiz();
        }
        else
        {
            GameEnd();
            Debug.Log("QUIZ END, RESULT = (TRUE : "+_TrueAnswer.ToString()+") (FALSE : "+_FalseAnswer+")");
        }
    }
    #endregion

    #region BATTLE
    void InitBattle()
    {
        _IsStart = true;
        _ScorePoint = 0;
        _ScoreText.text = "SCORE : " + Mathf.FloorToInt(_ScorePoint);
    }

    private void Update()
    {
        if (_IsStart)
        {
            _TimeCount += (1 * Time.deltaTime);
            Debug.Log(_TimeCount);
        }
    }

    void CheckAction(bool isTrue)
    {
        KillTween();
        if (isTrue)
        {
            _MoveTween = _BattleObject.DOAnchorPos(new Vector2(_BattleObject.anchoredPosition.x + 50, _BattleObject.anchoredPosition.y), 1).OnComplete(ConditionAfterAnswer);
        }
        else
        {
            _MoveTween = _BattleObject.DOAnchorPos(new Vector2(_BattleObject.anchoredPosition.x - 50, _BattleObject.anchoredPosition.y), 1).OnComplete(ConditionAfterAnswer); 
        }


    }

    void ConditionAfterAnswer()
    {
        if (!_IsStart)
            return;

        KillTween();
        if (_BattleObject.anchoredPosition.x >= 300)
            _MoveTween = _BattleObject.DOAnchorPos(new Vector2(_BattleObject.anchoredPosition.x - 100, _BattleObject.anchoredPosition.y), 1);
        else if (_BattleObject.anchoredPosition.x <= -300)
            _MoveTween = _BattleObject.DOAnchorPos(new Vector2(_BattleObject.anchoredPosition.x + 100, _BattleObject.anchoredPosition.y), 1);
    } 

    void GameEnd()
    {
        _IsStart = false;
        
        //CHARACTER RESULT OBJECT
        if (_TrueAnswer > _FalseAnswer)
        {
            EventManager.TriggerEvent(new ResultCharacterEvent(ResultType.WIN));
            EventManager.TriggerEvent(new SFXPlayEvent(SfxType.WIN, false));

            WinningBonus();
        }
        else
        {
            EventManager.TriggerEvent(new ResultCharacterEvent(ResultType.LOSE));
            EventManager.TriggerEvent(new SFXPlayEvent(SfxType.LOSE, false));
        }

        //BUAT NAMPILIN POP UP SCORE
        EventManager.TriggerEvent(new PopUpScoreEvent(Mathf.FloorToInt(_ScorePoint).ToString(), true));
    }

    void WinningBonus()
    {
        float winBonus = _TrueAnswer * _TrueAnswer;
        
        _ScorePoint += Mathf.FloorToInt(winBonus);
        _ScoreText.text = "SCORE : " + Mathf.FloorToInt(_ScorePoint);
    }
    #endregion
    void ResetData()
    {
        _TimeCount = 0;
        _QuizCount = 0;
        _TrueAnswer = 0;
        _FalseAnswer = 0;

        KillTween();
    }

    void KillTween() {
        if (_MoveTween != null)
            _MoveTween.Kill(false);
    }
}

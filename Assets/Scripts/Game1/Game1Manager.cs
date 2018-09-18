using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    Image _ImageFill;
    [SerializeField]
    RectTransform _BattleObject;
    Vector2 _DefaultPosBattle;
    [SerializeField]
    float _Speed = 10;
    [SerializeField]
    float _FillSpeed = 0.05f;
    [SerializeField]
    float _IncreaseSpeed = 15f;
    [SerializeField]
    float _IncreaseFillSpeed = 0.1f;

    int _WaitingTime = 1;
    bool _isWaiting = false;

    float _VelocityValue = 0;
    float _FillVelocityValue = 0;
    bool _IsStart = false;
    Coroutine _AddValue;

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
            _isWaiting = true;
            _TrueAnswer++;
            Debug.Log("TRUE");
            _BenarPopUp.SetActive(true);
            _ScorePoint += 20;
            _ScoreText.text = "SCORE : " + Mathf.FloorToInt(_ScorePoint);
            StartCoroutine(TrueAnswerWaiting());
        }
        else
        {
            _FalseAnswer++;
            Debug.Log("FALSE");
            _SalahPopUp.SetActive(true);
        }

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
        _QuizCount++;
        if (_QuizCount < _MaxQuiz)
        {
            yield return new WaitForSeconds(1);
            ShowQuiz();
        }
        else
        {
            if (_TrueAnswer >= _QuizCount)
                WinningBonus();

            GameEnd();
            Debug.Log("QUIZ END, RESULT = (TRUE : "+_TrueAnswer.ToString()+") (FALSE : "+_FalseAnswer+")");
        }
    }
    #endregion

    #region BATTLE
    void InitBattle()
    {
        if (_AddValue != null)
            StopCoroutine(_AddValue);

        _ImageFill.fillAmount = 0.5f;
        _IsStart = true;
        _VelocityValue = 0;
        _FillVelocityValue = 0;
        _ScorePoint = 0;
        _ScoreText.text = "SCORE : " + Mathf.FloorToInt(_ScorePoint);
        _isWaiting = false;

        //_AddValue = StartCoroutine(IncreaseValue());
    }
    private void Update()
    {
        if (_IsStart)
        {
            _ScorePoint += 2 * Time.deltaTime;
            _ScoreText.text = "SCORE : " + Mathf.FloorToInt(_ScorePoint);
            if (!_isWaiting)
            {
                _BattleObject.anchoredPosition = new Vector3(_BattleObject.anchoredPosition.x - ((_Speed + _VelocityValue) * Time.deltaTime), _BattleObject.anchoredPosition.y);
                _ImageFill.fillAmount -= ((0.05f + _FillVelocityValue) * Time.deltaTime);
            }
            else
            {
                _BattleObject.anchoredPosition = new Vector3(_BattleObject.anchoredPosition.x + _IncreaseSpeed * Time.deltaTime, _BattleObject.anchoredPosition.y);
                _ImageFill.fillAmount += _IncreaseFillSpeed * Time.deltaTime;
            }

            if (_ImageFill.fillAmount <= 0)
            {
                GameEnd();
            }
            else if (_ImageFill.fillAmount >= 1)
            {
                WinningBonus();
                GameEnd();
            }
        }
    }

    IEnumerator TrueAnswerWaiting()
    {
        yield return new WaitForSeconds(1);
        _isWaiting = false;
    }

    //IEnumerator IncreaseValue() {
    //    while (_IsStart)
    //    {
    //        yield return new WaitForSeconds(5);
    //        _FillVelocityValue += 0.05f;
    //        _VelocityValue += 5;
    //    }
    //}

    void GameEnd()
    {
        _IsStart = false;

        if (_AddValue != null)
            StopCoroutine(_AddValue);

        _VelocityValue = 0;
        _FillVelocityValue = 0;

        //BUAT NAMPILIN POP UP SCORE
        EventManager.TriggerEvent(new PopUpScoreEvent(new HighScore("JOKO", Mathf.FloorToInt(_ScorePoint).ToString())));
    }

    void WinningBonus()
    {
        _ScorePoint += 100;
        _ScoreText.text = "SCORE : " + Mathf.FloorToInt(_ScorePoint);
    }
    #endregion
    void ResetData() {
        _QuizCount = 0;
        _TrueAnswer = 0;
        _FalseAnswer = 0;
    }
}

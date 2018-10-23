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
    public List<Quiz> _quizList = new List<Quiz>();
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
    GameObject [] _OptionBtn;

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
    [SerializeField]
    float _PushValue = 50;
    bool _IdleBattle = false;
    
    bool _IsStart = false;
    float _TimeCount = 0;

    [Header("Time")]
    [SerializeField]
    float _TimeDuration = 120;
    [SerializeField]
    Text _TimeText;
    string _Minutes;
    string _Second;

    Tween _MoveTween;

    [SerializeField]
    Sprite[] _AnswerBG;

    [SerializeField]
    List<OptionData> _OptionData = new List<OptionData>();
    private void Awake()
    {
        _OptionBtn[0].AddComponent<Button>().onClick.AddListener(delegate
        {
            SelectAnswer(OptionType.OPTION_A);
        });
        _OptionBtn[1].AddComponent<Button>().onClick.AddListener(delegate
        {
            SelectAnswer(OptionType.OPTION_B);
        });
        _OptionBtn[2].AddComponent<Button>().onClick.AddListener(delegate
        {
            SelectAnswer(OptionType.OPTION_C);
        });
        _OptionBtn[3].AddComponent<Button>().onClick.AddListener(delegate
        {
            SelectAnswer(OptionType.OPTION_D);
        });

        //int index = 0;
        //foreach (GameObject btn in _OptionBtn)
        //{
        //    btn.AddComponent<Button>().onClick.AddListener(delegate
        //    {
        //        OptionType type = (OptionType)index;
        //        SelectAnswer(type);
        //    });
        //    index++;
        //}

        //Set Option Data
        foreach (GameObject btn in _OptionBtn)
            _OptionData.Add(btn.AddComponent<OptionData>());
        
        int indexData = 0;

        foreach (OptionData data in _OptionData)
        {
            data.thisImage = _OptionBtn[indexData].GetComponent<Image>();
            data.thisText = _OptionBtn[indexData].GetComponentInChildren<Text>();
            indexData++;
        }
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
    void SelectAnswer(OptionType type)
    {
        ThrowAnswer((int)type);
    }
    
    void ThrowAnswer(int index) {
        _IdleBattle = false;
        bool isTrue = _OptionData[index].option.isTrue;

        if (isTrue)
        {
            _TrueAnswer++;
            _BenarPopUp.SetActive(true);

            float truePoint = 100 / _TimeCount;
            _ScorePoint += Mathf.FloorToInt (truePoint);

            _ScoreText.text = "SCORE : " + Mathf.FloorToInt(_ScorePoint);

            EventManager.TriggerEvent(new SFXPlayEvent(SfxType.BENAR, false));

            _OptionData[index].thisImage.sprite = _AnswerBG[1];
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
            
            //CHANGE BG
            OptionData data = _OptionData.Find(x => x.option.isTrue); //===> TRUE BG
            data.thisImage.sprite = _AnswerBG[1];
            _OptionData[index].thisImage.sprite = _AnswerBG[2]; //===> FALSE BG

        }
        CheckAction(isTrue);
        EventManager.TriggerEvent(new FaceEvent(FaceType.HAPPY, isTrue));
        EventManager.TriggerEvent(new HoldOnEvent(true));

        StartCoroutine (NextQuiz());
    }

    void ShowQuiz()
    {
        _TriviaObject.SetActive(true);

        EventManager.TriggerEvent(new HoldOnEvent(false));
        _QuestionText.text = _generatedQuiz[_QuizCount].question;

        for (int i = 0; i < _OptionData.Count; i++) {
            _OptionData[i].thisText.text = _generatedQuiz[_QuizCount].option[i].answer;
        }

        for (int i = 0; i < _OptionData.Count; i++)
            _OptionData[i].option = _generatedQuiz[_QuizCount].option[i];
    }

    IEnumerator NextQuiz() {
        _TimeCount = 0;
        _QuizCount++;
        if (_QuizCount < _MaxQuiz)
        {
            yield return new WaitForSeconds(1);
            for (int i = 0; i < _OptionData.Count; i++)
                _OptionData[i].thisImage.sprite = _AnswerBG[0]; //=========> RESTART BG
            ShowQuiz();
        }
        else
        {
            GameEnd(ConditionType.DEFAULT);
            yield return new WaitForSeconds(1);
            for (int i = 0; i < _OptionData.Count; i++)
                _OptionData[i].thisImage.sprite = _AnswerBG[0]; //=========> RESTART BG
            //Debug.Log("QUIZ END, RESULT = (TRUE : "+_TrueAnswer.ToString()+") (FALSE : "+_FalseAnswer+")");
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

            _TimeDuration -= (1 * Time.deltaTime);
            _Minutes = Mathf.Floor(_TimeDuration / 60).ToString("00");
            _Second = (_TimeDuration % 60).ToString("00");
            
            _TimeText.text = string.Format("{0}:{1}", _Minutes, _Second);

            if (_TimeDuration < 0) {
                _TimeDuration = 0;
                _TimeText.text = "00:00";
                GameEnd(ConditionType.TIME_OVER);
            }
            if (_IdleBattle)
                _BattleObject.anchoredPosition = new Vector2(_BattleObject.anchoredPosition.x - (5*Time.deltaTime), _BattleObject.anchoredPosition.y);

            UpdateCondition();
        }
    }

    void UpdateCondition() {
        if (_BattleObject.anchoredPosition.x < -350)
        {
            GameEnd(ConditionType.LOSE_BATTLE);
        }
        else if (_BattleObject.anchoredPosition.x > 350)
        {
            GameEnd(ConditionType.WIN_BATTLE);
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

        _IdleBattle = true;
        KillTween();
        if (_BattleObject.anchoredPosition.x >= 300)
            _PushValue = 20;
        else if (_BattleObject.anchoredPosition.x <= -300)
            _PushValue = 20;
        else
            _PushValue = 50;

        if (_BattleObject.anchoredPosition.x < -200)
        {
            EventManager.TriggerEvent(new FaceEvent(FaceType.ANXIOUS, false));
        }
        else if (_BattleObject.anchoredPosition.x > 200)
        {
            EventManager.TriggerEvent(new FaceEvent(FaceType.ANXIOUS, true));
        }
        else
        {
            EventManager.TriggerEvent(new FaceEvent(FaceType.IDLE, true));
        }
    }

    void GameEnd(ConditionType type)
    {
        _IsStart = false;
        bool isWin = false;
        switch (type) {
            case ConditionType.DEFAULT:
                //CHARACTER RESULT OBJECT
                if (_TrueAnswer > _FalseAnswer)
                {
                    EventManager.TriggerEvent(new ResultCharacterEvent(ResultType.WIN));
                    EventManager.TriggerEvent(new SFXPlayEvent(SfxType.WIN, true));

                    TimeBonus();
                    QuestionBonus();
                    isWin = true;
                }
                else
                {
                    EventManager.TriggerEvent(new ResultCharacterEvent(ResultType.LOSE));
                    EventManager.TriggerEvent(new SFXPlayEvent(SfxType.LOSE, true));
                }
                break;
            case ConditionType.TIME_OVER:
                if (_TrueAnswer > _FalseAnswer)
                {
                    EventManager.TriggerEvent(new ResultCharacterEvent(ResultType.WIN));
                    EventManager.TriggerEvent(new SFXPlayEvent(SfxType.WIN, true));

                    QuestionBonus();
                    isWin = true;
                }
                else
                {
                    EventManager.TriggerEvent(new ResultCharacterEvent(ResultType.LOSE));
                    EventManager.TriggerEvent(new SFXPlayEvent(SfxType.LOSE, true));
                }
                break;
            case ConditionType.WIN_BATTLE:
                EventManager.TriggerEvent(new ResultCharacterEvent(ResultType.WIN));
                EventManager.TriggerEvent(new SFXPlayEvent(SfxType.WIN, true));

                TimeBonus();
                QuestionBonus();
                isWin = true;
                break;
            case ConditionType.LOSE_BATTLE:
                EventManager.TriggerEvent(new ResultCharacterEvent(ResultType.LOSE));
                EventManager.TriggerEvent(new SFXPlayEvent(SfxType.LOSE, true));
                break;
        }

        //BUAT NAMPILIN POP UP SCORE
        EventManager.TriggerEvent(new PopUpScoreEvent(Mathf.FloorToInt(_ScorePoint).ToString(), true, isWin));
    }

    void TimeBonus() {
        _ScorePoint = _TimeDuration * 2;
    }

    void QuestionBonus()
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

        _PushValue = 50;
        _IdleBattle = true;

        for (int i = 0; i < _OptionData.Count; i++)
            _OptionData[i].thisImage.sprite = _AnswerBG[0]; //=========> RESTART BG
        KillTween();
    }

    void KillTween() {
        if (_MoveTween != null)
            _MoveTween.Kill(false);
    }
}

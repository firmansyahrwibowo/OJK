﻿using System.Collections;
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
    public List<Quiz> Game2Quiz;
    public List<Quiz> RandomizedQuiz;

    bool _IsStart = false;

    FruitSpawner _Spawner;
    Blade _Blade;
    Animator m_AnimatorEnemy, m_AnimatorDodo, m_AnimatorNina;

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
        //Sebab Bug Blade
        //Blade.SetActive(true);

        Score = 0;
        time = timeAmount;
        _Blade.Init();  
        Spawner.SetActive(true);
        durationFill.fillAmount = 1;
        _IsStart = true;
        _Spawner.InitSpawner();
        _Blade.Init();

        //Reset Random Quiz
        CopyQuizFromGame1();
        QuizRandomizer();
    }

    // Update is called once per frame
    void Update()
    {
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

        //Sebab Bug Blade
        //Blade.SetActive(false);

        _Spawner.Reset();
        _Blade.StopInit();
        durationFill.fillAmount = 0;
        Spawner.SetActive(false);
        EventManager.TriggerEvent(new PopUpScoreEvent(Score.ToString(), false));
        Dodo.SetActive(false);
        Nina.SetActive(false);
        Enemy.SetActive(false);
       
        if (Score>=300)
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

        if (e.Type == 0)
        {
            Dodo.SetActive(true);
            Enemy.SetActive(true);
            CharacterPick = 0;
        }
        else
        {
            Nina.SetActive(true);
            Enemy.SetActive(true);
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
}

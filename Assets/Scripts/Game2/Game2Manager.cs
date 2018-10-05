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
    public GameObject Nina;
    public GameObject Enemy;
    public CharacterType Type;

    bool _IsStart = false;

    FruitSpawner _Spawner;
    Blade _Blade;
    Animator m_AnimatorDodo, m_AnimatorNina, m_AnimatorEnemy;
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
        time = timeAmount;
        Blade.SetActive(true);
        Spawner.SetActive(true);
        durationFill.fillAmount = 1;
        //StartCoroutine(spawnObject.SpawnFruits());
        _IsStart = true;
        _Spawner.InitSpawner();
        _Blade.Init();
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
        _Spawner.Reset();
        _Blade.StopInit();
        durationFill.fillAmount = 0;
        Blade.SetActive(false);
        Spawner.SetActive(false);
        //GameOver.GetComponent<Text>().text = "GAME OVER YOUR SCORE : "+Score.ToString();
        //GameOver.SetActive(true);
        EventManager.TriggerEvent(new PopUpScoreEvent(Score.ToString(), false));
        //m_AnimatorDodo.SetBool("IsPlay", false);
        //m_AnimatorNina.SetBool("IsPlay", false);
        m_AnimatorEnemy.SetBool("IsPlay", false);
        if (Score>=300)
        {
            m_AnimatorDodo.SetBool("IsWin", true);
            m_AnimatorNina.SetBool("IsWin", true);
        }
        else
        {
            m_AnimatorDodo.SetBool("IsLose", true);
            m_AnimatorNina.SetBool("IsLose", true);
        }
    }

    void InitCharacterManager(InitCharacterManagerEvent e)
    {
        Dodo.SetActive(false);
        Nina.SetActive(false);
        Enemy.SetActive(false);
        m_AnimatorDodo.SetBool("IsWin", false);
        m_AnimatorNina.SetBool("IsWin", false);
        m_AnimatorDodo.SetBool("IsLose", false);
        m_AnimatorNina.SetBool("IsLose", false);
        if (e.Type == 0)
        {
            Dodo.SetActive(true);
            Enemy.SetActive(true);
        }
        else
        {
            Nina.SetActive(true);
            Enemy.SetActive(true);
        }
    }
}

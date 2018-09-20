using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BismaEvent;
using System;

public class Game2Manager : MonoBehaviour {

    public Text scoreText;
    public int Score;
    public Image durationFill;
    private float timeAmount=80;
    public float time;
    public GameObject GameOver;
    public GameObject Blade;
    public GameObject Spawner;

    bool _IsStart = false;

    FruitSpawner _Spawner;
    Blade _Blade;
    // Use this for initialization
    private void Awake()
    {
        EventManager.AddListener<DurationCutEvent>(CutEventHandler);
        EventManager.AddListener<ScoreSetEvent>(SetScoreHandler);

        _Spawner = GetComponent<FruitSpawner>();
        _Blade = GetComponentInChildren<Blade>();
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

    public void Init ()
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
	void Update ()
    {
        if (_IsStart)
        {
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
        EventManager.TriggerEvent(new PopUpScoreEvent(new HighScore("xx", Score.ToString())));
    }
}

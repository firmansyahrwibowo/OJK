using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreManager : MonoBehaviour {

    [SerializeField]
    GameObject _SelesaiButton;

    [SerializeField]
    GameObject _UIScore;
    [SerializeField]
    Text _ScoreText;
    // Use this for initialization
    private void Awake()
    {
        EventManager.AddListener<PopUpScoreEvent>(PopUpScoreHandler);

        _SelesaiButton.AddComponent<Button>().onClick.AddListener(delegate {
            SelesaiButtonHandler();
        });
    }

    void Start() {
        _UIScore.SetActive(false);
    }

    private void SelesaiButtonHandler()
    {
        _UIScore.SetActive(false);
        EventManager.TriggerEvent(new ButtonActionEvent(ObjectType.MAIN_MENU));
    }

    private void PopUpScoreHandler(PopUpScoreEvent e)
    {
        _UIScore.SetActive(true);
        _ScoreText.text = e.highScore.Score;
    }
    
	
	// Update is called once per frame
	void Update () {
		
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class RecordData {
    public Text Name;
    public Text Score;

    public RecordData(Text name, Text score)
    {
        Name = name;
        Score = score;
    }
}

public class HighScoreManager : MonoBehaviour {

    [SerializeField]
    GameObject _SelesaiButton;

    [SerializeField]
    GameObject _UIScore;
    [SerializeField]
    Text _ScoreText;

    string _Score;
    bool _IsGame1;

    [SerializeField]
    GameObject _RecordUI;

    [SerializeField]
    List<RecordData> _RecordData = new List<RecordData>();
    [SerializeField]
    Image _ImageBG;
    // Use this for initialization
    private void Awake()
    {
        EventManager.AddListener<PopUpScoreEvent>(PopUpScoreHandler);
        EventManager.AddListener<ShowRecordEvent>(ShowRecord);
        EventManager.AddListener<CloseRecordEvent>(CloseRecord);

        _SelesaiButton.AddComponent<Button>().onClick.AddListener(delegate {
            SelesaiButtonHandler();

            EventManager.TriggerEvent(new SFXPlayEvent(SfxType.CLICK, false));
        });

        for (int i = 0; i < _RecordUI.transform.GetChild(1).GetChild(1).childCount; i++) {
            RecordData recordData = new RecordData(
                _RecordUI.transform.GetChild(1).GetChild(1).GetChild(i).GetChild(1).GetComponent<Text>(),
                _RecordUI.transform.GetChild(1).GetChild(1).GetChild(i).GetChild(2).GetComponent<Text>()
                );
            _RecordData.Add(recordData);
        }
    }
    
    void Start() {
        _UIScore.SetActive(false);
        _RecordUI.SetActive(false);
    }

    private void SelesaiButtonHandler()
    {
        if (Global.PlayerName == "")
            return;
        _UIScore.SetActive(false);
        EventManager.TriggerEvent(new ButtonActionEvent(ObjectType.MAIN_MENU));
        EventManager.TriggerEvent(new SaveHighScoreEvent(new HighScore(Global.PlayerName, _Score), _IsGame1));
    }

    private void PopUpScoreHandler(PopUpScoreEvent e)
    {
        _UIScore.SetActive(true);
        _ScoreText.text = e.Score;
        _Score = e.Score;
        _IsGame1 = e.IsGame1;

        if (e.IsWin)
            _ImageBG.color = new Color(0, 1, 0.1f, 0.4f);
        else
            _ImageBG.color = new Color(1, 0, 0, 0.4f);
    }

    private void ShowRecord(ShowRecordEvent e)
    {
        _RecordUI.SetActive(true);
        for (int i = 0; i < e.HighScore.Count; i++) {
            if (i < _RecordData.Count)
            {
                _RecordData[i].Name.text = e.HighScore[i].NamePlayer;
                _RecordData[i].Score.text = e.HighScore[i].Score;
            }
            else
                return;
        }
    }

    private void CloseRecord(CloseRecordEvent e)
    {
        _RecordUI.SetActive(false);
        for (int i = 0; i < _RecordData.Count; i++)
        {
            _RecordData[i].Name.text = "????????";
            _RecordData[i].Score.text = "?????";
        }
    }
}

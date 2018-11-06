using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour {

    [SerializeField]
    GameObject _PauseUI;
    [SerializeField]
    GameObject _ResumeButton;

    [SerializeField]
    GameObject _ExitButton;

    private void Awake()
    {
        _ResumeButton.AddComponent<Button>().onClick.AddListener(delegate {
            PauseHandler(false);
            EventManager.TriggerEvent(new SFXPlayEvent(SfxType.YES, false));
        });
        _ExitButton.AddComponent<Button>().onClick.AddListener(delegate {
            PauseHandler(false);
            ExitHandler();
            EventManager.TriggerEvent(new SFXPlayEvent(SfxType.NO, false));
        });
    }

    public void PauseHandler(bool isPause) {
        if (isPause)
        {
            _PauseUI.SetActive(true);
            Global.IsPause = true;
            Time.timeScale = 0.01f;
        }
        else
        {
            _PauseUI.SetActive(false);
            Global.IsPause = false;
            Time.timeScale = 1f;
        }
    }

    void ExitHandler() {
        EventManager.TriggerEvent(new ButtonActionEvent(ObjectType.MAIN_MENU));
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ButtonManager : MonoBehaviour {

    [Header("MAIN MENU")]
    [SerializeField]
    GameObject _StartGameBtn;
    [SerializeField]
    GameObject _ExitBtn;

    [Header("HIGHSCORE MENU")]
    [SerializeField]
    GameObject _HighscoreButton;
    [SerializeField]
    GameObject _ShowHighScore1Btn;
    [SerializeField]
    GameObject _ShowHighScore2Btn;
    [SerializeField]
    GameObject _CloseHighScore;


    [Header("SELECT CHARACTER")]
    [SerializeField]
    GameObject _DodoButton;
    [SerializeField]
    GameObject _LuluButton;

    [Header("SELECT GAME")]
    [SerializeField]
    GameObject _Game1Button;
    [SerializeField]
    GameObject _Game2Button;

    [Header("INTRO BUTTON")]
    [SerializeField]
    GameObject _SkipIntro1;
    [SerializeField]
    GameObject _SkipIntro2;

    [Header("TUTORIAL BUTTON")]
    [SerializeField]
    GameObject _SkipTutorial1;
    [SerializeField]
    GameObject _SkipTutorial2;

    Backeend _Backend;
    PauseManager _PauseManager;
    // Use this for initialization
    void Awake () {
        _Backend = GetComponent<Backeend>();
        _PauseManager = GetComponent<PauseManager>();

        //MAIN MENU
        _StartGameBtn.AddComponent<Button>().onClick.AddListener(delegate {
            EventManager.TriggerEvent(new ButtonActionEvent(ObjectType.SELECT_CHARACTER));

            EventManager.TriggerEvent(new SFXPlayEvent(SfxType.CLICK, false));
        });
        _ExitBtn.AddComponent<Button>().onClick.AddListener(delegate {
            ExitButton();

            EventManager.TriggerEvent(new SFXPlayEvent(SfxType.CLICK, false));
        });


        _HighscoreButton.AddComponent<Button>().onClick.AddListener(delegate {
            EventManager.TriggerEvent(new ButtonActionEvent(ObjectType.HIGH_SCORE));
        });
        _ShowHighScore1Btn.AddComponent<Button>().onClick.AddListener(delegate {
            EventManager.TriggerEvent(new ButtonActionEvent(ObjectType.HIGH_SCORE_FALSE));
            EventManager.TriggerEvent(new SFXPlayEvent(SfxType.YES, false));
            EventManager.TriggerEvent(new ShowRecordEvent(_Backend._Game1HighScore));
        });
        _ShowHighScore2Btn.AddComponent<Button>().onClick.AddListener(delegate {
            EventManager.TriggerEvent(new ButtonActionEvent(ObjectType.HIGH_SCORE_FALSE));
            EventManager.TriggerEvent(new SFXPlayEvent(SfxType.YES, false));
            EventManager.TriggerEvent(new ShowRecordEvent(_Backend._Game2HighScore));
        });
        _CloseHighScore.AddComponent<Button>().onClick.AddListener(delegate {
            EventManager.TriggerEvent(new SFXPlayEvent(SfxType.YES, false));
            EventManager.TriggerEvent(new CloseRecordEvent());
        });

        //SELECY T CHARACTER
        _DodoButton.AddComponent<Button>().onClick.AddListener(delegate {
            SelectCharacterButton(CharacterType.DODO);

            EventManager.TriggerEvent(new SFXPlayEvent(SfxType.CLICK, false));
        });
        _LuluButton.AddComponent<Button>().onClick.AddListener(delegate {
            SelectCharacterButton(CharacterType.NINA);

            EventManager.TriggerEvent(new SFXPlayEvent(SfxType.CLICK, false));
        });

        //SELECT GAME
        _Game1Button.AddComponent<Button>().onClick.AddListener(delegate {
            StartGameButton(GameType.GAME_1);

            EventManager.TriggerEvent(new SFXPlayEvent(SfxType.CLICK, false));
        });
        _Game2Button.AddComponent<Button>().onClick.AddListener(delegate {
            StartGameButton(GameType.GAME_2);

            EventManager.TriggerEvent(new SFXPlayEvent(SfxType.CLICK, false));
        });

        //SKIP INTRO
        _SkipIntro1.AddComponent<Button>().onClick.AddListener(delegate {
            EventManager.TriggerEvent(new ButtonActionEvent(ObjectType.TUTORIAL_GAME));

            EventManager.TriggerEvent(new SFXPlayEvent(SfxType.CLICK, false));
        });
        _SkipIntro2.AddComponent<Button>().onClick.AddListener(delegate {
            EventManager.TriggerEvent(new ButtonActionEvent(ObjectType.TUTORIAL_GAME));

            EventManager.TriggerEvent(new SFXPlayEvent(SfxType.CLICK, false));
        });

        //SKIP TUTORIAL
        _SkipTutorial1.AddComponent<Button>().onClick.AddListener(delegate {
            EventManager.TriggerEvent(new ButtonActionEvent(ObjectType.PLAY_GAME));

            EventManager.TriggerEvent(new SFXPlayEvent(SfxType.CLICK, false));
        });
        _SkipTutorial2.AddComponent<Button>().onClick.AddListener(delegate {
            EventManager.TriggerEvent(new ButtonActionEvent(ObjectType.PLAY_GAME));

            EventManager.TriggerEvent(new SFXPlayEvent(SfxType.CLICK, false));
        });
    }

    void SelectCharacterButton (CharacterType type)
    {
        EventManager.TriggerEvent(new SelectCharacterEvent(type));
        EventManager.TriggerEvent(new ButtonActionEvent(ObjectType.SELECT_GAME));
    }

    void StartGameButton(GameType type)
    {
        EventManager.TriggerEvent(new SelectGameEvent(type));
        EventManager.TriggerEvent(new ButtonActionEvent(ObjectType.INTRO_GAME));
    }


    void OptionButton()
    {
        Debug.Log("Option");
    }
    void ExitButton()
    {
        Application.Quit();
        Debug.Log("Exit");
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape)) {
            if (!Global.IsPause)
                _PauseManager.PauseHandler(true);
            else
                _PauseManager.PauseHandler(false);
        }
    }
}

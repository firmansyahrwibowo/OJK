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
    GameObject _OptionBtn;
    [SerializeField]
    GameObject _ExitBtn;


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

    // Use this for initialization
    void Awake () {
        //MAIN MENU
        _StartGameBtn.AddComponent<Button>().onClick.AddListener(delegate {
            EventManager.TriggerEvent(new ButtonActionEvent(ObjectType.SELECT_CHARACTER));
        });
        _OptionBtn.AddComponent<Button>().onClick.AddListener(delegate {
            OptionButton();
        });
        _ExitBtn.AddComponent<Button>().onClick.AddListener(delegate {
            ExitButton();
        });

        //SELECY T CHARACTER
        _DodoButton.AddComponent<Button>().onClick.AddListener(delegate {
            SelectCharacterButton(CharacterType.DODO);
        });
        _LuluButton.AddComponent<Button>().onClick.AddListener(delegate {
            SelectCharacterButton(CharacterType.NINA);
        });

        //SELECT GAME
        _Game1Button.AddComponent<Button>().onClick.AddListener(delegate {
            StartGameButton(GameType.GAME_1);
        });
        _Game2Button.AddComponent<Button>().onClick.AddListener(delegate {
            StartGameButton(GameType.GAME_2);
        });

        //SKIP INTRO
        _SkipIntro1.AddComponent<Button>().onClick.AddListener(delegate {
            EventManager.TriggerEvent(new ButtonActionEvent(ObjectType.TUTORIAL_GAME));
        });
        _SkipIntro2.AddComponent<Button>().onClick.AddListener(delegate {
            EventManager.TriggerEvent(new ButtonActionEvent(ObjectType.TUTORIAL_GAME));
        });

        //SKIP TUTORIAL
        _SkipTutorial1.AddComponent<Button>().onClick.AddListener(delegate {
            EventManager.TriggerEvent(new ButtonActionEvent(ObjectType.PLAY_GAME));
        });
        _SkipTutorial2.AddComponent<Button>().onClick.AddListener(delegate {
            EventManager.TriggerEvent(new ButtonActionEvent(ObjectType.PLAY_GAME));
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
}

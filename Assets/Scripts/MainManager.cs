using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour {

    [SerializeField]
    GameObject _MainMenu;
    [SerializeField]
    GameObject _SelectCharacter;
    [SerializeField]
    GameObject _SelectGame;

    //GAME 1
    [Header("Game 1 Component")]
    [SerializeField]
    GameObject _Game1;
    [SerializeField]
    GameObject _Intro1;
    [SerializeField]
    GameObject _Intro2;
    [SerializeField]
    GameObject _Tutorial1;
    [SerializeField]
    GameObject _Tutorial2;
    
    //GAME 2
    [Header("Game 2 Component")]
    [SerializeField]
    GameObject _Game2;

    [SerializeField]
    GameObject _HighscoreGroup;
    [SerializeField]
    GameObject _HighscoreUI;

    public CharacterType CharacterSelected;
    public GameType GameSelected;

    Game1Manager _Game1Manager;
    Game2Manager _Game2Manager;

    
    void Awake () {
        EventManager.AddListener<ButtonActionEvent>(ButtonActionHandler);
        EventManager.AddListener<SelectCharacterEvent>(SelectCharacterHandler);
        EventManager.AddListener<SelectGameEvent>(SelectGameHandler);
        
        //initialization of this code
        //find the reference of code by searching children
        _Game1Manager = GetComponentInChildren<Game1Manager>();
        _Game2Manager = GetComponentInChildren<Game2Manager>();
    }

    private void Start()
    {
        EventManager.TriggerEvent(new HoldOnEvent(false));
        _MainMenu.SetActive(true);
        _SelectCharacter.SetActive(false);
        _SelectGame.SetActive(false);
        _Intro1.SetActive(false);
        _Intro2.SetActive(false);
        _Tutorial1.SetActive(false);
        _Tutorial2.SetActive(false);
        _Game1.SetActive(false);
        _Game2.SetActive(false);

        EventManager.TriggerEvent(new BGMEvent(BGMType.MAIN_MENU));
    }

    private void ButtonActionHandler(ButtonActionEvent e)
    {
        switch (e.ObjType) {
            case ObjectType.MAIN_MENU:
                EventManager.TriggerEvent(new HoldOnEvent(false));
                _MainMenu.SetActive(true);
                _SelectCharacter.SetActive(false);
                _SelectGame.SetActive(false);
                _Intro1.SetActive(false);
                _Intro2.SetActive(false);
                _Tutorial1.SetActive(false);
                _Tutorial2.SetActive(false);
                _Game1.SetActive(false);
                _Game2.SetActive(false);
                _HighscoreGroup.SetActive(false);
                _HighscoreUI.SetActive(false);

                EventManager.TriggerEvent(new BGMEvent(BGMType.MAIN_MENU));
                break;
            case ObjectType.HIGH_SCORE:
                _HighscoreGroup.SetActive(true);
                break;
            case ObjectType.HIGH_SCORE_FALSE:
                _HighscoreGroup.SetActive(false);
                break;
            case ObjectType.SELECT_CHARACTER:
                _SelectGame.SetActive(false);
                _SelectCharacter.SetActive(true);
                break;
            case ObjectType.SELECT_GAME:
                _SelectCharacter.SetActive(false);
                _SelectGame.SetActive(true);
                break;
            case ObjectType.INTRO_GAME:
                _MainMenu.SetActive(false);
                _SelectCharacter.SetActive(false);
                _SelectGame.SetActive(false);
                

                if (GameSelected == GameType.GAME_1)
                    _Intro1.SetActive(true);
                else
                    _Intro2.SetActive(true);
                
                break;
            case ObjectType.TUTORIAL_GAME:

                if (GameSelected == GameType.GAME_1)
                {
                    _Intro1.SetActive(false);
                    _Tutorial1.SetActive(true);
                }
                else
                {
                    _Intro2.SetActive(false);
                    _Tutorial2.SetActive(true);
                }
                break;
            case ObjectType.PLAY_GAME:
                EventManager.TriggerEvent(new KeyboardInitEvent());

                EventManager.TriggerEvent(new SFXPlayEvent(SfxType.START_GAME, true));
                if (GameSelected == GameType.GAME_1)
                {
                    EventManager.TriggerEvent(new InitCharacterManagerEvent(CharacterSelected));

                    _Tutorial1.SetActive(false);
                    _Game1.SetActive(true);
                    _Game1Manager.Init();
                    EventManager.TriggerEvent(new BGMEvent(BGMType.GAMEPLAY_1));
                }
                else
                {
                    EventManager.TriggerEvent(new InitCharacterManagerEvent(CharacterSelected));

                    _Tutorial2.SetActive(false);
                    _Game2.SetActive(true);
                    _Game2Manager.Init();
                    EventManager.TriggerEvent(new BGMEvent(BGMType.GAMEPLAY_2));
                }

                break;
        }
    }

    private void SelectGameHandler(SelectGameEvent e)
    {
        GameSelected = e.Type;
    }

    private void SelectCharacterHandler(SelectCharacterEvent e)
    {
        CharacterSelected = e.Type;
    }

}

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

    public CharacterType CharacterSelected;
    public GameType GameSelected;

    Game1Manager _Game1Manager;
    Game2Manager _Game2Manager;
    // Use this for initialization
    void Awake () {
        EventManager.AddListener<ButtonActionEvent>(ButtonActionHandler);
        EventManager.AddListener<SelectCharacterEvent>(SelectCharacterHandler);
        EventManager.AddListener<SelectGameEvent>(SelectGameHandler);

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
                break;
            case ObjectType.SELECT_CHARACTER:
                _MainMenu.SetActive(false);
                _SelectGame.SetActive(false);

                _SelectCharacter.SetActive(true);
                break;
            case ObjectType.SELECT_GAME:
                _MainMenu.SetActive(false);
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
                if (GameSelected == GameType.GAME_1)
                {
                    _Tutorial1.SetActive(false);
                    _Game1.SetActive(true);
                    _Game1Manager.Init();
                }
                else
                {
                    _Tutorial2.SetActive(false);
                    _Game2.SetActive(true);
                    _Game2Manager.Init();
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

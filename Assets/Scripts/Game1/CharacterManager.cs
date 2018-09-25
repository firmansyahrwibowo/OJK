using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameplayCharacter {
    public GameObject Object;
    public CharacterType Type;
    public Animator Animator;
    public GameObject WinObject;
    public GameObject LoseObject;

    public GameplayCharacter(GameObject @object, CharacterType type, Animator animator, GameObject winObject, GameObject loseObject)
    {
        Object = @object;
        Type = type;
        Animator = animator;
        WinObject = winObject;
        LoseObject = loseObject;
    }
}


public class CharacterManager : MonoBehaviour {

    [SerializeField]
    GameplayCharacter [] _CharacterList;
    GameplayCharacter _Character;
    [SerializeField]
    GameplayCharacter _Enemy;
    
    // Use this for initialization
    private void Awake()
    {
        EventManager.AddListener<InitCharacterManagerEvent>(InitCharacterManager);
        EventManager.AddListener<ResultCharacterEvent>(ResultAnimation);
    }

    void InitCharacterManager (InitCharacterManagerEvent e) {
        for (int i = 0; i < _CharacterList.Length; i++) {
            if (e.Type == _CharacterList[i].Type)
            {
                _Character = new GameplayCharacter(_CharacterList[i].Object, _CharacterList[i].Type, _CharacterList[i].Animator, _CharacterList[i].WinObject, _CharacterList[i].LoseObject);
                _Character.Object.SetActive(true);
                _Character.WinObject.SetActive(false);
                _Character.LoseObject.SetActive(false);
            }
            else
            {
                _CharacterList[i].Object.SetActive(false);
                _CharacterList[i].WinObject.SetActive(false);
                _CharacterList[i].LoseObject.SetActive(false);
            }
        }

        _Enemy.Object.SetActive(true);
        _Enemy.WinObject.SetActive(false);
        _Enemy.LoseObject.SetActive(false);
    }

    void ResultAnimation (ResultCharacterEvent e) {
        if (e.Type == ResultType.WIN)
        {
            //PLAYER ACTION
            _Character.Object.SetActive(false);
            _Character.WinObject.SetActive(true);
            _Character.LoseObject.SetActive(false);

            //ENEMY ACTION
            _Enemy.Object.SetActive(false);
            _Enemy.WinObject.SetActive(false);
            _Enemy.LoseObject.SetActive(true);
        }
        else
        {
            //PLAYER ACTION
            _Character.Object.SetActive(false);
            _Character.WinObject.SetActive(false);
            _Character.LoseObject.SetActive(true);

            //ENEMY ACTION
            _Enemy.Object.SetActive(false);
            _Enemy.WinObject.SetActive(true);
            _Enemy.LoseObject.SetActive(false);
        }

    }
}

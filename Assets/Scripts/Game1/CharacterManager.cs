using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class GameplayCharacter {
    public GameObject Object;
    public CharacterType Type;
    public Animator Animator;
    public GameObject WinObject;
    public GameObject LoseObject;
    public Image Head;
    public Sprite IdleFace;
    public Sprite SadFace;
    public Sprite HappyFace;
    public Sprite AnxiousFace;

    public GameplayCharacter(GameObject @object, CharacterType type, Animator animator, GameObject winObject, GameObject loseObject, Image head, Sprite idleFace, Sprite sadFace, Sprite happyFace, Sprite anxiousFace)
    {
        Object = @object;
        Type = type;
        Animator = animator;
        WinObject = winObject;
        LoseObject = loseObject;
        Head = head;
        IdleFace = idleFace;
        SadFace = sadFace;
        HappyFace = happyFace;
        AnxiousFace = anxiousFace;
    }
}


public class CharacterManager : MonoBehaviour {

    [SerializeField]
    GameplayCharacter [] _CharacterList;
    GameplayCharacter _Character;
    [SerializeField]
    GameplayCharacter _Enemy;

    Vector2 _CharacterDefaultPosition;
    Vector2 _EnemyDefaultPosition;

    [SerializeField]
    GameObject _CharacterAnxious;
    [SerializeField]
    GameObject _EnemyAnxious;

    FaceEvent _LastFace;
    // Use this for initialization
    private void Awake()
    {
        EventManager.AddListener<InitCharacterManagerEvent>(InitCharacterManager);
        EventManager.AddListener<ResultCharacterEvent>(ResultAnimation);
        EventManager.AddListener<FaceEvent>(FaceHandler);
    }


    void InitCharacterManager (InitCharacterManagerEvent e)
    {
        for (int i = 0; i < _CharacterList.Length; i++) {
            if (e.Type == _CharacterList[i].Type)
            {
                _Character = new GameplayCharacter(_CharacterList[i].Object, _CharacterList[i].Type, _CharacterList[i].Animator, _CharacterList[i].WinObject, _CharacterList[i].LoseObject, _CharacterList[i].Head, _CharacterList[i].IdleFace, _CharacterList[i].SadFace, _CharacterList[i].HappyFace, _CharacterList[i].AnxiousFace);
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
        _LastFace = new FaceEvent(FaceType.IDLE, true);
        FaceHandler(_LastFace);
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


    void FaceHandler(FaceEvent e) {
        _LastFace = new FaceEvent(FaceType.IDLE, true);
        switch (e.Type) {
            case FaceType.IDLE:
                _Character.Head.sprite = _Character.IdleFace;
                _Enemy.Head.sprite = _Enemy.IdleFace;

                _LastFace = e;
                break;
            case FaceType.HAPPY:
                if (e.IsTrue)
                {
                    _Character.Head.sprite = _Character.HappyFace;
                    _Enemy.Head.sprite = _Enemy.SadFace;
                }
                else
                {
                    _Character.Head.sprite = _Character.SadFace;
                    _Enemy.Head.sprite = _Enemy.HappyFace;
                }
                StartCoroutine(ResetFace());
                break;
            case FaceType.ANXIOUS:
                if (e.IsTrue)
                {
                    _Character.Head.sprite = _Character.HappyFace;
                    _Enemy.Head.sprite = _Enemy.AnxiousFace;
                }
                else
                {
                    _Character.Head.sprite = _Character.AnxiousFace;
                    _Enemy.Head.sprite = _Enemy.HappyFace;
                }

                _LastFace = e;
                break;
        }
    }

    IEnumerator ResetFace() {
        yield return new WaitForSeconds(1);
        FaceHandler(new FaceEvent(_LastFace.Type, _LastFace.IsTrue));
    }
}

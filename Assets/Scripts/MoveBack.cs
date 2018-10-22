using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveBack : MonoBehaviour {

    RectTransform _Trans;
    Vector2 _DefaultPos;

    [SerializeField]
    float _Speed = 5f;
    [SerializeField]
    float _EndPos;
    [SerializeField]
    float _StartPos;

    Tween _Tween;
    // Use this for initialization
    private void Awake()
    {
        _Trans = GetComponent<RectTransform>();
        _DefaultPos = _Trans.anchoredPosition;
    }
    void Start () {
        _Trans.anchoredPosition = _DefaultPos;
        StartTween();
	}

    void StartTween() {
        _Tween = _Trans.DOAnchorPosX(_EndPos, _Speed).OnComplete(RestartObject);
    }

    void RestartObject() {
        _Trans.anchoredPosition = new Vector2(_StartPos, _Trans.anchoredPosition.y);
        if (_Tween != null)
            _Tween.Kill(false);

        StartTween();
    }
	
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldScreen : MonoBehaviour {

    [SerializeField]
    GameObject _ScreenHold;
	// Use this for initialization
	void Awake () {
        EventManager.AddListener<HoldOnEvent>(HoldHandler);
	}

    private void HoldHandler(HoldOnEvent e)
    {
        if (e.IsHold)
            _ScreenHold.SetActive(true);
        else
            _ScreenHold.SetActive(false);

    }
}

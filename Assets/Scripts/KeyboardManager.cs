using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardManager : MonoBehaviour {

    [SerializeField]
    GameObject _KeyboardObject;
    [SerializeField]
    Text _KeyboardText;
    
	// Use this for initialization
	void Awake ()
    {
        EventManager.AddListener<KeyboardTypeEvent>(TypeKeyboard);
        EventManager.AddListener<KeyboardInitEvent>(Init);
        
    }

    public void Init(KeyboardInitEvent e)
    {
        Global.PlayerName = "";
        _KeyboardText.text = "";
        _KeyboardObject.SetActive(true);
    }

    // Update is called once per frame
    void TypeKeyboard(KeyboardTypeEvent e)
    {
        if (!_KeyboardObject.activeInHierarchy)
            return;
        switch (e.KeyCode) {
            case "DEL":
                string text = "";
                for (int i = 0; i < Global.PlayerName.Length - 1; i++) {
                    text = text + "" + Global.PlayerName[i];
                }
                Global.PlayerName = text;
                _KeyboardText.text = Global.PlayerName;
                break;
            case "SPACE":
                Global.PlayerName = Global.PlayerName + " ";
                _KeyboardText.text = Global.PlayerName;
                break;
            case "ENTER":
                if (Global.PlayerName == "")
                    return;
                _KeyboardObject.SetActive(false);
                //DONE N SAVE
                break;
            default:
                Global.PlayerName = Global.PlayerName + "" + e.KeyCode;
                _KeyboardText.text = Global.PlayerName;
                break;
        }
        EventManager.TriggerEvent(new SFXPlayEvent(SfxType.TYPE, true));
    }
}

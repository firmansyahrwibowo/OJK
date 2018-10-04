using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class KeyboardType : MonoBehaviour
{
    [SerializeField]
    string _Key;
	
    Image _ThisImage;
    EventTrigger _EventTrigger;
    // Use this for initialization
    void Awake ()
    {
        _ThisImage = GetComponent<Image>();
        _ThisImage.color = new Color(1, 1, 1, 1);
        gameObject.AddComponent<Button>().onClick.AddListener(delegate
        {
            KeyboardClick();
        });

        _EventTrigger = gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry pointerDown = new EventTrigger.Entry();
        EventTrigger.Entry pointerUp = new EventTrigger.Entry();

        pointerDown.eventID = EventTriggerType.PointerDown;
        pointerUp.eventID = EventTriggerType.PointerUp;

        pointerDown.callback.AddListener((eventData) =>
        {
            ImageAction(true);
        });
        _EventTrigger.triggers.Add(pointerDown);


        pointerUp.callback.AddListener((eventData) =>
        {
            ImageAction(false);
        });
        _EventTrigger.triggers.Add(pointerUp);

        _Key = gameObject.name;
    }

    void KeyboardClick()
    {
        EventManager.TriggerEvent(new KeyboardTypeEvent(_Key));
    }

    private void ImageAction(bool isClick)
    {
        if (isClick)
        {
            _ThisImage.color = new Color(1, 1, 1, 0.5f);
        }
        else
        {
            _ThisImage.color = new Color(1, 1, 1, 1);
        }
    }
}

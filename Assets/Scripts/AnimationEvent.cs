using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour {

    public void OnMainMenuIntroEnd()
    {
        EventManager.TriggerEvent(new ButtonActionEvent(ObjectType.SKIP_INTRO_MAIN_MENU));
    }

    public void OnIntroEnd() {
        EventManager.TriggerEvent(new ButtonActionEvent(ObjectType.TUTORIAL_GAME));
    }
    public void OnTutorialEnd()
    {
        EventManager.TriggerEvent(new ButtonActionEvent(ObjectType.TUTORIAL_GAME));
    }
}

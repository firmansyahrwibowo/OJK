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
    public void OnThrilledBGM()
    {
        EventManager.TriggerEvent(new BGMEvent(BGMType.THRILL));
    }
    public void OnCountingEvent()
    {
        EventManager.TriggerEvent(new SFXPlayEvent(SfxType.NO, true));
    }
    public void OnStartGameEvent()
    {
        EventManager.TriggerEvent(new SFXPlayEvent(SfxType.START_GAME, true));
    }
}

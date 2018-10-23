using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonActionEvent : GameEvent
{
    public ObjectType ObjType;

    public ButtonActionEvent(ObjectType objType)
    {
        ObjType = objType;
    }
}
public class SelectCharacterEvent : GameEvent
{
    public CharacterType Type;

    public SelectCharacterEvent(CharacterType type)
    {
        Type = type;
    }
}
public class SelectGameEvent : GameEvent
{
    public GameType Type;

    public SelectGameEvent(GameType type)
    {
        Type = type;
    }
}
public class SaveHighScoreEvent : GameEvent
{
    public HighScore highScore;
    public bool IsGame1;

    public SaveHighScoreEvent(HighScore highScore, bool isGame1)
    {
        this.highScore = highScore;
        IsGame1 = isGame1;
    }
}

public class HoldOnEvent : GameEvent {
    public bool IsHold;

    public HoldOnEvent(bool isHold)
    {
        IsHold = isHold;
    }
}
public class ResetHighscoreEvent : GameEvent { }

public class PopUpScoreEvent : GameEvent {
    public string Score;
    public bool IsGame1;
    public bool IsWin;

    public PopUpScoreEvent(string score, bool isGame1, bool isWin = true)
    {
        Score = score;
        IsGame1 = isGame1;
        IsWin = isWin;
    }
}

public class InitCharacterManagerEvent : GameEvent
{
    public CharacterType Type;

    public InitCharacterManagerEvent(CharacterType type)
    {
        Type = type;
    }
}
public class ResultCharacterEvent : GameEvent
{
    public ResultType Type;

    public ResultCharacterEvent(ResultType type)
    {
        Type = type;
    }
}
public class SFXPlayEvent : GameEvent
{
    public SfxType Sfx;
    public bool IsEnd;

    public SFXPlayEvent(SfxType sfx, bool isEnd)
    {
        Sfx = sfx;
        IsEnd = isEnd;
    }
}

public class BGMEvent : GameEvent {
    public BGMType Type;

    public BGMEvent(BGMType type)
    {
        Type = type;
    }
}
public class KeyboardTypeEvent : GameEvent
{
    public string KeyCode;

    public KeyboardTypeEvent(string keyCode)
    {
        KeyCode = keyCode;
    }
}
public class KeyboardInitEvent : GameEvent
{

}
public class ShowRecordEvent : GameEvent
{
    public List<HighScore> HighScore;

    public ShowRecordEvent(List<HighScore> highScore)
    {
        HighScore = highScore;
    }
}

public class CloseRecordEvent : GameEvent
{

}
public class FaceEvent : GameEvent
{
    public FaceType Type;
    public bool IsTrue;

    public FaceEvent(FaceType type, bool isTrue)
    {
        Type = type;
        IsTrue = isTrue;
    }
}
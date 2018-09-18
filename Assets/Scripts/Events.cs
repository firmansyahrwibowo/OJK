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

public class PopUpScoreEvent : GameEvent {
    public HighScore highScore;

    public PopUpScoreEvent(HighScore highScore)
    {
        this.highScore = highScore;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ButtonType
{
    START_GAME,
    OPTION,
    EXIT
}
public enum CharacterType
{
    DODO, NINA, ENEMY
}

public enum ResultType
{
    WIN, LOSE
}
public enum GameType
{
    GAME_1, GAME_2
}
public enum ObjectType
{
    MAIN_MENU,
    SELECT_CHARACTER,
    SELECT_GAME,
    PLAY_GAME,
    INTRO_GAME,
    TUTORIAL_GAME
}
public enum OptionType {
    OPTION_A,
    OPTION_B,
    OPTION_C,
    OPTION_D,
}

public enum ObjectName {
    BATIK, //1
    BERLIAN, //2
    EMAS, //3
    GOLF, //4
    JAM, //5
    KADO, //6
    KALUNG, //7
    KANTUNG_RP, //8
    KOIN, //9
    KOPER, //10
    PARCEL, //11
    RAKET, //12
    RP_SUAP, //13
    VERBODEN_RP, //14
    VOUCHER, //15

    BLADE_TRAIL//16
}

public enum OrderType {
    ASCENDING, DESCENDING
}
public enum DataType
{
    STRING, FLOAT
}
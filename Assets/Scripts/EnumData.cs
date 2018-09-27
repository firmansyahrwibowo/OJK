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
    DOLAR_SUAP, //0
    BATIK, //1
    BERLIAN, //2
    EMAS, //3
    GOLF, //4
    JAM, //5
    KADO, //6
    KALUNG, //7
    KANTUNG_DOLAR, //8
    KANTUNG_RP, //9
    KOIN, //10
    KOPER, //11
    PARCEL, //12
    RAKET, //13
    RP_SUAP, //14
    VERBODEN_DOLAR, //15
    VERBODEN_RP, //16
    VOUCHER, //17

    BLADE_TRAIL//18
}

public enum OrderType {
    ASCENDING, DESCENDING
}
public enum DataType
{
    STRING, FLOAT
}
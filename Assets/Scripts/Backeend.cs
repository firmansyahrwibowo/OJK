using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class HighScore {
    public string NamePlayer;
    public string Score;

    public HighScore(string nama, string score) {
        NamePlayer = nama;
        Score = score;
    }
}

public class Backeend : MonoBehaviour {
    
    public List<HighScore> _Game1HighScore = new List<HighScore>();
    public List<HighScore> _Game2HighScore = new List<HighScore>();

    HighScore[] _HighScoreArray;

    string _BackeendDir = "_Data";

    void Awake () {
        EventManager.AddListener<SaveHighScoreEvent>(SaveHighScore);
    }

    void Start()
    {
        if (!Directory.Exists(_BackeendDir))
            Directory.CreateDirectory(_BackeendDir);

        LoadAllHighScore();
    }

    #region Save
    public void SaveHighScore(SaveHighScoreEvent e)
    {
        string fileName = "";
        string filePath = "";
        string json = "";

        if (e.IsGame1)
        {
            fileName = "Game1HighScore.fyr";
            _Game1HighScore.Add(e.highScore);
            _HighScoreArray = _Game1HighScore.ToArray();

            MySort(OrderType.DESCENDING, DataType.FLOAT);
        }
        else
        {
            fileName = "Game2HighScore.fyr";
            _Game2HighScore.Add(e.highScore);
            _HighScoreArray = _Game2HighScore.ToArray();

            MySort(OrderType.ASCENDING, DataType.FLOAT);
        }

        filePath = _BackeendDir +"/"+fileName;


        json = JsonHelper.ToJson(_HighScoreArray, true);        //JsonUtility.ToJson(scoreData);
        File.WriteAllText(filePath, json);

        LoadAllHighScore();
    }
    #endregion

    #region LOAD
    public void LoadAllHighScore( )
    {
        string filePath = "";
        string fileName = "";
        
        fileName = "Game1HighScore.fyr";
        
        filePath = _BackeendDir + "/" + fileName;

        if (File.Exists(filePath))
        {
            _Game1HighScore = new List<HighScore>();
            string json = File.ReadAllText(filePath);
            HighScore[] highScore = JsonHelper.FromJson<HighScore>(json);
            _Game1HighScore = highScore.ToList();
        }

        fileName = "Game2HighScore.fyr";

        filePath = _BackeendDir + "/" + fileName;

        if (File.Exists(filePath))
        {
            _Game2HighScore = new List<HighScore>();
            string json = File.ReadAllText(filePath);
            HighScore[] highScore = JsonHelper.FromJson<HighScore>(json);
            _Game2HighScore = highScore.ToList();
        }
    }
    #endregion

    #region SORTING
    void MySort (OrderType Order, DataType By)
    {
        if (Order == OrderType.ASCENDING)
        {
            if (By == DataType.STRING)
            {
                System.Array.Sort(_HighScoreArray, delegate (HighScore thing1, HighScore thing2) {
                    return thing1.NamePlayer.CompareTo(thing2.NamePlayer);
                });
            }
            else if (By == DataType.FLOAT)
            {
                System.Array.Sort(_HighScoreArray, delegate (HighScore thing1, HighScore thing2) {
                    return Int32.Parse(thing1.Score).CompareTo(Int32.Parse(thing2.Score));
                });
            }
        }
        else
        {
            if (By == DataType.STRING)
            {
                System.Array.Sort(_HighScoreArray, delegate (HighScore thing1, HighScore thing2) {
                    return thing2.NamePlayer.CompareTo(thing1.NamePlayer);
                });
            }
            else if (By == DataType.FLOAT)
            {
                System.Array.Sort(_HighScoreArray, delegate (HighScore thing1, HighScore thing2) {
                    return Int32.Parse(thing2.Score).CompareTo(Int32.Parse(thing1.Score));
                });
            }
        }
    }
    #endregion
    #region ARRAY_JSON
    public class JsonHelper
    {
        public static T[] FromJson<T>(string json)
        {
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.data;
        }

        public static string ToJson<T>(T[] array)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.data = array;
            return JsonUtility.ToJson(wrapper);
        }

        public static string ToJson<T>(T[] array, bool prettyPrint)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.data = array;
            return JsonUtility.ToJson(wrapper, prettyPrint);
        }
    }
    [Serializable]
    private class Wrapper<T>
    {
        public T[] data;
    }
    #endregion
}

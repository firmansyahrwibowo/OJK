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

    [SerializeField]
    private List<HighScore> _Game1HighScore = new List<HighScore>();
    [SerializeField]
    private List<HighScore> _Game2HighScore = new List<HighScore>();

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
            fileName = "Game1HighScore.fyr";
        else
            fileName = "Game2HighScore.fyr";


        filePath = _BackeendDir +"/"+fileName;

        //Game1HighScore.Add(data);
        json = JsonHelper.ToJson(_Game1HighScore.ToArray(), true);        //JsonUtility.ToJson(scoreData);

        File.WriteAllText(filePath, json);
        
    }
    #endregion

    #region LOAD
    public void LoadAllHighScore( )
    {
        string path = "";
        string fileName = "";
        
        fileName = "Game1HighScore.fyr";
        
        string filePath = _BackeendDir + "/" + fileName;

        if (File.Exists(filePath))
        {
            _Game1HighScore = new List<HighScore>();
            string json = File.ReadAllText(filePath);
            HighScore[] highScore = JsonHelper.FromJson<HighScore>(json);
            _Game1HighScore = highScore.ToList();
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

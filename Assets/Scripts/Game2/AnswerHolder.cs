using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnswerHolder : MonoBehaviour
{
    public string AnswerContent;
    public bool IsTrue;
    public Text AnswerText;
    public Game2Manager Manager;
    // Use this for initialization
    void Start () {
		
	}

    public void Clicked()
    {
        Manager.Next();
    }
}

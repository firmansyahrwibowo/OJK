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
		Manager.CurrentQuestion += 1;
		if (IsTrue) {
			Manager.QuestionAnswered += 1;
			EventManager.TriggerEvent (new SFXPlayEvent (SfxType.BENAR, false));
			Manager.BenarPopUp.SetActive (true);
		} else {
			EventManager.TriggerEvent (new SFXPlayEvent (SfxType.SALAH, false));
			Manager.SalahPopUp.SetActive (true);
		}
    }
}

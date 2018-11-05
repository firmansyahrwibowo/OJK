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
	public Image durationFill;
	public Sprite NormalButton;
	public Sprite GreenButton;
	public Sprite RedButton;
	public GameObject ThisQuestion;
	public GameObject NextQuestion;
	public GameObject BlockSpam;
    // Use this for initialization
    void Start () {
		
	}

    public void Clicked()
    {
		Manager.CurrentQuestion += 1;
		BlockSpam.SetActive (true);
		if (IsTrue) {
			Manager.QuestionAnswered += 1;
			EventManager.TriggerEvent (new SFXPlayEvent (SfxType.BENAR, false));
			this.gameObject.GetComponent<Image> ().sprite = GreenButton;
			Manager.BenarPopUp.SetActive (true);
			durationFill.fillAmount += 0.33f;
		} else {
			EventManager.TriggerEvent (new SFXPlayEvent (SfxType.SALAH, false));
			this.gameObject.GetComponent<Image> ().sprite = RedButton;
			Manager.SalahPopUp.SetActive (true);
		}
    }

	public void	Result()
	{
		StartCoroutine (WaitForResult ());
	}
		

	public IEnumerator WaitForResult()
	{
		yield return new WaitForSeconds(1f);
		if (Manager.CurrentQuestion < 3) 
		{
			ThisQuestion.SetActive (false);
			NextQuestion.SetActive (true);
			BlockSpam.SetActive (false);
			this.gameObject.GetComponent<Image> ().sprite = NormalButton;
		} 
		else 
		{
			ThisQuestion.SetActive (false);
			this.gameObject.GetComponent<Image> ().sprite = NormalButton;
		}
	}
}

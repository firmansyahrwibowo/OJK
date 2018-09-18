using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BismaEvent;

public class Fruit : MonoBehaviour {

	public GameObject fruitSlicedPrefab;
	public float startForce = 15f;
    public int scoreGet;
    public int durationCut;
    //private Game2Manager _GameManager;
	Rigidbody2D rb;

	void Start ()
	{
		rb = GetComponent<Rigidbody2D>();
		rb.AddForce(transform.up * startForce, ForceMode2D.Impulse);
        //_GameManager = GameObject.Find("GAMEMANAGER").GetComponent<Game2Manager>();

    }

	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.tag == "Blade")
		{
			GameObject slicedFruit = Instantiate(fruitSlicedPrefab,transform.position,transform.rotation);
			Destroy(slicedFruit, 3f);

            EventManager.TriggerEvent(new DurationCutEvent(durationCut));
            EventManager.TriggerEvent(new ScoreSetEvent(scoreGet));
            //_GameManager.time -= durationCut;
            //_GameManager.Score += scoreGet;

			Destroy(gameObject);
		}
	}

}

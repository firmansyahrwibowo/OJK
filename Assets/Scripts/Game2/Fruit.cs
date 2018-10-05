using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BismaEvent;

public class Fruit : MonoBehaviour {

	public GameObject fruitSlicedPrefab;
	public float startForce;
    public int scoreGet;
    public int durationCut;
    //private Game2Manager _GameManager;
	public Rigidbody2D rb;

    Coroutine _ThisCoroutine;
    Coroutine _SlashedCoroutine;
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void Init ()
    {
        float force = Random.Range(12.1f, 14.1f);
        rb.AddForce(transform.up * force, ForceMode2D.Impulse);
    }

	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.tag == "Blade")
		{
            GameObject slice = PoolingObject.Instance.GetSlicedObject(gameObject.name);
            if (slice != null)
            {
                slice.transform.localPosition = transform.localPosition;
                //slice.transform.localRotation = transform.localRotation;
            }
            

            //GameObject slicedFruit = Instantiate(fruitSlicedPrefab,transform.position,transform.rotation);
            //Destroy(slicedFruit, 1f);


            EventManager.TriggerEvent(new DurationCutEvent(durationCut));
            EventManager.TriggerEvent(new ScoreSetEvent(scoreGet));
            //_GameManager.time -= durationCut;
            //_GameManager.Score += scoreGet;

            //gameObject.SetActive(false);
            DisableActivation();
		}
	}

    void DisableActivation() {
        transform.localPosition = new Vector3(1000, 1000, 1000);
    }

    void OnEnable ()
    {
        if (_ThisCoroutine != null)
            StopCoroutine(_ThisCoroutine);

        _ThisCoroutine = StartCoroutine(DestroyThis());
    }

    IEnumerator DestroyThis()
    {
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        if (_ThisCoroutine != null)
            StopCoroutine(_ThisCoroutine);
    }
}

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
	public Rigidbody2D rb;

    Coroutine _ThisCoroutine;
    Coroutine _SlashedCoroutine;

    private Game2Manager _Manager;
    private GameObject _DecreaseTimeEffect;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _Manager = GameObject.Find("Game2Manager").GetComponent<Game2Manager>();
        _DecreaseTimeEffect = _Manager.DurationBarEffect;
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
            EventManager.TriggerEvent (new SFXPlayEvent (SfxType.SWOOSH, false));
            if (slice != null)
            {
                slice.transform.localPosition = transform.localPosition;
                _Manager.SpawnedItem.Remove(this.gameObject);
            }

            EventManager.TriggerEvent(new DurationCutEvent(durationCut));

            if (this.gameObject.GetComponent<Fruit>().durationCut>0)
            {
;               _DecreaseTimeEffect.SetActive(true);
            }

            EventManager.TriggerEvent(new ScoreSetEvent(scoreGet));
            DisableActivation();
		}
    }

    void DisableActivation() {
        transform.localPosition = new Vector3(1000, 1000, 1000);
        DestroyThis();
    }

    void OnEnable ()
    {
        if (_ThisCoroutine != null)
            StopCoroutine(_ThisCoroutine);
        _ThisCoroutine = StartCoroutine(UndestroyedItem());
    }

    void DestroyThis()
    {
        gameObject.SetActive(false);
        _Manager.SpawnedItem.Remove(this.gameObject);
    }

    IEnumerator UndestroyedItem()
    {
        yield return new WaitForSeconds(1);
        _Manager.Score -= this.gameObject.GetComponent<Fruit>().scoreGet;
        gameObject.SetActive(false);
        _Manager.SpawnedItem.Remove(this.gameObject);
    }

    private void OnDisable()
    {
        if (_ThisCoroutine != null)
            StopCoroutine(_ThisCoroutine);
    }
}

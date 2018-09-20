using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSpawner : MonoBehaviour {

    //public GameObject[] itemPrefabs;
	public Transform[] spawnPoints;

	public float minDelay = 0.1f;
	public float maxDelay = 0.8f;

    Coroutine _LoopSpawner;
    bool _IsStart;
    // Use this for initialization
    public void InitSpawner ()
    {
        if (_LoopSpawner != null)
            StopCoroutine(_LoopSpawner);

        _IsStart = true;
        _LoopSpawner = StartCoroutine(SpawnFruits());
	}
    
	public IEnumerator SpawnFruits ()
	{
		while (_IsStart)
		{
			float delay = Random.Range(minDelay, maxDelay);
			yield return new WaitForSeconds(delay);

			int spawnIndex = Random.Range(0, spawnPoints.Length);
			Transform spawnPoint = spawnPoints[spawnIndex];

            int itemIndex = Random.Range(0, 53);

            GameObject slice = PoolingObject.Instance.GetRandom(itemIndex);
            if (slice != null)
            {
                slice.transform.position = spawnPoint.position;
                slice.transform.rotation = spawnPoint.rotation;
                slice.SetActive(true);
                slice.GetComponent<Fruit>().Init();
            }
        }
	}

    public void Reset()
    {
        _IsStart = false;
    }
}

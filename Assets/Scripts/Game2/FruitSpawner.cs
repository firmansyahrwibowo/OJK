using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSpawner : MonoBehaviour {

    public GameObject[] itemPrefabs;
	public Transform[] spawnPoints;

	public float minDelay = .1f;
	public float maxDelay = 1f;

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

            int itemIndex = Random.Range(0, itemPrefabs.Length);
			GameObject spawnedFruit = Instantiate(itemPrefabs[itemIndex], spawnPoint.position, spawnPoint.rotation);
			Destroy(spawnedFruit, 5f);
		}
	}

    public void Reset()
    {
        _IsStart = false;
    }
}

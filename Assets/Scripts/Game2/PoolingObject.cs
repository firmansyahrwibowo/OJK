using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PoolingData {
    public GameObject Object;
    public string AlternName;
}

public class PoolingObject : MonoBehaviour
{
	public delegate void PoolingSystem ();
	public event PoolingSystem DonePooling;

    public static PoolingObject Instance;
    public List<PoolingData> pooledObject;
    public List<PoolingData> slicedObject;

    public int pooledAmount = 20;
    public bool willGrow = true;
    
    [SerializeField]
    public List<PoolingData> _PooledObjects;
    [SerializeField]
    public List<PoolingData> _SlicedObjects;

    private void Awake()
    {
        Instance = this;
    }

	public void InitPooling()
    {
        _PooledObjects = new List<PoolingData>();
        for (int i = 0; i < pooledObject.Count; i++)
        {
            for (int j = 0; j < pooledAmount; j++)
            {
                GameObject obj = (GameObject)Instantiate(pooledObject[i].Object);
				obj.transform.SetParent(transform);
                obj.SetActive(false);
                obj.name = pooledObject[i].AlternName;

                PoolingData data = new PoolingData();
                data.Object = obj;
                data.AlternName = pooledObject[i].AlternName;
                _PooledObjects.Add(data);
            }
        }

        _SlicedObjects = new List<PoolingData>();
        for (int i = 0; i < slicedObject.Count; i++)
        {
            for (int j = 0; j < pooledAmount; j++)
            {
                GameObject obj = (GameObject)Instantiate(slicedObject[i].Object);
                obj.transform.SetParent(transform);
                obj.SetActive(false);

                PoolingData data = new PoolingData();
                data.Object = obj;
                data.AlternName = slicedObject[i].AlternName;
                _SlicedObjects.Add(data);
            }
        }
        if (DonePooling != null)
			DonePooling();
    }

    public GameObject GetPooledObject(string objName)
    {
        for (int i = 0; i < _PooledObjects.Count; i++)
        {
           if (!_PooledObjects[i].Object.activeInHierarchy)
            {
                if (objName == _PooledObjects[i].AlternName)
                {
                    return _PooledObjects[i].Object;
                }
                  
            }
        }

        return null;
    }

    public GameObject GetRandom (int index)
    {
        for (int i = 0; i < _PooledObjects.Count; i++)
        {
            if (!_PooledObjects[i].Object.activeInHierarchy)
            {
                if (index == i)
                {
                    Debug.Log(i);
                    return _PooledObjects[i].Object;
                }

            }
        }

        return null;
    }
    public GameObject GetSlicedObject(string objName)
    {
        for (int i = 0; i < _SlicedObjects.Count; i++)
        {
            if (!_SlicedObjects[i].Object.activeInHierarchy)
            {
                if (objName == _SlicedObjects[i].AlternName)
                {
                    _SlicedObjects[i].Object.SetActive(true);
                    return _SlicedObjects[i].Object;
                }

            }
        }

        return null;
    }
 //   public void PoolObject (string objName, Vector3 trans){
	//	bool StopFind = false;
	//	for (int i = 0; i < _PooledObjects.Count && !StopFind; i++)
	//	{
	//		if (!_PooledObjects[i].activeInHierarchy)
	//		{
	//			if (objName+"(Clone)" == _PooledObjects[i].name)
	//			{
	//				_PooledObjects[i].transform.position = trans;
	//				_PooledObjects[i].SetActive(true);
	//				StopFind = true;
	//			}

	//		}
	//	}
	//}

	//public void DestroyPool (){
	//	for (int i = 0; i < _PooledObjects.Count; i++)
	//	{
	//		if (_PooledObjects[i].activeInHierarchy)
	//		{
	//			_PooledObjects[i].SetActive(false); 
	//		}
	//	}
	//}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlicedObject : MonoBehaviour {

    private Coroutine _ThisCoroutine;
    private List<Vector3> _ChildrenPos;

    private void Awake()
    {
        _ChildrenPos = new List<Vector3>();
        _ChildrenPos.Add (transform.GetChild(0).localPosition);
        _ChildrenPos.Add(transform.GetChild(1).localPosition);
    }

    void OnEnable()
    {
        if (_ThisCoroutine != null)
            StopCoroutine(_ThisCoroutine);

        _ThisCoroutine = StartCoroutine(DestroyThis());

        transform.GetChild(0).localPosition = _ChildrenPos[0];
        transform.GetChild(1).localPosition = _ChildrenPos[1];
    }

    IEnumerator DestroyThis()
    {
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        if (_ThisCoroutine != null)
            StopCoroutine(_ThisCoroutine);
    }
}

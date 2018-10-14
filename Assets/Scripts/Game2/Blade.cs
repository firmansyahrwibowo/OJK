using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour {

	public float minCuttingVelocity = .001f;

	bool isCutting = false;

	//Vector2 previousPosition;

	GameObject currentBladeTrail;

	Rigidbody2D rb;
	Camera cam;
	CircleCollider2D circleCollider;

    bool _IsStart = false;
    Transform _BladeParent;

	void Awake ()
	{
		cam = Camera.main;
		//rb = GetComponent<Rigidbody2D>();
		//circleCollider = GetComponent<CircleCollider2D>();
	}

    public void Init() {
        _IsStart = true;
	}
	public void Reset() {
		StopCutting ();
	}
	// Update is called once per frame
	void Update () {
        if (_IsStart)
        {
            if (Input.GetMouseButtonDown(0))
            {
                StartCutting();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                StopCutting();
            }

            if (isCutting)
            {
                UpdateCut();
            }
        }
	}

	void UpdateCut ()
	{
		Vector2 newPosition = cam.ScreenToWorldPoint(Input.mousePosition);
        currentBladeTrail.transform.position = newPosition;
	}

	void StartCutting ()
	{
		isCutting = true;
		currentBladeTrail = PoolingObject.Instance.GetPooledObject(ObjectName.BLADE_TRAIL); // Instantiate(bladeTrailPrefab, transform);
        if (currentBladeTrail == null)
            return;
        _BladeParent = currentBladeTrail.transform.parent;
        currentBladeTrail.transform.SetParent (transform);
        currentBladeTrail.transform.localPosition = new Vector3(0, 0, 0);
        currentBladeTrail.SetActive(true);
		//previousPosition = cam.ScreenToWorldPoint(Input.mousePosition);
	}

	void StopCutting ()
	{
		isCutting = false;
        if (currentBladeTrail == null)
            return;

        currentBladeTrail.transform.SetParent(_BladeParent);
        currentBladeTrail.SetActive(false);
	}


    public void StopInit() {
        _IsStart = false;
    }
}

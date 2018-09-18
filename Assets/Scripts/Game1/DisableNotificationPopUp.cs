using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableNotificationPopUp : MonoBehaviour {

    [SerializeField]
    bool _IsDisable = true;

    public void DisableObject() {
        if (_IsDisable)
            gameObject.SetActive(false);
    }
}

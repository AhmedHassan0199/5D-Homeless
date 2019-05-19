using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointGuns : MonoBehaviour {

    Camera PlayerCamera;

	void Start () {
        PlayerCamera = Camera.main;
	}
	
	void Update () {
        gameObject.transform.rotation = PlayerCamera.transform.rotation;
	}
}
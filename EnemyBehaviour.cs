using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {

    GameObject LeftEye;
    GameObject RightEye;
    Vector3 LeftEyeScale;
    Vector3 RightEyeScale;
    bool LeftChanged;
    bool RightChanged;

    void Start () {
        LeftEye = gameObject.transform.Find("Left Eye").gameObject;
        RightEye = gameObject.transform.Find("Right Eye").gameObject;
        LeftEyeScale = LeftEye.transform.localScale;
        RightEyeScale = RightEye.transform.localScale;
        LeftChanged = false;
        RightChanged = false;
    }

    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
            Destroy(other.gameObject);

        Destroy(gameObject);
    }
}
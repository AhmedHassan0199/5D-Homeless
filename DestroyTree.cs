using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTree : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject Wood;
     void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")&& Input.GetKey(KeyCode.Z))
        {
            Instantiate(Wood, this.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}

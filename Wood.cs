using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject Player;
    void Start()
    {
        Vector3 OriginalSize = transform.localScale;
        transform.localScale = Vector3.zero;
        StartCoroutine(ReScale(OriginalSize, 10));
        Player = GameObject.FindGameObjectWithTag("Player");
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")|| collision.gameObject.CompareTag("Enemy"))
        {
            StartCoroutine(ReScale(Vector3.zero, 0));
            Player.GetComponent<MyPlayer>().WoodCount++;
        }
    }


    IEnumerator ReScale(Vector3 Target,float Delay)
    {
       
        while (transform.localScale != Target)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, Target, 5 * Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(Delay);
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] Enemies;
    public GameObject Player;
    public int EnemyCount=0;
    void Start()
    {
        StartCoroutine(Spawn(5));
    }
    IEnumerator Spawn(int Count)
    {

        while (true)
        {
            if(EnemyCount<10)
            {

                int Delay = Random.Range(1, 3);
                yield return new WaitForSeconds(Delay);
                GameObject NewEnemy = Instantiate(Enemies[Delay - 1],  Vector3.one * Random.Range(-5,15), Quaternion.identity);
                NewEnemy.AddComponent<Enemy2D>();
                NewEnemy.GetComponent<SpriteRenderer>().sortingOrder = 1;
                NewEnemy.gameObject.tag = "Enemy";
                NewEnemy.transform.parent = transform;
                EnemyCount++;

            }
            
                yield return new WaitForSecondsRealtime(3);
        }
       
   
    }
   
}

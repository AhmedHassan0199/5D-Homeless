using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2D : MonoBehaviour
{
    public GameObject Player;
    GameObject Source;
    Animator EnemyAnimator;
    float Distance;
    float Speed = 2f;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        EnemyAnimator = GetComponent<Animator>();
        Source = GameObject.FindGameObjectWithTag("Spawner");
        StartCoroutine(Follow());
    }

    IEnumerator Follow()
    {
        int Delay,Idle;
        Vector3 OldPostion;
        while (transform.position != Player.transform.position)
        {
            OldPostion = transform.position;
            Idle = Random.Range(1, 700);
            if (Idle == 5)
            {
                Delay = Random.Range(1, 3);
                yield return new WaitForSeconds(Delay);
            }
            transform.rotation = Quaternion.identity;
            transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, Speed * Time.deltaTime);
            if (OldPostion.magnitude - transform.position.magnitude > 0)
            {
                if (GetComponent<SpriteRenderer>().flipX)
                {
                    GetComponent<SpriteRenderer>().flipX = false;
                }
            }
            else if (OldPostion.magnitude - transform.position.magnitude < 0)
            {
                if (!GetComponent<SpriteRenderer>().flipX)
                {
                    GetComponent<SpriteRenderer>().flipX = true;
                }
            }
            yield return null;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            EnemyAnimator.SetBool("Attack", true);
            if (Player.GetComponent<MyPlayer>().WoodCount > 0)
            {
                Player.GetComponent<MyPlayer>().WoodCount -= 5;
            }

            if (Source.GetComponent<Spawner>().EnemyCount > 0 && Input.GetKeyDown(KeyCode.Z))
            {
                Source.GetComponent<Spawner>().EnemyCount -= 1;
            }
        }
        else
        {
            EnemyAnimator.SetBool("Attack", false);
        }
    }
    void Update()
    {
        Distance = Vector3.Distance(transform.position, Player.transform.position);


        if ( Distance >= 1.5f && Distance <=2.5f && Input.GetKeyDown(KeyCode.Z))
        {
            if (Source.GetComponent<Spawner>().EnemyCount > 0 )
            {
                Source.GetComponent<Spawner>().EnemyCount -= 1;
            }

            Destroy(gameObject);

        }
        
    }

}

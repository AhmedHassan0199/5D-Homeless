using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MyPlayer : MonoBehaviour
{

    public float Speed;
    public GameObject Wood;
    public int WoodCount;
    private Animator PlayerAnimation;
    Rigidbody2D PlayerRB;
    public Text WoodText;
    GameObject Flag;
    public GameObject Canvas;
    // Use this for initialization
    void Start()
    {
        PlayerRB = GetComponent<Rigidbody2D>();
        PlayerAnimation = GetComponent<Animator>();
        Flag = GameObject.FindGameObjectWithTag("Flag");
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Tree") && Input.GetKeyDown(KeyCode.Z))
        {
            PlayerAnimation.SetBool("Attack", true);
            Instantiate(Wood, collision.gameObject.transform.position, Quaternion.identity);
            Instantiate(Wood, collision.gameObject.transform.position + Vector3.one * 2, Quaternion.identity);
            Instantiate(Wood, collision.gameObject.transform.position + Vector3.left * -2, Quaternion.identity);
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Flag"))
        {
            Canvas.SetActive(true);
        }
        else
        {
            PlayerAnimation.SetBool("Attack", false);
        }
    }


    // Update is called once per frame
    void Update()
    {
        WoodText.text = "Wood : " + WoodCount.ToString() + "/50";
        WoodText.fontSize=30;
        transform.Translate(Input.GetAxis("Horizontal") * Speed * Time.deltaTime, Input.GetAxis("Vertical") * Speed * Time.deltaTime, 0);
        transform.rotation = Quaternion.identity;
        if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0 || Mathf.Abs(Input.GetAxis("Vertical")) > 0)
        {
            PlayerAnimation.SetFloat("Speed", 1);
        }
        else
        {
            PlayerAnimation.SetFloat("Speed", 0);
        }
        if (Input.GetKey(KeyCode.Z))
        {
            PlayerAnimation.SetBool("Attack", true);
        }
        else
        {
            PlayerAnimation.SetBool("Attack", false);
        }

        if (Input.GetAxis("Horizontal") < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (Input.GetAxis("Horizontal") > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        if (WoodCount < 0)
        {
            WoodCount = 0;
        }
        if (WoodCount >= 50)
        {
            WoodText.text = "Run to the flag in the ice zone upwards";
            Flag.GetComponent<BoxCollider2D>().enabled = true;
            Flag.GetComponent<SpriteRenderer>().enabled = true;
        }

    }
}

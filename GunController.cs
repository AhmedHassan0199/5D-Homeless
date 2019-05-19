using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {

    public GameObject Muzle;
    public GameObject Bullet;
    public PlayerStats playerStats;

	void Start () {
		
	}
	
	void Update () {

        if (Input.GetMouseButtonUp(0) && playerStats.Ammo > 0) 
        {
            GameObject NewBullet = Instantiate(Bullet, Muzle.transform.position, Quaternion.identity);
            NewBullet.GetComponent<Rigidbody>().AddRelativeForce(Muzle.transform.forward * 50,ForceMode.Impulse);
            playerStats.Ammo--;
        }
	}
}
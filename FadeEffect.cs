using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class FadeEffect : MonoBehaviour {

    Text text;
    

    float Alpha;

	// Use this for initialization
	void Start () {
        text = gameObject.GetComponent<Text>();
        Alpha = text.color.a;
        StartCoroutine(fadingEffect());
	}
	
    IEnumerator fadingEffect()
    {
        while(Alpha < 255)
        {
            text.color = new Color32(0,0,0, (byte)Alpha);       
            Alpha+=10;
            yield return null;
        }

    }

	// Update is called once per frame
	void Update () {
		
	}
}

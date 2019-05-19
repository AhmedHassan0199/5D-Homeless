using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProjectManager : MonoBehaviour {

    public Text text;
    float Alpha1 = 0;

    public string xaxa;

	// Use this for initialization
	void Start () {

       
	}

    
    IEnumerator fadingEffect()
    {
       
        while (Alpha1 < 255)
        {
            text.color = new Color32(255, 0, 0, (byte)Alpha1); 
            Alpha1 +=10;
            yield return null;
        }

    }


    public void Clicked()
    {
        StartCoroutine(fadingEffect());
        xaxa = "OKAY";
    }

    public void NextScene()
    {
        print(xaxa);
        if (xaxa == "OKAY")
        {
            SceneManager.LoadScene("Dialouge1");
        }
    }
}

using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class DogSceneScript : MonoBehaviour
{
    public Text text;
    public Image image;
    float Alpha1 = 0;
    
    public string xaxa;


    // Start is called before the first frame update
    void Start()
    {
        image.enabled = false;
    }

    IEnumerator fadingEffect()
    {

        while (Alpha1 < 255)
        {
            text.color = new Color32(255, 0, 0, (byte)Alpha1);
            Alpha1 += 10;
            yield return null;
        }

    }

    public void ShowText()
    {
        StartCoroutine(fadingEffect());
        xaxa = "Completed";
    }
    public void ShowImage()
    {
        image.enabled = true;
    }

    public void NextScene()
    {
        if (xaxa=="Completed")
        {
            SceneManager.LoadScene("HowToPlay2D");
        }
    }
    public void To2D()
    {

            SceneManager.LoadScene("Main");

    }


}

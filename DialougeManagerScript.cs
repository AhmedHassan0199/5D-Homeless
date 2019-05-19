using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class DialougeManagerScript : MonoBehaviour {

    public Image HomeLessGuy, RudeGuy, DialougeBox, DialougeBox1;

    public GameObject text1,text2;
    public TextMeshProUGUI textDisplay;
    public string sentence;
    public string xaxa;
    //public int Turn = 0;

    // Use this for initialization
    void Start () {
        RudeGuy.enabled = false;
        DialougeBox1.enabled = false;
	}
    IEnumerator TextWrite()
    {
        foreach (char letter in sentence.ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(0.02f);
        }
    }
    public void NextDialouge()
    {
            RudeGuy.enabled = true;
            DialougeBox1.enabled = true;
            HomeLessGuy.enabled = false;
            DialougeBox.enabled = false;
            StartCoroutine(TextWrite());
        xaxa = "OKAY";
    }
    public void NextScene()
    {
        if(xaxa=="OKAY")
        {
            SceneManager.LoadScene("DogScene");
        }
    }

}

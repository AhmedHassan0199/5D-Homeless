using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Dialouge : MonoBehaviour {
    public TextMeshProUGUI textDisplay;
    public string sentence;
    public Image eshta;
        
	// Use this for initialization
	void Start () {
        StartCoroutine(TextWrite());
	}
	IEnumerator TextWrite()
    {
        foreach(char letter in sentence.ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(0.02f);
        }
    }
}

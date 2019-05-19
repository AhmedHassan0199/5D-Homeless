using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class FadeEffectTany : MonoBehaviour {

    public Image text;
    float Alpha1 = 0;

    // Use this for initialization
    void Start()
    {

        text = gameObject.GetComponent<Image>();
    }


    IEnumerator fadingEffect()
    {

        while (Alpha1 < 255)
        {
            text.color = new Color32(194,194,194, (byte)Alpha1);
            Alpha1 += 10;
            yield return null;
        }

    }


    public void Clicked()
    {
        StartCoroutine(fadingEffect());
    }
}

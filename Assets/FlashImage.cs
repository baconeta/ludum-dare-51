using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashImage : MonoBehaviour
{

    public AnimationCurve fadeCurve;
    public Image flashImage;
    public float flashDuration;
    public Color flashColor;

    private void Start()
    {
        flashImage = GetComponent<Image>();
        Flash();
    }

    public void Flash()
    {
        StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        
        float t = 0;
        //Half for flash in, half for flash out.
        float halfFlashDuration = flashDuration / 2;
        //Fade in
        while (t < halfFlashDuration)
        {
            //Lerp 0 - 1 over t
            float lerp = Mathf.Lerp(0, 1, t / halfFlashDuration);
            lerp *= fadeCurve.Evaluate(t / halfFlashDuration);
            //Set opacity to lerp
            flashColor.a = lerp;
            flashImage.color = flashColor;
            //Increment t
            t += Time.deltaTime;
            yield return null;
        }
        
        //Fade out
        while (t < flashDuration)
        {
            //Lerp backwards to 0
            float lerp = Mathf.Lerp(0, 1, t / halfFlashDuration);
            lerp *= fadeCurve.Evaluate(t / halfFlashDuration);
            
            //Set opacity to lerp
            flashColor.a = lerp;
            flashImage.color = flashColor;
            //Increment t
            t -= Time.deltaTime;
            yield return null;

        }

        yield return null;
    }
}

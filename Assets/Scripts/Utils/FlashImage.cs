using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashImage : MonoBehaviour
{

    public AnimationCurve fadeCurve;
    public SpriteRenderer flashSpriteRenderer;
    public float flashDuration;
    public Color flashColor;

    private bool isFlashing;

    private void Start()
    {
        flashSpriteRenderer = GetComponent<SpriteRenderer>();
        
    }

    public void Flash()
    {
        //If not mid flash, then flash.
        if(!isFlashing) StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        isFlashing = true;
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
            flashSpriteRenderer.color = flashColor;
            //Increment t
            t += Time.deltaTime;
            yield return null;
        }
        
        //Fade out
        while (t > 0)
        {
            //Lerp backwards to 0
            float lerp = Mathf.Lerp(0, 1, t / halfFlashDuration);
            lerp *= fadeCurve.Evaluate(t / halfFlashDuration);
            
            //Set opacity to lerp
            flashColor.a = lerp;
            flashSpriteRenderer.color = flashColor;
            //Increment t
            t -= Time.deltaTime;
            yield return null;

        }

        //Finish
        isFlashing = false;
        yield return null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessing : MonoBehaviour
{
    public AnimationCurve fadeCurve;
    public float fadeDuration;
    public Volume ppVolume;

    public Light2D playerLight;

    //Fades the post processing volume from transparent to opaque. Determined by phase.
    public void FadeLighting(EWorldPhase worldPhase)
    {
        if (worldPhase is EWorldPhase.DARK)
        {
            //Make dark
            StartCoroutine(EFadeIn());
        }
        else
        {
            //Make light
            StartCoroutine(EFadeOut());
        }

    }

    private IEnumerator EFadeIn()
    {
        float t = 0;
        while (t < fadeDuration)
        {
            ppVolume.weight = fadeCurve.Evaluate(t / fadeDuration);
            t += Time.deltaTime;
            yield return null;
        }

        //Enable player lantern
        playerLight.enabled = true;
        
        yield return null;
    }

    private IEnumerator EFadeOut()
    {
        float t = fadeDuration;
        while (t > 0)
        {
            ppVolume.weight = fadeCurve.Evaluate(t / fadeDuration);
            t -= Time.deltaTime;
            yield return null;
        }
        
        //Disable player lantern
        playerLight.enabled = false;
        
        yield return null;
    }
}

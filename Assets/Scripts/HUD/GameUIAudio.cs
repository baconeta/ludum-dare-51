using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIAudio : MonoBehaviour
{
    [Header("Audio")] 
    public AudioSource uISoundSource;
    
    public AudioClip buttonClickAudio;
    public AudioClip buttonHoverAudio;
    
    public void PlayHoverSound()
    {
        uISoundSource.PlayOneShot(buttonHoverAudio);
    }

    public void PlayClickSound()
    {
        uISoundSource.PlayOneShot(buttonClickAudio);
    }
}

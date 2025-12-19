using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonfunction : MonoBehaviour
{
    public AudioSource SoundEffect;
    public AudioClip hover;
    public AudioClip click;

    public void hoverSound()
    {
        SoundEffect.PlayOneShot(hover);
    }

    public void clickSound()
    {
        SoundEffect.PlayOneShot(click);
    }







}

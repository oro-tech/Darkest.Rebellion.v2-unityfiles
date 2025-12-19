using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    [Header("------Music Source-----")]
    [SerializeField] AudioSource musicsource;

    [Header("------Audio sound-----")]
    public AudioClip BackGround;

    private void Start()
    {
        musicsource.clip = BackGround;
        musicsource.loop = true;
        musicsource.Play();
    }





}

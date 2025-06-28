using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundIntroSound : MonoBehaviour
{
    public AudioClip introSound;
    public AudioSource AudioSource { get; private set; }

    private void Awake()
    {
        AudioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        SoundManager.instance.PlayClip(introSound, AudioSource);
    }
}

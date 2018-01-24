using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : SingletonComponent<AudioController> {


    public AudioClip stepClip;

    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayStepEffect()
    {
        audioSource.PlayOneShot(stepClip);
    }
}

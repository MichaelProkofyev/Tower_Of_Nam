using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : SingletonComponent<AudioController> {


    public AudioClip wetStepClip;
    public AudioClip[] dryStepClips;

    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void StopPlayingRain()
    {

    }

    public void PlayStepEffect()
    {
        switch (GameController.Instance.State)
        {
            case GameController.GameState.INTRODUCTION:
            case GameController.GameState.OUTSIDE:
                audioSource.PlayOneShot(wetStepClip);
                break;
            case GameController.GameState.TOWER:
                audioSource.PlayOneShot(dryStepClips[Random.Range(0, dryStepClips.Length - 1)]);
                break;
            case GameController.GameState.ASCENSION:
                break;
        }
    }
}

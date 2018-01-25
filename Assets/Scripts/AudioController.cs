using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ThemeType
{
    HELICOPTER,
    RAIN,
    RAIN_MUFFLED,
    AMBIENT_ASCEND
}

public class AudioController : SingletonComponent<AudioController> {


    public AudioClip helicopterSound;
    public AudioClip rainSound;
    public AudioClip rainMuffledSound;
    public AudioClip ascendSound;

    public AudioClip wetStepClip;
    public AudioClip[] dryStepClips;

    public AudioSource audioSource;


    public void PlayTheme(ThemeType themeType)
    {
        audioSource.Stop();
        audioSource.loop = true;
        switch (themeType)
        {
            case ThemeType.HELICOPTER:
                audioSource.clip = helicopterSound;
                break;
            case ThemeType.RAIN:
                audioSource.clip = rainSound;
                break;
            case ThemeType.RAIN_MUFFLED:
                audioSource.clip = rainMuffledSound;
                break;
            case ThemeType.AMBIENT_ASCEND:
                audioSource.clip = ascendSound;
                break;
            default:
                break;
        }
        audioSource.Play();
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

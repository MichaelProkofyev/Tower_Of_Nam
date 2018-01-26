using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ThemeType
{
    HELICOPTER,
    RAIN,
    RAIN_MUFFLED,
    NOISE,
    AMBIENT_ASCEND
}

public class AudioController : SingletonComponent<AudioController> {


    public AudioClip helicopterSound;
    public AudioClip rainSound;
    public AudioClip rainMuffledSound;
    public AudioClip workNoise;
    public AudioClip ascendSound;

    public AudioSource audioSource;

    public DoubleAudioSource doubleAudio;


    public void PlayTheme(ThemeType themeType)
    {
        //audioSource.Stop();
        //audioSource.loop = true;
        switch (themeType)
        {
            case ThemeType.HELICOPTER:
                doubleAudio.CrossFade(helicopterSound, 1, 1);
                break;
            case ThemeType.RAIN:
                doubleAudio.CrossFade(rainSound, 1, 1);
                break;
            case ThemeType.RAIN_MUFFLED:
                doubleAudio.CrossFade(rainMuffledSound, 1, 1);
                break;
            case ThemeType.NOISE:
                doubleAudio.CrossFade(workNoise, 1, 5f);
                break;
            case ThemeType.AMBIENT_ASCEND:
                doubleAudio.CrossFade(ascendSound, .6f, 3, 1f);
                break;
            default:
                break;
        }
        audioSource.Play();
    }

    public void StopAll()
    {
        doubleAudio.StopAll();
    }
}

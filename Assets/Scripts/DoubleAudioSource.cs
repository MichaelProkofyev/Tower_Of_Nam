﻿using UnityEngine;
using System.Collections;


[ExecuteInEditMode]
public class DoubleAudioSource : MonoBehaviour
{
    AudioSource _source0;
    AudioSource _source1;

    #region internal vars
    bool cur_is_source0 = true; //is _source0 currently the active AudioSource (plays some sound right now)

    Coroutine _curSourceFadeRoutine = null;
    Coroutine _newSourceFadeRoutine = null;
    #endregion


    #region internal functionality
    void Reset()
    {
        Update();
    }


    void Awake()
    {
        Update();
    }


    void Update()
    {
        //constantly check if our game object doesn't contain audio sources which we are referencing.

        //if the _source0 or _source1 contain obsolete references (most likely 'null'), then
        //we will re-init them:
        if (_source0 == null || _source1 == null)
        {
            InitAudioSources();
        }

    }

    public void StopAll()
    {
        _source0.volume = 0;
        _source1.volume = 0;
        _source0.Stop();
        _source1.Stop();
        _source0.clip = null;
        _source1.clip = null;
    }


    //re-establishes references to audio sources on this game object:
    void InitAudioSources()
    {
        //re-connect _source0 and _source1 to the ones in attachedSources[]
        AudioSource[] audioSources = gameObject.GetComponents<AudioSource>();

        if (ReferenceEquals(audioSources, null) || audioSources.Length == 0)
        {
            _source0 = gameObject.AddComponent<AudioSource>();
            _source1 = gameObject.AddComponent<AudioSource>();
            //DefaultTheSource(_source0);
            // DefaultTheSource(_source1);  //remove? we do this in editor only
            return;
        }

        switch (audioSources.Length)
        {
            case 1:
                {
                    _source0 = audioSources[0];
                    _source1 = gameObject.AddComponent<AudioSource>();
                    //DefaultTheSource(_source1);  //TODO remove?  we do this in editor only
                }
                break;
            default:
                { //2 and more
                    _source0 = audioSources[0];
                    _source1 = audioSources[1];
                }
                break;
        }//end switch
    }
    #endregion


    //gradually shifts the sound comming from our audio sources to the this clip:
    // maxVolume should be in 0-to-1 range
    public void CrossFade(AudioClip clipToPlay, float maxVolume, float fadingTime, float delay_before_crossFade = 0)
    {
        //var fadeRoutine = StartCoroutine(Fade(clipToPlay, maxVolume, fadingTime, delay_before_crossFade));
        StartCoroutine(Fade(clipToPlay, maxVolume, fadingTime, delay_before_crossFade));

    }//end CrossFade()


    IEnumerator Fade(AudioClip playMe, float maxVolume, float fadingTime, float delay_before_crossFade = 0)
    {
        if (delay_before_crossFade > 0)
        {
            yield return new WaitForSeconds(delay_before_crossFade);
        }

        AudioSource curActiveSource, newActiveSource;
        if (cur_is_source0)
        {
            //_source0 is currently playing the most recent AudioClip
            curActiveSource = _source0;
            //so launch on _source1
            newActiveSource = _source1;
        }
        else
        {
            //otherwise, _source1 is currently active
            curActiveSource = _source1;
            //so play on _source0
            newActiveSource = _source0;
        }

        //perform the switching
        newActiveSource.clip = playMe;
        newActiveSource.Play();
        newActiveSource.volume = 0;

        if (_curSourceFadeRoutine != null)
        {
            StopCoroutine(_curSourceFadeRoutine);
        }

        if (_newSourceFadeRoutine != null)
        {
            StopCoroutine(_newSourceFadeRoutine);
        }

        _curSourceFadeRoutine = StartCoroutine(fadeSource(curActiveSource, curActiveSource.volume, 0, fadingTime));
        _newSourceFadeRoutine = StartCoroutine(fadeSource(newActiveSource, newActiveSource.volume, maxVolume, fadingTime));

        cur_is_source0 = !cur_is_source0;

        yield break;
    }


    IEnumerator fadeSource(AudioSource sourceToFade, float startVolume, float endVolume, float duration)
    {
        float startTime = Time.time;

        while (true)
        {
            float elapsed = Time.time - startTime;

            sourceToFade.volume = Mathf.Clamp01(Mathf.Lerp(startVolume, endVolume, elapsed / duration));

            if (sourceToFade.volume == endVolume)
            {
                break;
            }

            yield return null;
        }//end while
    }


    //returns false if BOTH sources are not playing and there are no sounds are staged to be played.
    //also returns false if one of the sources is not yet initialized
    public bool isPlaying
    {
        get
        {
            if (_source0 == null || _source1 == null)
            {
                return false;
            }

            //otherwise, both sources are initialized. See if any is playing:
            return _source0.isPlaying || _source1.isPlaying;
        }//end get
    }

}
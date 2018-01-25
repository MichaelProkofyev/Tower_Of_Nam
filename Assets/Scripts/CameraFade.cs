using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraFade : MonoBehaviour {


    public Image fadeImage;


    public float fadeDuration = 1f;

    private float fadeAlpha = 1f;
    private int fadeDirection = -1;
    private bool fading = false;

    public void SetFadedOut()
    {
        fadeAlpha = 1f;
        var c = fadeImage.color;
        c.a = fadeAlpha;
        fadeImage.color = c;
    }

    public void FadeIn()
    {
        fadeDirection = -1;
        fadeAlpha = 1f;
        fading = true;
    }

    public void FadeOut()
    {
        fadeDirection = 1;
        fadeAlpha = 0f;
        fading = true;
    }


    void Update()
    {
        if (fading)
        {
            fadeAlpha += fadeDirection * Time.deltaTime / fadeDuration;
            fadeAlpha = Mathf.Clamp01(fadeAlpha);
            var c = fadeImage.color;
            c.a = fadeAlpha;
            fadeImage.color = c;

            if (Mathf.Approximately(fadeAlpha, fadeDirection == -1 ? 0 : 1))
            {
                fading = false;
            }
        }
    }
}

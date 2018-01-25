using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameController : SingletonComponent<GameController> {

    public enum GameState
    {
        INTRODUCTION,
        OUTSIDE,
        TOWER,
        ASCENSION
    }


    public Image arrowImage;
    public Text locationDescriptionText;
    public CameraFade fadeController;

    public GameObject rainParticles;
    public GameObject towerOuterShell;
    public GameObject towerInnerShell;

    public Node firstStepNode1;
    public Node firstStepNode2;
    public Node towerEntranceNode;
    public Node firstTowerNode;
    public Node ascensionNode;

    public GameObject focusedPlayer;
    public GameObject fpsPlayer;

    public GameState State
    { get { return state; }
        set
        {
            state = value;
            switch (state)
            {
                case GameState.INTRODUCTION:
                    AudioController.Instance.PlayTheme(ThemeType.HELICOPTER);
                    fadeController.SetFadedOut();
                    MakeAction(() => StartCoroutine(ShowText("Vietnam, 1975.", 3f)), 1f);
                    MakeAction(() => StartCoroutine(ShowText("Find her in the <color=\"#E2CC52FF\">Tower</color>", 3f)), 5f);
                    MakeAction(() => fadeController.FadeIn(), 7f);
                    MakeAction(() => State = GameState.OUTSIDE, 7f);
                    break;
                case GameState.OUTSIDE:
                    AudioController.Instance.PlayTheme(ThemeType.RAIN);
                    arrowImage.enabled = false;
                    rainParticles.SetActive(true);
                    towerOuterShell.SetActive(true);
                    break;
                case GameState.TOWER:
                    AudioController.Instance.PlayTheme(ThemeType.RAIN_MUFFLED);
                    rainParticles.SetActive(false);
                    towerOuterShell.SetActive(false);
                    break;
                case GameState.ASCENSION:
                    focusedPlayer.SetActive(false);
                    fpsPlayer.SetActive(true);
                    RenderSettings.fogDensity = 0.0005f;
                    break;
                default:
                    break;
            }
        }
    }

    private GameState state;

    void Start()
    {
        firstStepNode1.action = () => { State = GameState.OUTSIDE; };
        firstStepNode2.action = () => { State = GameState.OUTSIDE; };
        towerEntranceNode.halfWayAction = () => { State = GameState.OUTSIDE; };
        firstTowerNode.halfWayAction = () => { State = GameState.TOWER; };
        ascensionNode.action = () => { State = GameState.ASCENSION; };
        State = GameState.OUTSIDE;
    }

    private void Update()
    {
        switch (State)
        {
            case GameState.INTRODUCTION:
                break;
            case GameState.OUTSIDE:
                break;
            case GameState.TOWER:
                break;
            case GameState.ASCENSION:
                break;
            default:
                break;
        }
    }

    void MakeAction(System.Action action, float delay)
    {
        StartCoroutine(Delay(action, delay));
    }

    IEnumerator Delay(System.Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action();
    }

    IEnumerator ShowText(string text, float duration)
    {
           locationDescriptionText.text = text;
            yield return new WaitForSeconds(duration);
        locationDescriptionText.text = string.Empty;
    }
}

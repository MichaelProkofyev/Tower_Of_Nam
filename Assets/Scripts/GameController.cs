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
        ROOM,
        ASCENSION
    }


    public Image arrowImage;
    public Text locationDescriptionText;
    public GameObject girlObject;
    public GameObject credits;
    public CameraFade fadeController;

    public GameObject rainParticles;
    public GameObject towerOuterShell;
    public GameObject towerInnerShell;

    public Node firstStepNode1;
    public Node firstStepNode2;
    public Node towerEntranceNode;
    public Node firstTowerNode;
    public Node firstRoomNode;
    public Node lastNode;

    public GameObject focusedPlayer;
    public GameObject fpsPlayer;

    public GameState State
    { get { return state; }
        set
        {
            state = value;
            rainParticles.SetActive(false);
            credits.SetActive(false);
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
                    towerInnerShell.SetActive(false);
                    towerOuterShell.SetActive(true);
                    break;
                case GameState.TOWER:
                    AudioController.Instance.PlayTheme(ThemeType.RAIN_MUFFLED);
                    towerInnerShell.SetActive(true);
                    towerOuterShell.SetActive(false);
                    break;
                case GameState.ROOM:
                    AudioController.Instance.PlayTheme(ThemeType.NOISE);
                    break;
                case GameState.ASCENSION:
                    girlObject.SetActive(false);
                    AudioController.Instance.audioSource.Stop();
                    fadeController.SetFadedOut();
                    focusedPlayer.SetActive(false);
                    fpsPlayer.SetActive(true);
                    MakeAction(() =>
                    {
                        fadeController.FadeIn();
                        AudioController.Instance.PlayTheme(ThemeType.AMBIENT_ASCEND);
                    }
                    , 2f);
                    RenderSettings.fogDensity = 0.001f;
                    MakeAction(() =>
                    {
                        credits.SetActive(true);
                    }
                    , 10f);
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
        towerEntranceNode.halfWayAction = () => {
            State = GameState.OUTSIDE;
            towerInnerShell.SetActive(true);
            towerOuterShell.SetActive(true);
        };
        firstTowerNode.halfWayAction = () => { State = GameState.TOWER; };
        firstRoomNode.halfWayAction = () => { State = GameState.ROOM; };
        lastNode.action = () => { State = GameState.ASCENSION; };
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

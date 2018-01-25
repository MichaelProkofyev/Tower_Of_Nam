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
    public Text storyText;
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
                    fadeController.FadeIn();
                    StartCoroutine(ShowText("Vietnam, 1975.", 1f, false));
                    StartCoroutine(ShowText("Rescue her.", 3f, true));
                    break;
                case GameState.OUTSIDE:
                    arrowImage.enabled = false;
                    rainParticles.SetActive(true);
                    towerOuterShell.SetActive(true);
                    //towerInnerShell.SetActive(false);
                    break;
                case GameState.TOWER:
                    rainParticles.SetActive(false);
                    towerOuterShell.SetActive(false);
                    //towerInnerShell.SetActive(true);
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
        towerEntranceNode.action = () => { State = GameState.OUTSIDE; };
        firstTowerNode.action = () => { State = GameState.TOWER; };
        ascensionNode.action = () => { State = GameState.ASCENSION; };
        State = GameState.INTRODUCTION;
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

    IEnumerator CallAction(System.Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action();
    }

    IEnumerator ShowText(string text, float duration, bool isStoryText)
    {
            var textField = isStoryText ? storyText : locationDescriptionText;
            textField.text = text;
            yield return new WaitForSeconds(duration);
            textField.text = string.Empty;
    }
}

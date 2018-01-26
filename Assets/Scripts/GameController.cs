using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameController : SingletonComponent<GameController> {

    public enum GameState
    {
        INTRO,
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

    public Node firstStepNode;
    public Node towerEntranceNode;
    public Node firstTowerNode;
    public Node firstRoomNode;
    public Node halfwayRoomNode;
    public Node lastStepsRoomNode;
    public Node lastNode;
    public float rotationSpeed;

    public GameObject focusedPlayer;
    public GameObject fpsPlayer;

    public bool canMove = false;

    public GameState State
    { get { return state; }
        set
        {
            state = value;
            rainParticles.SetActive(false);
            credits.SetActive(false);
            canMove = true;
            arrowImage.enabled = false;
            RenderSettings.fogDensity = 0.002f;
            switch (state)
            {
                case GameState.INTRO:
                    AudioController.Instance.PlayTheme(ThemeType.HELICOPTER);
                    fadeController.SetFadedOut();
                    canMove = false;
                    MakeAction(() => StartCoroutine(ShowText("Vietnam, 1975.", 3f)), 1f);
                    MakeAction(() => StartCoroutine(ShowText("Find her in <color=\"#E2CC52FF\">the tower</color>", 3f)), 5f);
                    MakeAction(() => { arrowImage.enabled = true; canMove = true; }, 8f);
                    
                    break;
                case GameState.OUTSIDE:
                    fadeController.FadeIn();
                    AudioController.Instance.PlayTheme(ThemeType.RAIN);
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
                    AudioController.Instance.StopAll();
                    fadeController.SetFadedOut();
                    MakeAction(() => StartCoroutine(ShowText("Can't save them all", 4f)), 1f);
                    MakeAction(() => StartCoroutine(ShowText("There are other worlds than these", 8f)), 6f);
                    focusedPlayer.SetActive(false);
                    fpsPlayer.SetActive(true);
                    MakeAction(() =>
                    {
                        fadeController.FadeIn();
                        AudioController.Instance.PlayTheme(ThemeType.AMBIENT_ASCEND);
                    }
                    , 2f);
                    MakeAction(() =>
                    {
                        credits.SetActive(true);
                    }
                    , 14f);
                    break;
                default:
                    break;
            }
        }
    }

    public GameState state;

    void Start()
    {
        firstStepNode.halfWayAction = () => { if(State != GameState.OUTSIDE) State = GameState.OUTSIDE; };
        towerEntranceNode.halfWayAction = () => {
            State = GameState.OUTSIDE;
            towerInnerShell.SetActive(true);
            towerOuterShell.SetActive(true);
        };
        firstTowerNode.halfWayAction = () => { State = GameState.TOWER; };
        firstRoomNode.halfWayAction = () => { State = GameState.ROOM; };
        halfwayRoomNode.halfWayAction = () => { PlayerControls.Instance.SetDistanceGirlNoiseMult(4); };
        lastStepsRoomNode.halfWayAction = () => { PlayerControls.Instance.SetDistanceGirlNoiseMult(11); };
        lastNode.action = () => { State = GameState.ASCENSION; };
        State = GameState.INTRO;
    }

    private void Update()
    {
        switch (State)
        {
            case GameState.INTRO:
                break;
            case GameState.OUTSIDE:
                break;
            case GameState.TOWER:
                break;
            case GameState.ASCENSION:

                fpsPlayer.transform.Rotate(Random.insideUnitSphere * rotationSpeed * Time.deltaTime);
                fpsPlayer.transform.eulerAngles = new Vector3(Mathf.Clamp(fpsPlayer.transform.eulerAngles.x, -20f, 20f) , Mathf.Clamp(fpsPlayer.transform.eulerAngles.y, -20f, 20f), Mathf.Clamp(fpsPlayer.transform.eulerAngles.z, -20f, 20f));

                if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Escape))
                {
                    Application.Quit();
                }
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

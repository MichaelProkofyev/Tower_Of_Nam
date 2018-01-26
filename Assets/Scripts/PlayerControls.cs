using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : SingletonComponent<PlayerControls> {

    public Transform houseOrigin;

    bool moving = false;
    public Node currentNode;
    public Node targetNode;

    public Camera cam;
    public Material girlMat;


    float transitionProgress = 0f;
    float trueProgress;
    private float shakeDuration = .1f;
    private CameraShake camShakeController;
    private CameraFade fadeController;

    public AudioClip wetStepClip;
    public AudioClip[] dryStepClips;
    public AudioSource audioSource;

    // Use this for initialization
    void Start () {
        if (currentNode != null)
        {
            transform.position = currentNode.transform.position;
        }
        camShakeController = GetComponent<CameraShake>();
        fadeController = GetComponent<CameraFade>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        //Material properties update

        girlMat.SetVector("_PullPoint", transform.position);

        //if (Input.GetKeyDown(KeyCode.I))
        //{
        //    fadeController.FadeIn();

        //}
        //if (Input.GetKeyDown(KeyCode.O))
        //{
        //    fadeController.FadeOut();
        //}



        if (!moving)
        {
            if(!TryMovement(KeyCode.UpArrow, x=>x.forward))
            {
                if (!TryMovement(KeyCode.DownArrow, x => x.back))
                {
                    if (!TryMovement(KeyCode.LeftArrow, x => x.left))
                    {
                        if (!TryMovement(KeyCode.RightArrow, x => x.right))
                        {

                        }
                    }
                }
            }
        }
        else
        {
            bool arrivedAtTarget = Vector3.Distance(transform.position, targetNode.transform.position) < 0.1f;
            if (arrivedAtTarget)
            {
                moving = false;
                transitionProgress = 0f;
                trueProgress = 0;
                transform.position = targetNode.transform.position;
                currentNode = targetNode;
                RenderSettings.fogDensity = 0.002f;
                if (currentNode.action != null)
                {
                    currentNode.action();
                }
            }
            else
            {
                float wholeTravelDistance = Vector3.Distance(currentNode.transform.position, targetNode.transform.position);
                transitionProgress += Time.deltaTime * 5f / Mathf.Clamp(wholeTravelDistance, 1, 10);
                transform.position = Vector3.Slerp(transform.position, targetNode.transform.position, transitionProgress);

                //Fog update on transition
                float oldTrueProgress = trueProgress;
                trueProgress = (wholeTravelDistance - Vector3.Distance(transform.position, targetNode.transform.position)) / wholeTravelDistance;
                if (oldTrueProgress <= .5f && 0.5f < trueProgress)
                {
                    if (targetNode.halfWayAction != null)
                    {
                        targetNode.halfWayAction();
                    }
                }
                RenderSettings.fogDensity = 0.002f + .02f * Mathf.Sin(trueProgress * Mathf.PI);
            }
        }


            cam.transform.LookAt(houseOrigin);
    }

    bool TryMovement(KeyCode key, System.Func<Node,Node> resolver)
    {
        if (Input.GetKeyDown(key) && GameController.Instance.canMove)
        {
            Node possibleTarget = resolver(currentNode);
            if (possibleTarget != null)
            {
                targetNode = possibleTarget;
                moving = true;
                PlayStepEffect();
                return true;
            }
            else
            {
                camShakeController.shakeDuration = shakeDuration;
            }
        }
        return false;
    }

    public void SetDistanceGirlNoiseMult(float f)
    {
        girlMat.SetFloat("_DistanceMultiplier", f);

    }

    public void PlayStepEffect()
    {
        switch (GameController.Instance.State)
        {
            case GameController.GameState.INTRO:
            case GameController.GameState.OUTSIDE:
                audioSource.PlayOneShot(wetStepClip);
                break;
            case GameController.GameState.TOWER:
                audioSource.volume = .55f;
                audioSource.PlayOneShot(dryStepClips[Random.Range(0, dryStepClips.Length - 1)]);
                break;
            case GameController.GameState.ASCENSION:
                break;
        }
    }
}

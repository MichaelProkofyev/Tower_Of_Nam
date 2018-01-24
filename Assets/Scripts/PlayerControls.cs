using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour {

    public Transform houseOrigin;

    bool moving = false;
    public Node currentNode;
    public Node targetNode;

    public Camera cam;
    public Material girlMat;


    float transitionProgress = 0f;
    private float shakeDuration = .1f;
    private CameraShake camShakeController;
    private CameraFade fadeController;

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

        if (Input.GetKeyDown(KeyCode.I))
        {
            fadeController.FadeIn();

        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            fadeController.FadeOut();
        }



        if (!moving)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (currentNode.forward != null)
                {
                    targetNode = currentNode.forward;
                    moving = true;
                    AudioController.Instance.PlayStepEffect();
                }
                else camShakeController.shakeDuration = shakeDuration;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (currentNode.back != null)
                {
                    targetNode = currentNode.back;
                    moving = true;
                    AudioController.Instance.PlayStepEffect();
                }
                else camShakeController.shakeDuration = shakeDuration;
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (currentNode.left != null)
                {
                    targetNode = currentNode.left;
                    moving = true;
                    AudioController.Instance.PlayStepEffect();
                }
                else camShakeController.shakeDuration = shakeDuration;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (currentNode.right != null)
                {
                    targetNode = currentNode.right;
                    moving = true;
                    AudioController.Instance.PlayStepEffect();
                }
                else camShakeController.shakeDuration = shakeDuration;
            }
        }
        else
        {
            bool arrivedAtTarget = Vector3.Distance(transform.position, targetNode.transform.position) < 0.1f;
            if (arrivedAtTarget)
            {
                moving = false;
                transitionProgress = 0f;
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
                float trueProgress = (wholeTravelDistance - Vector3.Distance(transform.position, targetNode.transform.position)) / wholeTravelDistance;
                RenderSettings.fogDensity = 0.002f + .02f * Mathf.Sin(trueProgress * Mathf.PI);
            }
        }


            cam.transform.LookAt(houseOrigin);
    }
}

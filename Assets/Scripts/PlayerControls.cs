using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour {

    public Transform houseOrigin;

    bool moving = false;
    public Node currentNode;
    public Node targetNode;

    float transitionProgress = 0f;

	// Use this for initialization
	void Start () {
        if (currentNode != null)
        {
            transform.position = currentNode.transform.position;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!moving)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (currentNode.forward != null)
                {
                    targetNode = currentNode.forward;
                    moving = true;
                }
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (currentNode.back != null)
                {
                    targetNode = currentNode.back;
                    moving = true;
                }
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (currentNode.left != null)
                {
                    targetNode = currentNode.left;
                    moving = true;
                }
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (currentNode.right != null)
                {
                    targetNode = currentNode.right;
                    moving = true;
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
                transform.position = targetNode.transform.position;
                currentNode = targetNode;
            }
            else
            {
                float wholeTravelDistance = Vector3.Distance(currentNode.transform.position, targetNode.transform.position);
                transitionProgress += Time.deltaTime * 5f / wholeTravelDistance;
                transform.position = Vector3.Slerp(transform.position, targetNode.transform.position, transitionProgress);
            }
        }


        Vector3 lookAt = new Vector3(houseOrigin.position.x, transform.position.y, houseOrigin.position.z);
        transform.LookAt(lookAt);
    }
}

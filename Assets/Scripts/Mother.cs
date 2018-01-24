using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Mother : MonoBehaviour {

    public float patrolRange = 10.0f;
    NavMeshAgent navAgent;
    public State state;
    public Transform playerT;
    public Transform[] partrolPoints;

    private float patrolSpeed = 4f;
    private float huntSpeed = 20f;
    private float huntMemoryLeft = 0f;

    public enum State
    {
        PATROL,
        HUNT,
    }

    void Start () {
        navAgent = GetComponent<NavMeshAgent>();
    }

    void Update () {

        switch (state)
        {
            case State.PATROL:
                navAgent.speed = patrolSpeed;
                if (IsSeeingIntruder())
                {
                    state = State.HUNT;
                    navAgent.destination = playerT.position;
                }
                else
                {
                    if (navAgent.pathPending || navAgent.remainingDistance > 0.1f)
                    {
                        return;
                    }
                    else
                    {
                        navAgent.destination = partrolPoints[Random.Range(0, partrolPoints.Length - 1)].position; // patrolRange * Random.insideUnitCircle;
                    }
                }
                break;
            case State.HUNT:
                navAgent.speed = huntSpeed;
                bool seeingIntruder = IsSeeingIntruder();
                if (seeingIntruder || 0 < huntMemoryLeft)
                {
                    navAgent.destination = playerT.position;
                    huntMemoryLeft = seeingIntruder ? 1f : huntMemoryLeft - Time.deltaTime;
                }
                else
                {
                    //Moving to last seen position
                    if (navAgent.pathPending || navAgent.remainingDistance > 0.1f)
                    {
                        return;
                    }
                    else
                    {
                        state = State.PATROL;
                    }
                }
                break;
        }
    }

    bool IsSeeingIntruder()
    {
        RaycastHit hit;
        var rayDirection = Vector3.Normalize(playerT.position - transform.position);
        rayDirection *= 10f;
        if (Physics.Raycast(transform.position, rayDirection, out hit))
        {
            if (hit.transform == playerT)
            {
                Vector3 targetDir = playerT.position - transform.position;
                float angle = Vector3.Angle(targetDir, transform.forward);
                if (angle < 90f)
                {
                    return true;
                }
            }
        }
        return false;
    }


    
}

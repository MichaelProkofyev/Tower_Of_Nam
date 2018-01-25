using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

    public Node forward;
    public Node back;
    public Node left;
    public Node right;

    public bool lookAtY = true;

    public UnityEngine.Events.UnityAction action;
    public UnityEngine.Events.UnityAction halfWayAction;
}

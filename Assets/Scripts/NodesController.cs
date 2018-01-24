using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodesController : MonoBehaviour {

    Color oneWayColor = Color.blue; //new Color(1f, 95f/255f, 95f/255f);

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnDrawGizmos()
    {
        var nodes = FindObjectsOfType<Node>();
        foreach (var node in nodes)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawCube(node.transform.position, new Vector3(1f, 1f, 1f));
            if (node.forward != null)
            {
                Gizmos.color = node.forward.back != null ? Color.red : oneWayColor;
                Gizmos.DrawLine(node.transform.position, node.forward.transform.position);
            }
            if (node.back != null)
            {
                Gizmos.color = node.back.forward != null ? Color.red : oneWayColor;
                Gizmos.DrawLine(node.transform.position, node.back.transform.position);
            }
            if (node.left != null)
            {
                Gizmos.color = node.left.right != null ? Color.red : oneWayColor;
                Gizmos.DrawLine(node.transform.position, node.left.transform.position);
            }
            if (node.right != null)
            {
                Gizmos.color = node.right.left != null ? Color.red : oneWayColor;
                Gizmos.DrawLine(node.transform.position, node.right.transform.position);
            }
        }
    }
}

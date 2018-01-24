using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreelookPlayer : MonoBehaviour {

    public float ascensionSpeed = 10f;
    bool ascending = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (ascending)
        {
            transform.Translate(Vector3.up * ascensionSpeed);
        }
	}

    private void OnEnable()
    {
        ascending = true;
    }
}

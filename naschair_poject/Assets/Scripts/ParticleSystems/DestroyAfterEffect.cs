using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterEffect : MonoBehaviour {

    private ParticleSystem partSys;

	// Use this for initialization
	void Start () {
        partSys = GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
		
        if (partSys.isStopped)
        {
            Destroy(gameObject);
        }

	}
}

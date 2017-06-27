using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeChair : MonoBehaviour {
	AudioSource _as;
	// Use this for initialization
	void Start () {
		_as = GetComponent<AudioSource> ();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter2D(Collision2D coll) {
		print ("##" + coll.gameObject.name);
		if (coll.gameObject.name == "R-Leg" && !_as.isPlaying) {
			_as.Play ();
		}
			

	}
}

﻿using UnityEngine;
using System.Collections;

public class InteriorLightTrigger : MonoBehaviour {
	Animator m_anim;
	AudioSource _audioSource;
	// Use this for initialization
	void Start () {
		if (GetComponent<Animator> ()) {
			m_anim = GetComponent<Animator> ();
		}

		if (GetComponent<AudioSource> ()) {
			_audioSource = gameObject.GetComponent<AudioSource> ();
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void OnTriggerEnter2D(Collider2D other) {
		if(other.name == "GuardStructure"){
			m_anim.SetBool ("IsOn", true);
			if (_audioSource != null) {
				_audioSource.Play ();
			}

		}


	}

	void OnTriggerExit2D(Collider2D other) {
		if(other.name == "GuardStructure"){
			m_anim.SetBool ("IsOn", false);
			if (_audioSource != null) {
				_audioSource.Play ();
			}
		}


	}

}

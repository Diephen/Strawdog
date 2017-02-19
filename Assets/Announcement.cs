﻿using UnityEngine;
using System.Collections;

public class Announcement : MonoBehaviour {
	AudioSource _audioSource;
	BoxCollider2D _box2d;
	[SerializeField] bool _interrogation = false;
	Timer _waitTimer = new Timer(2.0f);
	// Use this for initialization
	void Start () {
		_audioSource = gameObject.GetComponent<AudioSource> ();
		_box2d = gameObject.GetComponent<BoxCollider2D> ();
		_waitTimer.Reset ();
	}

	void FixedUpdate(){
		if (_interrogation) {
			if (_waitTimer.IsOffCooldown) {
				_audioSource.Play ();
				_interrogation = false;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Guard" || other.tag == "Prisoner") {
				_audioSource.Play ();
				_box2d.enabled = false;
			}
	}

	void CloseOffice(OpenOfficeEvent e){
		if(!e.Opened){
			_box2d.enabled = false;
		} else {
			_box2d.enabled = true;
		}
	}

	void OnEnable(){
		Events.G.AddListener<OpenOfficeEvent>(CloseOffice);
	}

	void OnDisable(){
		Events.G.RemoveListener<OpenOfficeEvent>(CloseOffice);
	}
}

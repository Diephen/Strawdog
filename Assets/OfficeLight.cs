using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeLight : MonoBehaviour {
	[SerializeField] GameObject _light;
	AudioSource _AS;
	// Use this for initialization
	void Start () {
		
		_AS = GetComponent<AudioSource> ();
		_light.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void TurnOnLight(){
		_light.SetActive (true);
		_AS.Play ();
	}

	void TurnOffLight(){
		_light.SetActive (false);
		_AS.Play ();
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.name == "GuardStructure") {
			TurnOnLight ();
		}

	}

	void OnTriggerExit2D(Collider2D other){
		if (other.name == "GuardStructure") {
			TurnOffLight ();
		}

	}
}

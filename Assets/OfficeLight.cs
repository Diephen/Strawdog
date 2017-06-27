using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeLight : MonoBehaviour {
	[SerializeField] GameObject _light;
	// Use this for initialization
	void Start () {
		TurnOffLight ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void TurnOnLight(){
		_light.SetActive (true);
	}

	void TurnOffLight(){
		_light.SetActive (false);
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

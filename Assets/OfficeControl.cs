using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeControl : MonoBehaviour {
	BoxCollider2D[] _boxColls; // all the triggers and colliders in the office area 

	void OnEnable(){
		Events.G.AddListener<OfficeDoorEvent>(CloseOffice);
	}

	void OnDisable(){
		Events.G.RemoveListener<OfficeDoorEvent>(CloseOffice);

	}

	// Use this for initialization
	void Start () {
		_boxColls = GetComponentsInChildren<BoxCollider2D> ();
		foreach (BoxCollider2D b2d in _boxColls) {
			b2d.enabled = false;
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void CloseOffice(OfficeDoorEvent e){
		if (e.Opened) {
			foreach (BoxCollider2D b2d in _boxColls) {
				b2d.enabled = true;
			}
		} else {
			foreach (BoxCollider2D b2d in _boxColls) {
				b2d.enabled = false;
			}
		}
		
	}
}

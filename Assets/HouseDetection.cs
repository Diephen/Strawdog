using UnityEngine;
using System.Collections;

public class HouseDetection : MonoBehaviour {

	HousePatrol _housePatrolScript;
	bool _callOnce = false;

	// Use this for initialization
	void Start () {
		_housePatrolScript = transform.parent.parent.gameObject.GetComponent<HousePatrol> ();
	}

	void OnTriggerStay2D(Collider2D other) {
		if (other.tag == "Prisoner") {
			_housePatrolScript.Stop ();
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.tag == "Prisoner") {
			_housePatrolScript.CarryOn ();
		}
	}

	void IsHidden(PrisonerHideEvent e){
		if (e.Hidden) {
			if (!_callOnce) {
				_housePatrolScript.CarryOn ();
				_callOnce = true;
			}
		}
		else {
			_callOnce = e.Hidden;
		}
	}

	void OnEnable(){
		Events.G.AddListener<PrisonerHideEvent>(IsHidden);
	}
	void OnDisable(){
		Events.G.RemoveListener<PrisonerHideEvent>(IsHidden);
	}
}

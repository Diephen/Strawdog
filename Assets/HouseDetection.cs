using UnityEngine;
using System.Collections;

public class HouseDetection : MonoBehaviour {

	HousePatrol _housePatrolScript;
	bool _callOnce = false;
	bool _triggerOnce = false;

	void Start () {
		_housePatrolScript = transform.parent.parent.gameObject.GetComponent<HousePatrol> ();
	}

	void OnTriggerStay2D(Collider2D other) {
		if (other.tag == "Prisoner") {
			if (_triggerOnce == false) {
				_housePatrolScript.Stop ();
				_triggerOnce = true;
			}
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.tag == "Prisoner") {
			_housePatrolScript.CarryOn ();
			_triggerOnce = false;
		}
	}

	void IsHidden(PrisonerHideEvent e){
		if (_callOnce != e.Hidden) {
			_callOnce = e.Hidden;
			if (e.Hidden) {
				_housePatrolScript.CarryOn ();
			}
			else {
				_triggerOnce = false;
			}
		}
	}

	void OnEnable(){
		Events.G.AddListener<PrisonerHideEvent>(IsHidden);
	}
	void OnDisable(){
		Events.G.RemoveListener<PrisonerHideEvent>(IsHidden);
	}
}

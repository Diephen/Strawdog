using UnityEngine;
using System.Collections;

public class Detection : MonoBehaviour {

	patrol _patrolScript;

	// Use this for initialization
	void Start () {
		_patrolScript = transform.parent.parent.gameObject.GetComponent<patrol> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay2D(Collider2D other) {
		if (other.name == "FemaleStructure") {
			_patrolScript.StopAndLook ();
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.name == "FemaleStructure") {
			_patrolScript.CarryOn ();
		}
	}
}

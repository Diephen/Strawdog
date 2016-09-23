using UnityEngine;
using System.Collections;

public class PickableItem : MonoBehaviour {
	bool _pickedUp = false;
	Quaternion _myRot;
	Transform _originalParent;
	// Use this for initialization
	void Start () {
		_originalParent = transform.parent;
	}
	
	// Update is called once per frame
	void Update () {
		if (_pickedUp) {
			//Keep Item Upright
			_myRot = gameObject.transform.localRotation;
			_myRot.z = (gameObject.transform.parent.localRotation.z) * -1f;
			gameObject.transform.localRotation = _myRot;
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (_pickedUp == false && other.tag == "PickingUp") {
			gameObject.transform.SetParent (other.transform);
		} else if (_pickedUp == true && other.tag == "Ground") {
			gameObject.transform.SetParent (_originalParent);
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.tag == "PickingUp") {
			_pickedUp = false;
		} else if (other.tag == "Ground") {
			_pickedUp = true;
		}
	}
}

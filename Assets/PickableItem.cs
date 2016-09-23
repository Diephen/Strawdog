using UnityEngine;
using System.Collections;

public class PickableItem : MonoBehaviour {
	bool _pickedUp = false;
	Quaternion _myRot;
	// Use this for initialization
	void Start () {
	
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
		if (other.tag == "PickingUp") {
			_pickedUp = true;
			gameObject.transform.SetParent (other.transform);
		}
	}
}

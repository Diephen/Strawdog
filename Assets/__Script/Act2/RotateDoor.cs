using UnityEngine;
using System.Collections;

public class RotateDoor : MonoBehaviour {
	bool _openDoor = false;
	Timer _openDoorTimer;
	Vector3 _tempEuler;
	Quaternion _originalRotation;

	void Start() {
		_openDoorTimer = new Timer (2f);
		_originalRotation = transform.rotation;
	}
	
	void FixedUpdate () {
		if (_openDoor) {
			transform.rotation = Quaternion.Lerp (_originalRotation, Quaternion.Euler (_tempEuler), _openDoorTimer.PercentTimePassed);
		} else {
			transform.rotation = Quaternion.Lerp (transform.rotation, _originalRotation, _openDoorTimer.PercentTimePassed);
		}
	}

	void CloseDoor(LockCellEvent e){
		_openDoor = !e.Locked;
		_tempEuler = _originalRotation.eulerAngles;
		_tempEuler.y = 60.0f;
		_openDoorTimer.Reset ();
	}

	void OnEnable(){
		Events.G.AddListener<LockCellEvent>(CloseDoor);

	}

	void OnDisable(){
		Events.G.RemoveListener<LockCellEvent>(CloseDoor);
	}
}

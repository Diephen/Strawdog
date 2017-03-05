using UnityEngine;
using System.Collections;

public class RotateDoor : MonoBehaviour {
	bool _openDoor = false;
	Timer _openDoorTimer;
	Vector3 _tempEuler;
	Quaternion _originalRotation;
	SpriteRenderer _spriteRenderer;
	Color _tempSpriteColor;

	void Start() {
		_openDoorTimer = new Timer (2f);
		_originalRotation = transform.rotation;
		_spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		_tempSpriteColor = _spriteRenderer.color;

	}
	
	void FixedUpdate () {
		if (_openDoor) {
			transform.rotation = Quaternion.Lerp (_originalRotation, Quaternion.Euler (_tempEuler), _openDoorTimer.PercentTimePassed);
			_tempSpriteColor.a = _openDoorTimer.PercentTimeLeft;
		} else {
			transform.rotation = Quaternion.Lerp (transform.rotation, _originalRotation, _openDoorTimer.PercentTimePassed);
			_tempSpriteColor.a = _openDoorTimer.PercentTimePassed;
		}
		_spriteRenderer.color =  _tempSpriteColor;
	}

	void CloseDoor(LockCellEvent e){
		_openDoor = !e.Locked;
		_tempEuler = _originalRotation.eulerAngles;
		_tempEuler.y = 60.0f;
		_openDoorTimer.Reset ();
	}

	void CloseOffice(OpenOfficeEvent e){
		_openDoor = !e.Opened;
		_tempEuler = _originalRotation.eulerAngles;
		_tempEuler.y = 60.0f;
		_openDoorTimer.Reset ();
	}

	void OnEnable(){
		Events.G.AddListener<LockCellEvent>(CloseDoor);
		Events.G.AddListener<OpenOfficeEvent>(CloseOffice);
	}

	void OnDisable(){
		Events.G.RemoveListener<LockCellEvent>(CloseDoor);
		Events.G.RemoveListener<OpenOfficeEvent>(CloseOffice);
	}
}

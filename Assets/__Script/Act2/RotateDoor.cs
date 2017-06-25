using UnityEngine;
using System.Collections;

public class RotateDoor : MonoBehaviour {
	[SerializeField] bool _isOfficeDoor = false;
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
		_tempEuler = _originalRotation.eulerAngles;
		_tempEuler.y = 60.0f;
		if (_isOfficeDoor) {
			_tempSpriteColor = _spriteRenderer.color;
		}

	}
	
	void FixedUpdate () {
		if (!_openDoor) {
			transform.rotation = Quaternion.Lerp (_originalRotation, Quaternion.Euler (_tempEuler), _openDoorTimer.PercentTimePassed);
			//_tempSpriteColor.a = _openDoorTimer.PercentTimeLeft;
		} else {
			transform.rotation = Quaternion.Lerp (transform.rotation, _originalRotation, _openDoorTimer.PercentTimePassed);
			//_tempSpriteColor.a = _openDoorTimer.PercentTimePassed;
		}
		if (_isOfficeDoor) {
			_spriteRenderer.color = _tempSpriteColor;
		}
	}

	void CloseDoor(LockCellEvent e){
		_openDoor = !e.Locked;
		//_tempEuler = _originalRotation.eulerAngles;
		//_tempEuler.y = 60.0f;
		_openDoorTimer.Reset ();
		Debug.Log ("Close DOor is called: " + e.Locked);
	}

	void CloseOffice(OfficeDoorEvent e){
		_openDoor = e.Opened;
		//_tempEuler = _originalRotation.eulerAngles;
		//_tempEuler.y = 60.0f;
		_openDoorTimer.Reset ();
		Debug.Log ("Close Office is called");
	}

	void OnEnable(){
		Events.G.AddListener<LockCellEvent>(CloseDoor);
		Events.G.AddListener<OfficeDoorEvent>(CloseOffice);
	}

	void OnDisable(){
		Events.G.RemoveListener<LockCellEvent>(CloseDoor);
		Events.G.RemoveListener<OfficeDoorEvent>(CloseOffice);
	}
}

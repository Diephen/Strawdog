using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpenTransition : MonoBehaviour {
	[SerializeField] BoxCollider2D _doorTrigger;
	[SerializeField] GameObject _door;
	[SerializeField] Animator _doorAnim;
	[SerializeField] SpriteRenderer[] _doorFrame;
	//[SerializeField] GameObject[] _instruction;
	bool _openDoor = false;
	bool _isEnter = false;
	bool _isFirstTimeUp = true;
	Timer _openDoorTimer;
	Vector3 _tempEuler;
	Quaternion _originalRotation;
	SpriteRenderer _spriteRenderer;

	Color _tempSpriteColor;

	void Start() {
		_openDoorTimer = new Timer (2f);
		_originalRotation = transform.rotation;
		_spriteRenderer = _door.GetComponent<SpriteRenderer>();
		_tempSpriteColor = _spriteRenderer.color;
		_doorTrigger.enabled = false;
		//_instruction [0].SetActive (false);
		//_instruction [1].SetActive (false);
	}

	void FixedUpdate () {
		if (_openDoor) {
			print ("open");
			_door.transform.rotation = Quaternion.Lerp (_originalRotation, Quaternion.Euler (_tempEuler), _openDoorTimer.PercentTimePassed);
			//_tempSpriteColor.a = _openDoorTimer.PercentTimeLeft;
		} else {
			_door.transform.rotation = Quaternion.Lerp (transform.rotation, _originalRotation, _openDoorTimer.PercentTimePassed);
			//_tempSpriteColor.a = _openDoorTimer.PercentTimePassed;
		}
		//_spriteRenderer.color =  _tempSpriteColor;
	}

	void CloseDoor(LockCellEvent e){
		_openDoor = !e.Locked;
		_tempEuler = _originalRotation.eulerAngles;
		_tempEuler.y = 60.0f;
		_openDoorTimer.Reset ();
	}

	void CloseOffice(OfficeDoorEvent e){
		print ("## Check door " + e.Opened);
		_openDoor = e.Opened;
		_tempEuler = _originalRotation.eulerAngles;
		_tempEuler.y = 60.0f;
		_openDoorTimer.Reset ();
		if (_openDoor) {
			_doorTrigger.enabled = true;
			foreach (SpriteRenderer spr in _doorFrame) {
				spr.sortingOrder += 100;
			}
//			_instruction [0].SetActive (true);
//			_instruction [1].SetActive (false);

		} else {
			_doorTrigger.enabled = false;
			foreach (SpriteRenderer spr in _doorFrame) {
				spr.sortingOrder -= 100;
			}
//			_instruction [0].SetActive (false);
//			_instruction [1].SetActive (true);
		}
	}

	void DoorTransitionHandle(DoorTransitionEvent e){
		if (_isEnter != e.GoUp && _doorAnim != null &&_openDoor) {
			_isEnter = e.GoUp;
			if (_isEnter) {
				_doorAnim.Play ("Up");
			} else {
				_doorAnim.Play ("Down");
			}
		}


	}

	void OnEnable(){
		Events.G.AddListener<LockCellEvent>(CloseDoor);
		Events.G.AddListener<OfficeDoorEvent>(CloseOffice);
		Events.G.AddListener<DoorTransitionEvent>(DoorTransitionHandle);
	}

	void OnDisable(){
		Events.G.RemoveListener<LockCellEvent>(CloseDoor);
		Events.G.RemoveListener<OfficeDoorEvent>(CloseOffice);
		Events.G.RemoveListener<DoorTransitionEvent>(DoorTransitionHandle);
	}
}

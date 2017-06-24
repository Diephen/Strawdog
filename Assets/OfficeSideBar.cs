using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeSideBar : MonoBehaviour {
	BoxCollider2D _boxCollider2D;
	[SerializeField] SpriteRenderer _spriteRenderer;
	Color _tempSpriteColor;
	bool _openDoor = false;
	Timer _openDoorTimer;
	// Use this for initialization
	void Start () {
		_openDoorTimer = new Timer (2f);
		_boxCollider2D = gameObject.GetComponent<BoxCollider2D>();
	}
	
	void FixedUpdate () {
		if (_openDoor) {
			_tempSpriteColor.a = _openDoorTimer.PercentTimePassed;
		} else {
			_tempSpriteColor.a = _openDoorTimer.PercentTimeLeft;
		}
		_spriteRenderer.color =  _tempSpriteColor;
	}

	void CloseOffice(OfficeDoorEvent e){
		_openDoor = e.Opened;
		if(_openDoor){
			_boxCollider2D.enabled = true;
		} else {
			_boxCollider2D.enabled = false;
		}
		_openDoorTimer.Reset ();
	}

	void OnEnable(){
		Events.G.AddListener<OfficeDoorEvent>(CloseOffice);
	}

	void OnDisable(){
		Events.G.RemoveListener<OfficeDoorEvent>(CloseOffice);
	}
}

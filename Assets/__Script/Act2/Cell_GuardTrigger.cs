﻿using UnityEngine;
using System.Collections;

public class Cell_GuardTrigger : MonoBehaviour {
	[SerializeField] bool _isGuardTop = false;
	[SerializeField] GameObject _guard;
	PuppetControl _guardPuppetController;
	KeyCode[] _guardKeyCodes;

	bool _isStairs = false;
	bool _isDoor = false;
	bool _isSleep = false;
	bool _isBomb = false;
	bool _isBombArea = false;

	[SerializeField] float _stairStartPosition = 28f;
	bool _goToStart = false;
	Timer _stairStartTimer;
	bool _climbStair = false;
	bool _secretDoor = false;
	Vector3 _tempPosition;
	int _waveCnt = 0;

	bool _locked = true;
	[SerializeField] bool _isAct2 = false;


	Camera _mainCam;

	[SerializeField] Renderer _stairRenderer;
	[SerializeField] Renderer _doorRenderer;
	[SerializeField] Renderer _secretDoorRenderer;

	[SerializeField] GameObject _bomb;
	Bomb _bombScript;

	[SerializeField] BoxCollider2D _groundCollider1;
	[SerializeField] BoxCollider2D _groundCollider2;


	[SerializeField] BoxCollider2D _guardStop;
	[SerializeField] BoxCollider2D _prisonerStop;

	bool _isOnFlap = false;
	[SerializeField] SpriteRenderer _leftFlap;
	[SerializeField] SpriteRenderer _rightFlap;


	void Start(){
		_guardPuppetController = _guard.GetComponent <PuppetControl> ();
		_guardKeyCodes = _guardPuppetController.GetKeyCodes ();
		_stairStartTimer = new Timer (1f);
		_mainCam = Camera.main;
		if (_isGuardTop) {
			_bomb = GameObject.Find ("Bomb");
			_bombScript = _bomb.GetComponent<Bomb> ();
		}
	}

	void Update(){
		if (Input.GetKeyDown (_guardKeyCodes [3])) {
			if (_isStairs) {
				_guardPuppetController.StopWalkAudio ();
				_goToStart = true;
				_isStairs = false;
				_stairRenderer.gameObject.GetComponentInChildren<HighlightSprite> ().DisableHighlight();
				_guardPuppetController.DisableKeyInput ();
				_tempPosition = _guard.transform.position;
				_stairStartTimer.Reset ();
				Events.G.Raise (new GuardStairsStartEvent ());
			}
			else if (_isOnFlap) {
				Events.G.Raise (new LeftCellUnlockedEvent ());
				_guard.SetActive (false);
				_rightFlap.gameObject.GetComponentInChildren<HighlightSprite> ().DisableHighlight();
				_leftFlap.gameObject.GetComponentInChildren<HighlightSprite> ().DisableHighlight();
				_isOnFlap = false;
				_guardStop.enabled = false;
				_prisonerStop.enabled = true;

			}
			else if (_isDoor) {
				_locked = !_locked;
				Events.G.Raise (new LockCellEvent (_locked));
				if (_locked) {
					Events.G.Raise (new EnableMoveEvent (CharacterIdentity.Guard));
				}
			}

		}

		if (_goToStart) {
			_tempPosition.x = Mathf.Lerp (_tempPosition.x, _stairStartPosition, _stairStartTimer.PercentTimePassed);
			_guard.transform.position = _tempPosition;
			if (_stairStartTimer.IsOffCooldown) {
				if (!_isAct2) {
					SoldierFlip ();
				}
				_goToStart = false;
				_climbStair = true;
			}
		}
		else if (_climbStair && !_isGuardTop) {
			_guard.transform.Translate ((Vector3.left + Vector3.up) * 2.0f * Time.deltaTime);
			Events.G.Raise (new Act2_GuardWalkedUpStairsEvent ());
		}
		else if (_climbStair && _isGuardTop) {
			_groundCollider1.enabled = false;
			_groundCollider2.enabled = false;
			_guard.transform.Translate ((Vector3.left + Vector3.down) * 2.0f * Time.deltaTime);
			Events.G.Raise (new Act2_GuardWalkedDownStairsEvent ());
		}

		if (_isGuardTop) {
			if (_secretDoor && Input.GetKeyDown (_guardKeyCodes [3])) {
				Events.G.Raise (new CallSecretDoorEvent ());
			}
			else if (_isSleep && Input.GetKeyDown (_guardKeyCodes [3])) {
				Events.G.Raise (new GuardSleepEvent ());
			}
			else if (_isBomb && Input.GetKeyDown (_guardKeyCodes [3])) {
				_bomb.SetActive (false);
				Events.G.Raise (new GuardFoundBombEvent ());
			}

			if (_isBombArea) {
				if (_waveCnt == 0) {
					if (Input.GetKeyDown (_guardKeyCodes [0])) {
						_waveCnt = 1;
					}
					else if(
						Input.GetKeyDown(_guardKeyCodes[1]) || 
						Input.GetKeyDown(_guardKeyCodes[2]) || 
						Input.GetKeyDown(_guardKeyCodes[3])) 
					{
						_waveCnt = 0;
					}
				}
				else if (_waveCnt == 1) {
					if (Input.GetKeyDown (_guardKeyCodes [1])) {
						_waveCnt = 2;
					}
					else if(
						Input.GetKeyDown(_guardKeyCodes[0]) || 
						Input.GetKeyDown(_guardKeyCodes[2]) || 
						Input.GetKeyDown(_guardKeyCodes[3])){
						_waveCnt = 0;
					}
				}
				else if (_waveCnt == 2) {
					if (Input.GetKeyDown (_guardKeyCodes [0])) {
						_waveCnt = 3;
					}
					else if(
						Input.GetKeyDown(_guardKeyCodes[1]) || 
						Input.GetKeyDown(_guardKeyCodes[2]) || 
						Input.GetKeyDown(_guardKeyCodes[3])) {
						_waveCnt = 0;
					}
				}
				else if (_waveCnt == 3) {
					if (Input.GetKeyDown (_guardKeyCodes [2])) {
						_waveCnt = 4;
						_bombScript.ThrowBomb ();
					}
					else if(
						Input.GetKeyDown(_guardKeyCodes[0]) || 
						Input.GetKeyDown(_guardKeyCodes[1]) || 
						Input.GetKeyDown(_guardKeyCodes[3])) {
						_waveCnt = 0;
					}
				}
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Stairs") {
			_isStairs = true;
			other.GetComponentInChildren<HighlightSprite> ().EnableHighlight ();
		}
		else if (other.tag == "SecretDoor") {
			other.GetComponentInChildren<HighlightSprite> ().EnableHighlight ();
			_secretDoor = true;
		}
		else if (other.name == "LockCell") {
			other.GetComponentInChildren<HighlightSprite> ().EnableHighlight ();
			_isDoor = true;
		}
		else if (other.name == "Bomb") {
			other.GetComponentInChildren<HighlightSprite> ().EnableHighlight ();
			_isBomb = true;
		}
		else if (other.name == "open-right") {
			if (_isDoor) {
				_doorRenderer.gameObject.GetComponentInChildren<HighlightSprite> ().DisableHighlight ();
			}
			_isOnFlap = true;
			other.GetComponentInChildren<HighlightSprite> ().EnableHighlight ();
		}
		else if (other.name == "open-left") {
			other.GetComponentInChildren<HighlightSprite> ().EnableHighlight ();
			_isOnFlap = true;
		}
		else if (other.name == "sh-front_0") {
			other.GetComponentInChildren<HighlightSprite> ().EnableHighlight ();
			_isSleep = true;
		}
		else if (other.name == "BombArea") {
			_isBombArea = true;
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.name == "BombArea") {
			_waveCnt = 0;
			_isBombArea = false;
		} else if (other.tag == "Stairs") {
			_isStairs = false;
			other.GetComponentInChildren<HighlightSprite> ().DisableHighlight();
		} 
		else if (other.tag == "SecretDoor") {
			other.GetComponentInChildren<HighlightSprite> ().DisableHighlight ();
			_secretDoor = false;
		}
		else if (other.name == "LockCell") {
			other.GetComponentInChildren<HighlightSprite> ().DisableHighlight();
			if (_isOnFlap) {
				_rightFlap.gameObject.GetComponentInChildren<HighlightSprite> ().EnableHighlight();
			}
			else {
				other.GetComponentInChildren<HighlightSprite> ().DisableHighlight();

			}
			_isDoor = false;
		} else if (other.name == "Bomb") {
			other.GetComponentInChildren<HighlightSprite> ().DisableHighlight ();
			_isBomb = false;
		} else if (other.name == "open-right") {
			other.GetComponentInChildren<HighlightSprite> ().DisableHighlight();
			_isOnFlap = false;
			if (_isDoor) {
				_doorRenderer.gameObject.GetComponentInChildren<HighlightSprite> ().EnableHighlight ();
			}
		}
		else if (other.name == "open-left") {
			other.GetComponentInChildren<HighlightSprite> ().DisableHighlight();
			_isOnFlap = false;
		} else if (other.name == "sh-front_0") {
			other.GetComponentInChildren<HighlightSprite> ().DisableHighlight ();
			_isSleep = false;
		}
	}

	void SoldierFlip(){
		Vector3 _temp = _guard.transform.localPosition;
		if (_guard.transform.localScale.x >= 0) {
			_temp.x += 2.1f;
		}
		else {
			_temp.x -= 2.1f;
		}
		_guard.transform.localPosition = _temp;
		_temp = _guard.transform.localScale;
		_temp.x = _temp.x * -1.0f;
		_guard.transform.localScale = _temp;
	}
}

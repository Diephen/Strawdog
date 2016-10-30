﻿using UnityEngine;
using System.Collections;

public class Cell_PrisonerTrigger : MonoBehaviour {
	[SerializeField] bool _isPrisonerTop = false;

	[SerializeField] GameObject _prisoner;
	PuppetControl _prisonerPuppetController;
	KeyCode[] _prisonerKeyCodes;

	[SerializeField] Renderer _stairRenderer;
	[SerializeField] Renderer _bedRenderer;
	[SerializeField] Renderer _bombRenderer;
	[SerializeField] Renderer _secretDoorRenderer;

	int _waveCnt = 0;

	bool _isStairs = false;
	[SerializeField] float _stairStartPosition = 28.6f;


	bool _goToStart = false;
	Timer _stairStartTimer;
	bool _climbStair = false;

	bool _guardLeftCell = false;

	bool _crouchHideReady = false;
	bool _isHidden = false;
	bool _secretDoor = false;

	Vector3 _tempPosition;

	Camera _mainCam;
	//HighlightsFX _highlightsFX;
	[SerializeField] GameObject _bomb;
	Bomb _bombScript;

	[SerializeField] BoxCollider2D _groundCollider1;
	[SerializeField] BoxCollider2D _groundCollider2;

	void Start(){
		gameObject.tag = "Prisoner";
		_prisonerPuppetController = _prisoner.GetComponent <PuppetControl> ();
		_prisonerKeyCodes = _prisonerPuppetController.GetKeyCodes ();
		_stairStartTimer = new Timer (1f);
		_mainCam = Camera.main;
		//_highlightsFX = _mainCam.GetComponent <HighlightsFX> ();
		if (_isPrisonerTop) {
			_bomb = GameObject.Find ("Bomb");
			_bombScript = _bomb.GetComponent<Bomb> ();
			_bombRenderer = _bomb.GetComponent<Renderer> ();
		}
	}

	void Update(){

		if (Input.GetKeyDown (_prisonerKeyCodes [3]) && _isStairs) {
			_goToStart = true;
			_isStairs = false;
			//_highlightsFX.enabled = false;
			_stairRenderer.gameObject.GetComponentInChildren<HighlightSprite> ().DisableHighlight();
			_prisonerPuppetController.DisableKeyInput ();
			_tempPosition = _prisoner.transform.position;
			_stairStartTimer.Reset ();
			Events.G.Raise (new PrisonerStairsStartEvent ());
		}

		if (_goToStart) {
			_tempPosition.x = Mathf.Lerp (_tempPosition.x, _stairStartPosition, _stairStartTimer.PercentTimePassed);
			_prisoner.transform.position = _tempPosition;
			if (_stairStartTimer.IsOffCooldown) {
				_goToStart = false;
				_climbStair = true;
			}
		}
		else if (_climbStair && !_isPrisonerTop) {
			_prisoner.transform.Translate ((Vector3.right + Vector3.up) * 2.0f * Time.deltaTime);
			Events.G.Raise (new Act2_PrisonerWalkedUpStairsEvent ());
		}
		else if (_climbStair && _isPrisonerTop) {
			_groundCollider1.enabled = false;
			_groundCollider2.enabled = false;
			_prisoner.transform.Translate ((Vector3.left + Vector3.down) * 2.0f * Time.deltaTime);
			Events.G.Raise (new Act2_PrisonerWalkedDownStairsEvent ());
		}

		if (_secretDoor && Input.GetKeyDown (_prisonerKeyCodes [3])) {
			Events.G.Raise (new CallSecretDoorEvent ());
			//_highlightsFX.enabled = false;
		}

		//Checking if prisoner should be detectable or not
		if (_isHidden && gameObject.tag == "Prisoner") {
			gameObject.tag = "Untagged";
		}
		else if (!_isHidden && gameObject.tag == "Untagged") {
			gameObject.tag = "Prisoner";
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Stairs") {
			_isStairs = true;
			other.GetComponentInChildren<HighlightSprite> ().EnableHighlight();
			//_highlightsFX.objectRenderer = _stairRenderer;
			//_highlightsFX.enabled = true;
		}
		else if (other.name == "Bed" && _guardLeftCell) {
			other.GetComponentInChildren<HighlightSprite> ().EnableHighlight();
			//_highlightsFX.objectRenderer = _bedRenderer;
			//_highlightsFX.enabled = true;
		}
		else if (other.name == "Bomb") {
			//_highlightsFX.objectRenderer = _bombRenderer;
			//_highlightsFX.enabled = true;
		}

		if (_isPrisonerTop) {
			if (other.tag == "StandHide") {
				_isHidden = true;
				Events.G.Raise (new PrisonerHideEvent (_isHidden));
				Debug.Log ("[Hide] stand hide");
			}
			else if (other.tag == "SecretDoor") {
				other.GetComponentInChildren<HighlightSprite> ().EnableHighlight();
				_secretDoor = true;
				//_highlightsFX.objectRenderer = _secretDoorRenderer;
				//_highlightsFX.enabled = true;
			}
		}
	}

	void OnTriggerStay2D(Collider2D other){
		if (_isPrisonerTop) {
			if (other.tag == "CrouchHide") {
				_crouchHideReady = true;
			}
		}

		if (other.name == "Bed" && _guardLeftCell) {
			if (Input.GetKeyDown (_prisonerKeyCodes [3])) {
				Debug.Log ("Prisoner going to bed");
				Events.G.Raise (new SleepInCellEvent ());
			}
		}
		else if (other.name == "Bomb") {
			if (Input.GetKeyDown (_prisonerKeyCodes [3])) {
				//_highlightsFX.enabled = false;
				_bomb.SetActive (false);
				Events.G.Raise (new PrisonerFoundBombEvent ());
			}
		}
		if (other.name == "BombArea") {
			if(Input.GetKeyDown (_prisonerKeyCodes[2])){
				_waveCnt++;
				if (_waveCnt == 3) {
					_bombScript.ThrowBomb ();
				}
			}
		}
	}


	void OnTriggerExit2D(Collider2D other) {
		if (other.name == "BombArea") {
			_waveCnt = 0;
		} else if (other.tag == "Stairs") {
			_isStairs = false;
			//_highlightsFX.enabled = false;
			other.GetComponentInChildren<HighlightSprite> ().DisableHighlight();
		} else if (other.name == "Bed") {
			//_highlightsFX.enabled = false;
			other.GetComponentInChildren<HighlightSprite> ().DisableHighlight();
		} else if (other.name == "Bomb") {
			//_highlightsFX.enabled = false;
		}

		if (_isPrisonerTop) {
			if (other.tag == "CrouchHide") {
				_crouchHideReady = false;
			}
			else if (other.tag == "StandHide") {
				_isHidden = false;
				Events.G.Raise (new PrisonerHideEvent (_isHidden));
			}
			else if (other.tag == "SecretDoor") {
				other.GetComponentInChildren<HighlightSprite> ().DisableHighlight();
				_secretDoor = false;
				//_highlightsFX.enabled = false;
			}
		}
	}

	void LeftCellUnlocked(LeftCellUnlockedEvent e){
		_guardLeftCell = true;
	}

	void CrouchHide(CrouchHideEvent e){
		if (_crouchHideReady) {
			_isHidden = true;
			Events.G.Raise (new PrisonerHideEvent (_isHidden));
			Debug.Log ("[Hide] crouch hide");
		}
	}

	void CrouchRelease(CrouchReleaseHideEvent e){
		if (_crouchHideReady) {
			_isHidden = false;
			Events.G.Raise (new PrisonerHideEvent (_isHidden));
		}
	}

	void OnEnable(){
		Events.G.AddListener<LeftCellUnlockedEvent>(LeftCellUnlocked);
		Events.G.AddListener<CrouchHideEvent>(CrouchHide);
		Events.G.AddListener<CrouchReleaseHideEvent>(CrouchRelease);
	}
	void OnDisable(){
		Events.G.RemoveListener<LeftCellUnlockedEvent>(LeftCellUnlocked);
		Events.G.RemoveListener<CrouchHideEvent>(CrouchHide);
		Events.G.RemoveListener<CrouchReleaseHideEvent>(CrouchRelease);
	}
}

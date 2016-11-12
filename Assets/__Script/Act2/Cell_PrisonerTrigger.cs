using UnityEngine;
using System.Collections;
using System;

public class Cell_PrisonerTrigger : MonoBehaviour {
	[SerializeField] bool _isPrisonerTop = false;

	[SerializeField] GameObject _prisoner;
	PuppetControl _prisonerPuppetController;
	KeyCode[] _prisonerKeyCodes;

	[SerializeField] bool _backDown = false;

	[SerializeField] Renderer _stairRenderer;
	[SerializeField] Renderer _bedRenderer;
	[SerializeField] Renderer _secretDoorRenderer;

	int _waveCnt = 0;

	bool _isStairs = false;
	bool _isBombArea = false;
	bool _isBomb = false;
	bool _isBed = false;
	bool _isOnFlap = false;
	[SerializeField] float _stairStartPosition = 28.6f;


	bool _goToStart = false;
	Timer _stairStartTimer;
	bool _climbStair = false;

	bool _guardLeftCell = false;

	bool _crouchHideReady = false;
	bool _isHidden = false;
	bool _secretDoor = false;

	bool _leaveArea = false;
	bool _bombGiven = false;

	Vector3 _tempPosition;
	Timer _offsetTimer = new Timer (0.3f);
	Camera _mainCam;
	[SerializeField] GameObject _bomb;
	Bomb _bombScript;

	[SerializeField] BoxCollider2D _groundCollider1;
	[SerializeField] BoxCollider2D _groundCollider2;

	[SerializeField] GameObject _leftFlap;
	AudioSource _audioSource;
	GameObject _crouchTemp;

	void Start(){
		gameObject.tag = "Prisoner";
		_prisonerPuppetController = _prisoner.GetComponent <PuppetControl> ();
		_prisonerKeyCodes = _prisonerPuppetController.GetKeyCodes ();
		_stairStartTimer = new Timer (1f);
		_mainCam = Camera.main;
		if (_isPrisonerTop) {
			_bomb = GameObject.Find ("Bomb");
			_bombScript = _bomb.GetComponent<Bomb> ();
		}
		_audioSource = gameObject.AddComponent<AudioSource> ();
		_audioSource.clip = Resources.Load<AudioClip> ("Sounds/PianoPluckShorter");
		_audioSource.volume = 0.4f;
	}

	void Update(){

		if (Input.GetKeyDown (_prisonerKeyCodes [3]) && _isStairs) {
			_goToStart = true;
			_isStairs = false;
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
		}

		//Checking if prisoner should be detectable or not
		if (_isHidden && gameObject.tag == "Prisoner") {
			gameObject.tag = "Untagged";
		}
		else if (!_isHidden && gameObject.tag == "Untagged") {
			gameObject.tag = "Prisoner";
		}

		if (_isBed && Input.GetKeyDown (_prisonerKeyCodes [3])) {
			Events.G.Raise (new SleepInCellEvent ());
		}

		if (_isPrisonerTop) {
			if (_isBomb && Input.GetKeyDown (_prisonerKeyCodes [3])) {
				_bomb.SetActive (false);
				Events.G.Raise (new PrisonerFoundBombEvent ());
			} else if (_isOnFlap && Input.GetKeyDown (_prisonerKeyCodes [3])) {
				Events.G.Raise (new PrisonerWentBack ());
				_leftFlap.gameObject.GetComponentInChildren<HighlightSprite> ().DisableHighlight();
				_isOnFlap = false;
				_leaveArea = true;
				Events.G.Raise (new EnableMoveEvent (CharacterIdentity.Prisoner));
			}

			BombDetection ();

		}

		if (_leaveArea) {
			_prisoner.transform.Translate (Vector3.left * Time.deltaTime * 2.0f);
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		Debug.Log ("Prisoner "+ other.tag);
		if (other.tag == "Stairs") {
			_isStairs = true;
			other.GetComponentInChildren<HighlightSprite> ().EnableHighlight();
		}
		else if (other.name == "Bed" && _guardLeftCell || (other.name == "Bed" && _backDown)) {
			other.GetComponentInChildren<HighlightSprite> ().EnableHighlight();
			_isBed = true;
		}
		else if (other.name == "Bomb") {
			other.GetComponentInChildren<HighlightSprite> ().EnableHighlight();
			_isBomb = true;
		}

		if (_isPrisonerTop) {
			if (other.name == "BombArea") {
				_isBombArea = true;
			} 
			else if (other.tag == "StandHide") {
				_isHidden = true;
				Events.G.Raise (new PrisonerHideEvent (_isHidden));
				if (other.gameObject.GetComponent<HideFeedback> () != null) {
					other.gameObject.GetComponent<HideFeedback> ().LightUp (other.gameObject);
				}
				Debug.Log ("[Hide] stand hide");
			}
			else if (other.tag == "SecretDoor") {
				other.GetComponentInChildren<HighlightSprite> ().EnableHighlight();
				_secretDoor = true;
			} else if (other.name == "open-left") {
				other.GetComponentInChildren<HighlightSprite> ().EnableHighlight ();
				_isOnFlap = true;
			}
		}
	}

	void OnTriggerStay2D(Collider2D other){
		if (_isPrisonerTop) {
			if (other.tag == "CrouchHide") {
				_crouchHideReady = true;
				_crouchTemp = other.gameObject;
			}
		}
	}


	void OnTriggerExit2D(Collider2D other) {
		if (other.name == "BombArea") {
			_isBombArea = false;
			_waveCnt = 0;
		} else if (other.tag == "Stairs") {
			_isStairs = false;
			other.GetComponentInChildren<HighlightSprite> ().DisableHighlight();
		} else if (other.name == "Bed") {
			_isBed = false;
			other.GetComponentInChildren<HighlightSprite> ().DisableHighlight();
		} else if (other.name == "Bomb") {
			other.GetComponentInChildren<HighlightSprite> ().DisableHighlight();
			_isBomb = false;
		}

		if (_isPrisonerTop) {
			if (other.tag == "CrouchHide") {
				_crouchHideReady = false;
//				other.gameObject.GetComponent<HideFeedback> ().LightDown ();
			}
			else if (other.tag == "StandHide") {
				_isHidden = false;
				Events.G.Raise (new PrisonerHideEvent (_isHidden));

				if (other.gameObject.GetComponent<HideFeedback> () != null) {
					other.gameObject.GetComponent<HideFeedback> ().LightDown (other.gameObject);
				}
			}
			else if (other.tag == "SecretDoor") {
				other.GetComponentInChildren<HighlightSprite> ().DisableHighlight();
				_secretDoor = false;
			} else if (other.name == "open-left") {
				other.GetComponentInChildren<HighlightSprite> ().DisableHighlight ();
				_isOnFlap = false;
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
			_crouchTemp.GetComponent<HideFeedback> ().LightUp (_crouchTemp);
		}
	}

	void CrouchRelease(CrouchReleaseHideEvent e){
		if (_crouchHideReady) {
			_isHidden = false;
			Events.G.Raise (new PrisonerHideEvent (_isHidden));
			_crouchTemp.GetComponent<HideFeedback> ().LightDown (_crouchTemp);
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

	void BombDetection(){
		if (_isBombArea && !_bombGiven) {
			if (_waveCnt == 0) {
				if (Input.GetKeyDown (_prisonerKeyCodes [2])) {
					_waveCnt = 1;
					_audioSource.Play ();
					_audioSource.pitch = 1.0f;
					_offsetTimer.Reset ();
				}
				else if (Input.anyKeyDown){
					_waveCnt = 0;
				}
			}
			else if (_waveCnt == 1) {
				if (Input.GetKeyDown (_prisonerKeyCodes [1]) && _offsetTimer.IsOffCooldown) {
					_waveCnt = 2;
					_audioSource.Play ();
					_audioSource.pitch = 1.1f;
					_offsetTimer.Reset ();
				}
				else if (Input.anyKeyDown) {
					_waveCnt = 0;
				}
			}
			else if (_waveCnt == 2) {
				if (Input.GetKeyDown (_prisonerKeyCodes [2]) && _offsetTimer.IsOffCooldown) {
					_waveCnt = 3;
					_audioSource.Play ();
					_audioSource.pitch = 1.2f;
					_offsetTimer.Reset ();
				}
				else if (Input.anyKeyDown) {
					_waveCnt = 0;
				}
			}
			else if (_waveCnt == 3) {
				if (Input.GetKeyDown (_prisonerKeyCodes [0]) && _offsetTimer.IsOffCooldown) {
					_waveCnt = 4;
					_audioSource.Play ();
					_bombScript.ThrowBomb ();
					_audioSource.pitch = 0.9f;
					_bombGiven = true;
				}
				else if (Input.anyKeyDown) {
					_waveCnt = 0;
				}
			}
		}
	}
}

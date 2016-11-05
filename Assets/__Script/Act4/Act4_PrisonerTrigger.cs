using UnityEngine;
using System.Collections;

public class Act4_PrisonerTrigger : MonoBehaviour {
	enum prisonerState {Act4Begin};
	prisonerState _prisonerState = prisonerState.Act4Begin;

	[SerializeField] GameObject _prisoner;
	[SerializeField] SpriteRenderer[] _whiteBase = new SpriteRenderer[17];
	Color _originalColor;

	PuppetControl _prisonerPuppetController;
//	[SerializeField] MinMax _stairRange = new MinMax(0.2f, 9.8f); 
	KeyCode[] _prisonerKeyCodes;

	int _waveCnt = 0;
	Timer _alertTimer;

	bool _isSafe = true;

	bool _secretDoor = false;
	bool _isOnFlap = false;
	bool _isOnGuard = false;

	[SerializeField] SpriteRenderer _rightFlap;

	void Start(){
		_prisonerPuppetController = _prisoner.GetComponent <PuppetControl> ();
		_prisonerKeyCodes = _prisonerPuppetController.GetKeyCodes ();
		_alertTimer = new Timer (2.0f);
	}

	void Update(){
//		if (_isStairs) {
//			//			Debug.Log (_guard.transform.position.y);
//			if (((_prisoner.transform.position.y < _stairRange.Min) && (_stairTempPosition>_stairRange.Min)) 
//				|| ((_stairTempPosition < _stairRange.Max) &&(_prisoner.transform.position.y > _stairRange.Max))) {
//				_isStairs = false;
//
//				_prisonerPuppetController.SetIsStairs (_isStairs);
//			}
//		}
//		if (_waveCnt >= 3) {
//			_waveCnt = 0;
//			Events.G.Raise(new PrisonerFoundBombEvent());
//			Debug.Log ("Prisoner Found Bomb");
//		}
		if (_alertTimer.IsOffCooldown) {
			Events.G.Raise (new StrayOutOfLineEvent());
			//Leads to triggering Death Anim
		}
			
		if (_secretDoor && Input.GetKeyDown (_prisonerKeyCodes [3])) {
			Events.G.Raise (new CallSecretDoorEvent ());
			//_highlightsFX.enabled = false;
		} else if(_isOnFlap && Input.GetKeyDown (_prisonerKeyCodes [3])) {
//			_guard.SetActive (false);
			//_highlightsFX.enabled = false;
			_rightFlap.gameObject.GetComponentInChildren<HighlightSprite> ().DisableHighlight();
			_isOnFlap = false;

		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.name == "AlertTrigger") {
			_isSafe = true;
			Events.G.Raise (new AboutToStrayOutOfLineEvent (false));
		}
		else if (other.name == "Encounter") {
			Events.G.Raise (new Prisoner_EncounterEvent ());
			_prisonerPuppetController.DisableContinuousWalk ();
			other.enabled = false;
		}
		else if (other.tag == "SecretDoor") {
			other.GetComponentInChildren<HighlightSprite> ().EnableHighlight ();
			_secretDoor = true;
		}
		else if (other.name == "open-right") {
			other.GetComponentInChildren<HighlightSprite> ().EnableHighlight ();
			//_highlightsFX.objectRenderer = _rightFlap;
			//_highlightsFX.enabled = true;
			_isOnFlap = true;
			if (_isOnGuard) {
				Events.G.Raise (new EncounterTouchEvent (false));
//				for (int i = 0; i < _whiteBase.Length; i++) {
//					_whiteBase [i].color = _originalColor;
//				}
			}
		}
		else if (other.tag == "Guard") {
			// raise event 
			_isOnGuard = true;
			Events.G.Raise (new EncounterTouchEvent (true));
//			_originalColor = _whiteBase [0].color;
//			for (int i = 0; i < _whiteBase.Length; i++) {
//				_whiteBase [i].color = Color.white;
//			}
		}

//		if (other.tag == "Stairs") {
//			_isStairs = true;
//			_stairTempPosition = _prisoner.transform.position.y;
//			_prisonerPuppetController.SetIsStairs (_isStairs);
//		}
		//		if (other.name == "EngagePrisoner") {
		//			_guardState = guardState.EngagedPrisoner;
		//			Events.G.Raise(new GuardEngaginPrisonerEvent(true));
		//		}
	}

	void OnTriggerStay2D(Collider2D other){
//		if (other.name == "Note") {
//			if (Input.GetKey (_prisonerKeyCodes [3])) {
////				Events.G.Raise(new SleepInCellEvent());
//				_note.enabled = true;
//
//			} else {
//				_note.enabled = false;
//			}
//		}
//		if (other.name == "Bomb") {
//			if(Input.GetKeyDown (_prisonerKeyCodes[2])){
//				_waveCnt++;
//			}
//		}
	}


	void OnTriggerExit2D(Collider2D other) {
		if (other.name == "AlertTrigger") {
			_isSafe = false;
			_alertTimer.Reset ();
			Events.G.Raise (new AboutToStrayOutOfLineEvent (true));
		}
		else if (other.tag == "SecretDoor") {
			other.GetComponentInChildren<HighlightSprite> ().DisableHighlight ();
			_secretDoor = false;
		}
		else if (other.name == "open-right") {
			other.GetComponentInChildren<HighlightSprite> ().DisableHighlight ();
			_isOnFlap = false;
			if (_isOnGuard) {
				Events.G.Raise (new EncounterTouchEvent (true));
//				for (int i = 0; i < _whiteBase.Length; i++) {
//					_whiteBase [i].color = Color.white;
//				}
			}
		}
		else if (other.tag == "Guard") {
			// raise event to disable lightup
			_isOnGuard = false;
			Events.G.Raise (new EncounterTouchEvent (false));
//			_isOnGuard = false;
//			for (int i = 0; i < _whiteBase.Length; i++) {
//				_whiteBase [i].color = _originalColor;
//			}
		}
//		if (other.tag == "Stairs") {
//			_isStairs = false;
//			_prisonerPuppetController.SetIsStairs (_isStairs);
//		}
//		if (other.name == "Bomb") {
//			_waveCnt = 0;
//		}
	}
}

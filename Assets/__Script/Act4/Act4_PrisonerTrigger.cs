using UnityEngine;
using System.Collections;

public class Act4_PrisonerTrigger : MonoBehaviour {
	enum prisonerState {Act4Begin};
	prisonerState _prisonerState = prisonerState.Act4Begin;

	[SerializeField] GameObject _prisoner;
	PuppetControl _prisonerPuppetController;
//	[SerializeField] MinMax _stairRange = new MinMax(0.2f, 9.8f); 
	KeyCode[] _prisonerKeyCodes;

	int _waveCnt = 0;
	Timer _alertTimer;

	bool _isSafe = true;
//	bool _isStairs = false;
//	float _stairTempPosition;

	void Start(){
		_prisonerPuppetController = _prisoner.GetComponent <PuppetControl> ();
		_prisonerKeyCodes = _prisonerPuppetController.GetKeyCodes ();
		_alertTimer = new Timer (2.0f);
	}

	void Update(){
		if (Input.GetKeyDown (KeyCode.RightShift)) {
			Events.G.Raise (new ShootEvent());
		}
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
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.name == "AlertTrigger") {
			_isSafe = true;
			Events.G.Raise (new AboutToStrayOutOfLineEvent(false));
		} else if(other.name == "Encounter"){
			Events.G.Raise (new Prisoner_EncounterEvent());
			other.enabled = false;
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
			Events.G.Raise (new AboutToStrayOutOfLineEvent(true));
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

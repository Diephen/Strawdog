using UnityEngine;
using System.Collections;

public class Act3_PrisonerTrigger : MonoBehaviour {
	enum prisonerState {Act3Begin};
	prisonerState _prisonerState = prisonerState.Act3Begin;

	[SerializeField] GameObject _prisoner;
	[SerializeField] PuppetControl _prisonerPuppetController;
	[SerializeField] MeshRenderer _note;
//	[SerializeField] MinMax _stairRange = new MinMax(0.2f, 9.8f); 
	KeyCode[] _prisonerKeyCodes;

	int _waveCnt = 0;

//	bool _isStairs = false;
//	float _stairTempPosition;

	void Start(){
		_prisonerKeyCodes = _prisonerPuppetController.GetKeyCodes ();
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
	}

	void OnTriggerEnter2D(Collider2D other) {

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
		if (other.name == "Note") {
			if (Input.GetKey (_prisonerKeyCodes [3])) {
//				Events.G.Raise(new SleepInCellEvent());
				_note.enabled = true;

			} else {
				_note.enabled = false;
			}
		}
//		if (other.name == "Bomb") {
//			if(Input.GetKeyDown (_prisonerKeyCodes[2])){
//				_waveCnt++;
//			}
//		}
	}


	void OnTriggerExit2D(Collider2D other) {
//		if (other.tag == "Stairs") {
//			_isStairs = false;
//			_prisonerPuppetController.SetIsStairs (_isStairs);
//		}
//		if (other.name == "Bomb") {
//			_waveCnt = 0;
//		}
	}
}

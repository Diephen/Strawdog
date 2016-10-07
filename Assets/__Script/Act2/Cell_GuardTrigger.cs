using UnityEngine;
using System.Collections;

public class Cell_GuardTrigger : MonoBehaviour {
	enum guardState {Act2Begin, Locked, LeftUnlocked};
	guardState _guardState = guardState.Act2Begin;

	[SerializeField] GameObject _guard;
	[SerializeField] PuppetControl _guardPuppetController;
	[SerializeField] MinMax _stairRange = new MinMax(0.2f, 9.8f); 
	KeyCode[] _guardKeyCodes;

	bool _isStairs = false;
	float _stairTempPosition;

	int _waveCnt = 0;

	void Start(){
		_guardKeyCodes = _guardPuppetController.GetKeyCodes ();
	}

	void Update(){
		if (_isStairs) {
//			Debug.Log (_guard.transform.position.y);
			if (((_guard.transform.position.y < _stairRange.Min) && (_stairTempPosition>_stairRange.Min)) 
				|| ((_stairTempPosition < _stairRange.Max) &&(_guard.transform.position.y > _stairRange.Max))) {
				_isStairs = false;

				_guardPuppetController.SetIsStairs (_isStairs);
			}
		}

		if (_waveCnt >= 3) {
			_waveCnt = 0;
			Events.G.Raise(new GuardFoundBombEvent());
			Debug.Log ("Guard Found Bomb");
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if(_guardState == guardState.Act2Begin && other.name == "LeaveUnlocked"){
			_guardState = guardState.LeftUnlocked;
			Events.G.Raise (new LeftCellUnlockedEvent());
			_guard.SetActive (false);
		}

		if (other.tag == "Stairs") {
			Debug.Log ("But Does this Fire?");
			_isStairs = true;
			_stairTempPosition = _guard.transform.position.y;
			_guardPuppetController.SetIsStairs (_isStairs);
		}
		//		if (other.name == "EngagePrisoner") {
		//			_guardState = guardState.EngagedPrisoner;
		//			Events.G.Raise(new GuardEngaginPrisonerEvent(true));
		//		}
	}

	void OnTriggerStay2D(Collider2D other){
		if (_guardState == guardState.Act2Begin && other.name == "LockCell") {
			if(Input.GetKeyDown (_guardKeyCodes[3])){
				_guardState = guardState.Locked;
				Debug.Log ("Guard Locked the door");
				Events.G.Raise(new LockCellEvent());
			}
		}
		if (other.name == "Bomb" && _guardState == guardState.Locked) {
			if(Input.GetKeyDown (_guardKeyCodes[2])){
				_waveCnt++;
			}
		}
	}


	void OnTriggerExit2D(Collider2D other) {
		if (other.tag == "Stairs") {
			_isStairs = false;
			_guardPuppetController.SetIsStairs (_isStairs);
		}
		if (other.name == "Bomb") {
			_waveCnt = 0;
		}
	}
}

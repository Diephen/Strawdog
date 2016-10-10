using UnityEngine;
using System.Collections;

public class Cell_GuardTrigger : MonoBehaviour {
	enum guardState {Act2Begin, Locked, LeftUnlocked};
	guardState _guardState = guardState.Act2Begin;

	[SerializeField] GameObject _guard;
	PuppetControl _guardPuppetController;
	KeyCode[] _guardKeyCodes;

	bool _isStairs = false;

	[SerializeField] float _stairStartPosition = 28f;
	bool _goToStart = false;
	Timer _stairStartTimer;
	bool _climbStair = false;

	Vector3 _tempPosition;


	int _waveCnt = 0;


	Camera _mainCam;
	HighlightsFX _highlightsFX;

	[SerializeField] Renderer _stairRenderer;
	[SerializeField] Renderer _doorRenderer;

	void Start(){
		_guardPuppetController = _guard.GetComponent <PuppetControl> ();
		_guardKeyCodes = _guardPuppetController.GetKeyCodes ();
		_stairStartTimer = new Timer (1f);
		_mainCam = Camera.main;
		_highlightsFX = _mainCam.GetComponent <HighlightsFX> ();
	}

	void Update(){

		if (_waveCnt >= 3) {
			_waveCnt = 0;
			Events.G.Raise(new GuardFoundBombEvent());
			Debug.Log ("Guard Found Bomb");
		}
		if (Input.GetKeyDown (_guardKeyCodes [3]) && _isStairs) {
			_goToStart = true;
			_highlightsFX.enabled = false;
			_guardPuppetController.DisableKeyInput ();
			_tempPosition = _guard.transform.position;
			_stairStartTimer.Reset ();
		}

		if (_goToStart) {
			_tempPosition.x = Mathf.Lerp (_tempPosition.x, _stairStartPosition, _stairStartTimer.PercentTimePassed);
			_guard.transform.position = _tempPosition;
			if (_stairStartTimer.IsOffCooldown) {
				_goToStart = false;
				_climbStair = true;
			}
		} else if(_climbStair){
			_guard.transform.Translate ((Vector3.right + Vector3.up) * 2.0f * Time.deltaTime);
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if(_guardState == guardState.Act2Begin && other.name == "LeaveUnlocked"){
			_guardState = guardState.LeftUnlocked;
			Events.G.Raise (new LeftCellUnlockedEvent());
			_guard.SetActive (false);
		}

		if (other.tag == "Stairs") {
			_isStairs = true;
			_highlightsFX.objectRenderer = _stairRenderer;
			_highlightsFX.enabled = true;
		} else if (other.name == "LockCell") {
			_highlightsFX.objectRenderer = _doorRenderer;
			_highlightsFX.enabled = true;
		}
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
		if (other.name == "Bomb") {
			_waveCnt = 0;
		} else if (other.tag == "Stairs") {
			_isStairs = false;
			_highlightsFX.enabled = false;
		} else if (other.name == "LockCell") {
			_highlightsFX.enabled = false;
		}
	}
}

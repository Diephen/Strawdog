using UnityEngine;
using System.Collections;

public class Cell_GuardTrigger : MonoBehaviour {
	[SerializeField] bool _isGuardTop = false;
	[SerializeField] GameObject _guard;
	PuppetControl _guardPuppetController;
	KeyCode[] _guardKeyCodes;

	bool _isStairs = false;
	bool _isDoor = false;

	[SerializeField] float _stairStartPosition = 28f;
	bool _goToStart = false;
	Timer _stairStartTimer;
	bool _climbStair = false;

	Vector3 _tempPosition;


	int _waveCnt = 0;

	bool _locked = true;


	Camera _mainCam;
	HighlightsFX _highlightsFX;

	[SerializeField] Renderer _stairRenderer;
	[SerializeField] Renderer _doorRenderer;
	[SerializeField] Renderer _bombRenderer;
	[SerializeField] Renderer _secretDoorRenderer;

	[SerializeField] GameObject _bomb;
	Bomb _bombScript;

	[SerializeField] BoxCollider2D _groundCollider1;
	[SerializeField] BoxCollider2D _groundCollider2;

	void Start(){
		_guardPuppetController = _guard.GetComponent <PuppetControl> ();
		_guardKeyCodes = _guardPuppetController.GetKeyCodes ();
		_stairStartTimer = new Timer (1f);
		_mainCam = Camera.main;
		_highlightsFX = _mainCam.GetComponent <HighlightsFX> ();
		if (_isGuardTop) {
			_bomb = GameObject.Find ("Bomb");
			_bombScript = _bomb.GetComponent<Bomb> ();
			_bombRenderer = _bomb.GetComponent<Renderer> ();
		}
	}

	void Update(){
		if (Input.GetKeyDown (_guardKeyCodes [3])) {
			if (_isStairs) {
				_goToStart = true;
				_highlightsFX.enabled = false;
				_guardPuppetController.DisableKeyInput ();
				_tempPosition = _guard.transform.position;
				_stairStartTimer.Reset ();
			} 
			else if (_isDoor) 
			{
				_locked = !_locked;
				Events.G.Raise(new LockCellEvent(_locked));
			}
		}

		if (_goToStart) {
			_tempPosition.x = Mathf.Lerp (_tempPosition.x, _stairStartPosition, _stairStartTimer.PercentTimePassed);
			_guard.transform.position = _tempPosition;
			if (_stairStartTimer.IsOffCooldown) {
				_goToStart = false;
				_climbStair = true;
			}
		}
		else if(_climbStair && !_isGuardTop)
		{
			_guard.transform.Translate ((Vector3.right + Vector3.up) * 2.0f * Time.deltaTime);
			Events.G.Raise (new Act2_GuardWalkedUpStairsEvent ());
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (!_locked) {
			if (other.name == "LeaveUnlocked") {
				Events.G.Raise (new LeftCellUnlockedEvent ());
				_guard.SetActive (false);
			}
		}

		if (other.tag == "Stairs") {
			_isStairs = true;
			_highlightsFX.objectRenderer = _stairRenderer;
			_highlightsFX.enabled = true;
		} else if (other.name == "LockCell") {
			_highlightsFX.objectRenderer = _doorRenderer;
			_highlightsFX.enabled = true;
			_isDoor = true;
		} else if (other.name == "Bomb") {
			_highlightsFX.objectRenderer = _bombRenderer;
			_highlightsFX.enabled = true;
		}
	}

	void OnTriggerStay2D(Collider2D other){
		if (other.name == "Bomb") {
			if (Input.GetKeyDown (_guardKeyCodes [3])) {
				_highlightsFX.enabled = false;
				_bomb.SetActive (false);
				Events.G.Raise (new GuardFoundBombEvent ());
			}
		}
		if (other.name == "BombArea") {
			if(Input.GetKeyDown (_guardKeyCodes[2])){
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
			_highlightsFX.enabled = false;
		} else if (other.name == "LockCell") {
			_highlightsFX.enabled = false;
			_isDoor = false;
		} else if (other.name == "Bomb") {
			_highlightsFX.enabled = false;
		}
	}
}

using UnityEngine;
using System.Collections;

public class Cell_PrisonerTrigger : MonoBehaviour {
	enum prisonerState {Act2Begin};
	prisonerState _prisonerState = prisonerState.Act2Begin;

	[SerializeField] GameObject _prisoner;
	PuppetControl _prisonerPuppetController;
	[SerializeField] MinMax _stairRange = new MinMax(0.2f, 9.8f); 
	KeyCode[] _prisonerKeyCodes;

	[SerializeField] Renderer _stairRenderer;
	[SerializeField] Renderer _bedRenderer;

	int _waveCnt = 0;

	bool _isStairs = false;
	float _stairStartPosition = 28.6f;


	bool _goToStart = false;
	Timer _stairStartTimer;
	bool _climbStair = false;

	Vector3 _tempPosition;

	Camera _mainCam;
	HighlightsFX _highlightsFX;

	void Start(){
		_prisonerPuppetController = _prisoner.GetComponent <PuppetControl> ();
		_prisonerKeyCodes = _prisonerPuppetController.GetKeyCodes ();
		_stairStartTimer = new Timer (1f);
		_mainCam = Camera.main;
		_highlightsFX = _mainCam.GetComponent <HighlightsFX> ();
	}

	void Update(){
		if (_waveCnt >= 3) {
			_waveCnt = 0;
			Events.G.Raise(new PrisonerFoundBombEvent());
			Debug.Log ("Prisoner Found Bomb");
		}

		if (Input.GetKeyDown (_prisonerKeyCodes [3]) && _isStairs) {
			_goToStart = true;
			_highlightsFX.enabled = false;
			_prisonerPuppetController.DisableKeyInput ();
			_tempPosition = _prisoner.transform.position;
			_stairStartTimer.Reset ();
		}

		if (_goToStart) {
			_tempPosition.x = Mathf.Lerp (_tempPosition.x, _stairStartPosition, _stairStartTimer.PercentTimePassed);
			_prisoner.transform.position = _tempPosition;
			if (_stairStartTimer.IsOffCooldown) {
				_goToStart = false;
				_climbStair = true;
			}
		} else if(_climbStair){
			_prisoner.transform.Translate ((Vector3.right + Vector3.up) * 2.0f * Time.deltaTime);
		}
	}

	void OnTriggerEnter2D(Collider2D other) {

		if (other.tag == "Stairs") {
			_isStairs = true;
			_highlightsFX.objectRenderer = _stairRenderer;
			_highlightsFX.enabled = true;
		} else if (other.name == "Bed") {
			_highlightsFX.objectRenderer = _bedRenderer;
			_highlightsFX.enabled = true;
		}
	}

	void OnTriggerStay2D(Collider2D other){
		if (other.name == "Bed") {
			if(Input.GetKeyDown (_prisonerKeyCodes[3])){
				Debug.Log ("Prisoner going to bed");
				Events.G.Raise(new SleepInCellEvent());
			}
		}
		if (other.name == "Bomb") {
			if(Input.GetKeyDown (_prisonerKeyCodes[2])){
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
		} else if (other.name == "Bed") {
			_highlightsFX.enabled = false;
		}
	}
}

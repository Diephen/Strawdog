using UnityEngine;
using System.Collections;

public class GuardTrigger : MonoBehaviour {
	enum guardState {BeforeEntering, EnteredCell, EngagedPrisoner};
	guardState _guardState = guardState.BeforeEntering;

	[SerializeField] GameObject _guard;
	PuppetControl _guardPuppetController;
	KeyCode[] _guardKeyCodes;

	[SerializeField] Renderer _stairRenderer;

	//Stairs
	bool _isStairs = false;
	float _stairStartPosition = 116.6f;
	bool _goToStart = false;
	Timer _stairStartTimer;
	bool _climbStair = false;
	Vector3 _tempPosition;
	[SerializeField] BoxCollider2D _groundCollider1;
	[SerializeField] BoxCollider2D _groundCollider2;

	//Variables for Highlight
	Camera _mainCam;
	HighlightsFX _highlightsFX;

	void Start(){
		_guardPuppetController = _guard.GetComponent <PuppetControl>();
		_guardKeyCodes = _guardPuppetController.GetKeyCodes ();

		_stairStartTimer = new Timer (2f);

		//For The glow Effect
		_mainCam = Camera.main;
		_highlightsFX = _mainCam.GetComponent <HighlightsFX> ();
	}

	void Update(){
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
				Vector3 flipScale = _guard.transform.localScale;
				flipScale.x = flipScale.x * -1.0f;
				_guard.transform.localScale = flipScale;

				Events.G.Raise (new Act0EndedEvent ());
			}
		} else if(_climbStair){
			_groundCollider1.enabled = false;
			_groundCollider2.enabled = false;
			_guard.transform.Translate ((Vector3.left + Vector3.down) * 2.0f * Time.deltaTime);
		}
	}


	void OnTriggerEnter2D(Collider2D other) {
		if (_guardState == guardState.BeforeEntering && other.name == "EnterCell") 
		{
			_guardState = guardState.EnteredCell;
			Events.G.Raise(new GuardEnteringCellEvent());
		} 
		else if(_guardState == guardState.EnteredCell && other.name == "EnterCell")
		{
			Events.G.Raise (new GuardLeavingCellEvent());
		}

		if (other.name == "EngagePrisoner") 
		{
			_guardState = guardState.EngagedPrisoner;
			Events.G.Raise(new GuardEngaginPrisonerEvent(true));
		} 
		else if (other.tag == "Stairs") 
		{
			_isStairs = true;
			_highlightsFX.objectRenderer = _stairRenderer;
			_highlightsFX.enabled = true;
		}
	}


	void OnTriggerExit2D(Collider2D other) 
	{
		if (other.name == "EngagePrisoner") 
		{
			_guardState = guardState.EnteredCell;
			Events.G.Raise(new GuardEngaginPrisonerEvent(false));
		} 
		else if (other.tag == "Stairs") 
		{
			_isStairs = false;
			_highlightsFX.enabled = false;
		}
	}
}		
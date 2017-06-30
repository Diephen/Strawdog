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

	bool _isOnFlap = false;
	[SerializeField] BoxCollider2D _groundCollider1;
	[SerializeField] BoxCollider2D _groundCollider2;
	[SerializeField] SpriteRenderer _leftFlap;
	[SerializeField] SpriteRenderer _rightFlap;

	bool _guardLeaveCell = false;
	bool _isDoor = false;
	bool _isOfficeOpened = false;
	//Variables for Highlight
	Camera _mainCam;
	HighlightsFX _highlightsFX;

	void Start(){
		_guardPuppetController = _guard.GetComponent <PuppetControl>();
		_guardKeyCodes = _guardPuppetController.GetKeyCodes ();

		_stairStartTimer = new Timer (1f);

		//For The glow Effect
		_mainCam = Camera.main;
		//_highlightsFX = _mainCam.GetComponent <HighlightsFX> ();
	}

	void Update(){
		if (Input.GetKeyDown (_guardKeyCodes [3])) {
			if(_isStairs){
				_goToStart = true;
				_isStairs = false;
				//_highlightsFX.enabled = false;
				_stairRenderer.gameObject.GetComponentInChildren<HighlightSprite> ().DisableHighlight();
				_guardPuppetController.DisableKeyInput ();
				_tempPosition = _guard.transform.position;
				_stairStartTimer.Reset ();
				Events.G.Raise (new GuardStairsStartEvent ());
			}
			else if (_isDoor) {
				print ("Guard open office door");
				_isOfficeOpened = !_isOfficeOpened;
				Events.G.Raise (new OfficeDoorEvent (_isOfficeOpened));

			}
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
		if (_isOnFlap && Input.GetKeyDown (_guardKeyCodes [3])) {
			Events.G.Raise (new GuardLeavingCellEvent ());
			_rightFlap.GetComponentInChildren<HighlightSprite> ().DisableHighlight();
			_leftFlap.GetComponentInChildren<HighlightSprite> ().DisableHighlight();
			Events.G.Raise (new EnableMoveEvent (CharacterIdentity.Guard));
			_guardLeaveCell = true;
		}

		if (_guardLeaveCell) {
			_guard.transform.Translate (Vector3.right* Time.deltaTime * 2.0f);
		}
	}


	void OnTriggerEnter2D(Collider2D other) {
		if (_guardState == guardState.BeforeEntering && other.name == "EnterCell") 
		{
			_guardState = guardState.EnteredCell;
			Events.G.Raise(new GuardEnteringCellEvent());
		} 
		if (other.name == "EngagePrisoner") {
			_guardState = guardState.EngagedPrisoner;
			Events.G.Raise (new GuardEngaginPrisonerEvent (true));
		} else if (other.tag == "Stairs") {
			_isStairs = true;
			other.GetComponent<HighlightSprite> ().EnableHighlight ();
			//_highlightsFX.objectRenderer = _stairRenderer;
			//_highlightsFX.enabled = true;
		} else if (other.name == "Office") {
			other.GetComponentInChildren<HighlightSprite> ().EnableHighlight ();

			_isDoor = true;
		} else if (other.name == "open-right") {
			//_highlightsFX.objectRenderer = _rightFlap;
			//_highlightsFX.enabled = true;
			other.GetComponent<HighlightSprite> ().EnableHighlight ();
			_isOnFlap = true;
		} else if (other.name == "open-left") {
			//_highlightsFX.objectRenderer = _leftFlap;
			//_highlightsFX.enabled = true;
			other.GetComponent<HighlightSprite> ().EnableHighlight ();
			_isOnFlap = true;
		} else if (other.name == "EnterInteriorTrigger") {
			// enter the interior 
			Events.G.Raise(new WallTransitionEvent(true));
		}else if (other.name == "ExitInteriorTrigger") {
			// enter the interior 
			Events.G.Raise(new WallTransitionEvent(false));
		}else if (other.name == "DoorEnterTrigger") {
			// enter the interior 
			Events.G.Raise(new DoorTransitionEvent(false));
		}
			
	}


	void OnTriggerExit2D(Collider2D other) 
	{
		if (other.name == "EngagePrisoner") 
		{
			_guardState = guardState.EnteredCell;
			Events.G.Raise(new GuardEngaginPrisonerEvent(false));
		} 
		else if (other.name == "Office") {
			other.GetComponentInChildren<HighlightSprite> ().DisableHighlight();
			if (_isOnFlap) {
				_rightFlap.gameObject.GetComponentInChildren<HighlightSprite> ().EnableHighlight();
			}
			else {
				other.GetComponentInChildren<HighlightSprite> ().DisableHighlight();

			}
			_isDoor = false;
		}
		else if (other.tag == "Stairs") 
		{
			_isStairs = false;
			//_highlightsFX.enabled = false;
			other.GetComponentInChildren<HighlightSprite> ().DisableHighlight();
		} else if (other.name == "open-right") {
			//_highlightsFX.enabled = false;
			//other.GetComponentInChildren<HighlightSprite> ().DisableHighlight();
			other.GetComponentInChildren<HighlightSprite> ().DisableHighlight();
			_isOnFlap = false;
		}
		else if (other.name == "open-left") {
			//_highlightsFX.enabled = false;
			other.GetComponentInChildren<HighlightSprite> ().DisableHighlight();
			_isOnFlap = false;
		}else if (other.name == "DoorEnterTrigger") {
			// enter the interior 
			Events.G.Raise(new DoorTransitionEvent(true));
		}
	}
}		
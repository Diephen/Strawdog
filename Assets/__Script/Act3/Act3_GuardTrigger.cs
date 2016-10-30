using UnityEngine;
using System.Collections;

public class Act3_GuardTrigger : MonoBehaviour {
	enum guardState {Act3Begin};
	guardState _guardState = guardState.Act3Begin;

	[SerializeField] GameObject _guard;
	PuppetControl _guardPuppetController;
//	[SerializeField] MinMax _stairRange = new MinMax(0.2f, 9.8f); 
	KeyCode[] _guardKeyCodes;

//	[SerializeField] Renderer _stairRenderer;

	[SerializeField] AnimationCurve _screenTranslucencyCurve;
	[SerializeField] AnimationCurve _fullTranslucencyCurve;
	Color _tempColor;
	float _tempAlpha;
	[SerializeField] bool _isGuardTop = false;

	[SerializeField] TextMesh _code;
	[SerializeField] Renderer _foodStorageWall;
	bool _codeOnce = true;
//	bool _isStairs = false;
//	float _stairTempPosition;

	[SerializeField] float _stairStartPosition = 28f;
	bool _goToStart = false;
	Timer _stairStartTimer;
	bool _climbStair = false;
	Vector3 _tempPosition;
	bool _isStairs = false;
//	int _waveCnt = 0;
	SpriteRenderer _screenRenderer;
	bool _bombPlanted = false;
	bool _secretDoor = false;
	bool _foodStorage = false;

	void Start(){
		_stairStartTimer = new Timer (1f);
		_guardPuppetController = _guard.GetComponent<PuppetControl> ();
		_guardKeyCodes = _guardPuppetController.GetKeyCodes ();

	}

	void Update(){
		if (Input.GetKeyDown (_guardKeyCodes [3])) {
			if (_isStairs) {
				_goToStart = true;
				//_highlightsFX.enabled = false;
				_isStairs = false;
//				_stairRenderer.gameObject.GetComponentInChildren<HighlightSprite> ().DisableHighlight ();
				_guardPuppetController.DisableKeyInput ();
				_tempPosition = _guard.transform.position;
				_stairStartTimer.Reset ();
				Events.G.Raise (new GuardStairsStartEvent ());
			}
		}
//		if (_isStairs) {
////			Debug.Log (_guard.transform.position.y);
//			if (((_guard.transform.position.y < _stairRange.Min) && (_stairTempPosition>_stairRange.Min)) 
//				|| ((_stairTempPosition < _stairRange.Max) &&(_guard.transform.position.y > _stairRange.Max))) {
//				_isStairs = false;
//
//				_guardPuppetController.SetIsStairs (_isStairs);
//			}
//		}
//
//		if (_waveCnt >= 3) {
//			_waveCnt = 0;
//			Events.G.Raise(new GuardFoundBombEvent());
//			Debug.Log ("Guard Found Bomb");
//		}

		if (_goToStart) {
			_tempPosition.x = Mathf.Lerp (_tempPosition.x, _stairStartPosition, _stairStartTimer.PercentTimePassed);
			_guard.transform.position = _tempPosition;
			if (_stairStartTimer.IsOffCooldown) {
				SoldierFlip ();
				_goToStart = false;
				_climbStair = true;
			}
		}
		else if (_climbStair && !_isGuardTop) {
			_guard.transform.Translate ((Vector3.left + Vector3.up) * 2.0f * Time.deltaTime);
			Events.G.Raise (new Act2_GuardWalkedUpStairsEvent ());
		}
		else if (_climbStair && _isGuardTop) {
			_guard.transform.Translate ((Vector3.left + Vector3.down) * 2.0f * Time.deltaTime);
//			Events.G.Raise (new Act2_GuardWalkedUpStairsEvent ());
		}
		if (_isGuardTop) {
			if (_secretDoor && Input.GetKeyDown (_guardKeyCodes [3])) {
				Events.G.Raise (new CallSecretDoorEvent ());
				//_highlightsFX.enabled = false;
			} else if(_foodStorage && Input.GetKeyDown (_guardKeyCodes [3])){
				Events.G.Raise (new Plant_EnterFoodStorageEvent());
			}
		}

	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.tag == "Stairs") {
			_isStairs = true;
			other.GetComponentInChildren<HighlightSprite> ().EnableHighlight();
			//_highlightsFX.objectRenderer = _stairRenderer;
			//_highlightsFX.enabled = true;
		} else if (other.tag == "SecretDoor") {
			other.GetComponentInChildren<HighlightSprite> ().EnableHighlight();
			_secretDoor = true;
		}

		if (other.name == "Glass") {
			_screenRenderer = other.gameObject.GetComponent <SpriteRenderer> ();
			_tempColor = _screenRenderer.color;
			StartCoroutine (ScreenFadeOut ());


//			_guardState = guardState.LeftUnlocked;
//			Events.G.Raise (new LeftCellUnlockedEvent());
//			_guard.SetActive (false);
		}else if (other.name == "EnterFoodStorage") {
			_foodStorage = true;
			other.GetComponentInChildren<HighlightSprite> ().EnableHighlight();
			Debug.Log ("Found");
//			Events.G.Raise (new Plant_EnterFoodStorageEvent());
		} 
		else if (other.name == "LeaveFoodStorage" && _bombPlanted) {
			_tempColor = _foodStorageWall.material.color;
			StartCoroutine (WallFadeOut (_foodStorageWall));
		} else if (other.name == "Encounter") {
			Events.G.Raise (new Guard_EncounterEvent());
		} 
//
//		if (other.tag == "Stairs") {
//			Debug.Log ("But Does this Fire?");
//			_isStairs = true;
//			_stairTempPosition = _guard.transform.position.y;
//			_guardPuppetController.SetIsStairs (_isStairs);
//		}
		//		if (other.name == "EngagePrisoner") {
		//			_guardState = guardState.EngagedPrisoner;
		//			Events.G.Raise(new GuardEngaginPrisonerEvent(true));
		//		}
	}
	IEnumerator ScreenFadeOut(){
		float startTime = Time.time;
		bool i = true;
		while (i) {
			_tempAlpha = _screenTranslucencyCurve.Evaluate ((Time.time - startTime));
			_tempColor.a = _tempAlpha;
			_screenRenderer.color = _tempColor;
//			Debug.Log (_screenTimer.PercentTimePassed);
			yield return null;
			if ((Time.time - startTime) > 1.0f) {
				i = false;
			}
		}
	}

	IEnumerator ScreenFadeIn(){
		float startTime = Time.time;
		bool i = true;
		while (i) {
			_tempAlpha = _screenTranslucencyCurve.Evaluate (1.0f - (Time.time - startTime));
			_tempColor.a = _tempAlpha;
			_screenRenderer.color = _tempColor;
			//			Debug.Log (_screenTimer.PercentTimePassed);
			yield return null;
			if ((Time.time - startTime) > 1.0f) {
				i = false;
			}
		}
	}

	IEnumerator CodeFadeIn(){
		float startTime = Time.time;
		bool i = true;
		while (i) {
			_tempAlpha = _screenTranslucencyCurve.Evaluate (1.0f - (Time.time - startTime)/3f);
			_tempColor.a = _tempAlpha;
			_code.color = _tempColor;
			//			Debug.Log (_screenTimer.PercentTimePassed);
			yield return null;
			if ((Time.time - startTime)/3f > 1.0f) {
				i = false;
			}
		}
	}

	IEnumerator WallFadeIn(Renderer render){
		float startTime = Time.time;
		bool i = true;
		while (i) {
			_tempAlpha = _fullTranslucencyCurve.Evaluate ((Time.time - startTime)/2f);
			_tempColor.a = _tempAlpha;
			render.material.color = _tempColor;
			//			Debug.Log (_screenTimer.PercentTimePassed);
			yield return null;
			if ((Time.time - startTime)/2f > 1.0f) {
				i = false;
			}
		}
	}

	IEnumerator WallFadeOut(Renderer render){
		float startTime = Time.time;
		bool i = true;
		while (i) {
			_tempAlpha = _fullTranslucencyCurve.Evaluate (1.0f - (Time.time - startTime)/3f);
			_tempColor.a = _tempAlpha;
			render.material.color = _tempColor;
			//			Debug.Log (_screenTimer.PercentTimePassed);
			yield return null;
			if ((Time.time - startTime)/3f > 1.0f) {
				i = false;
			}
		}
	}

	void OnTriggerStay2D(Collider2D other){
//		if (_guardState == guardState.Act2Begin && other.name == "LockCell") {
//			if(Input.GetKeyDown (_guardKeyCodes[3])){
//				_guardState = guardState.Locked;
//				Debug.Log ("Guard Locked the door");
//				Events.G.Raise(new LockCellEvent());
//			}
//		}
		if (other.name == "BombPlant") {
			if (Input.GetKeyDown (_guardKeyCodes [3])) {
				Debug.Log ("Planted Bomb");
				_bombPlanted = true;
				if (_codeOnce) {
					_tempColor = _code.color;
					StartCoroutine (CodeFadeIn ());
					_codeOnce = false;
				}
			}
		}
	}


	void OnTriggerExit2D(Collider2D other) {
		if (other.name == "Glass") {
			_screenRenderer = other.gameObject.GetComponent <SpriteRenderer> ();
			_tempColor = _screenRenderer.color;
			StartCoroutine (ScreenFadeIn ());


			//			_guardState = guardState.LeftUnlocked;
			//			Events.G.Raise (new LeftCellUnlockedEvent());
			//			_guard.SetActive (false);
		}
		else if (other.tag == "Stairs") {
			_isStairs = false;
			//_highlightsFX.enabled = false;
			other.GetComponentInChildren<HighlightSprite> ().DisableHighlight ();
		}
		else if (other.tag == "SecretDoor") {
			other.GetComponentInChildren<HighlightSprite> ().DisableHighlight ();
			_secretDoor = false;
		}
		else if (other.name == "EnterFoodStorage") {
			_foodStorage = false;
			other.GetComponentInChildren<HighlightSprite> ().DisableHighlight();
		}

//		if (other.name == "Bomb") {
//			_waveCnt = 0;
//		}
	}

	void SoldierFlip(){
		Vector3 _temp = _guard.transform.localPosition;
		_temp.x += 2.1f;
		_guard.transform.localPosition = _temp;
		_temp = _guard.transform.localScale;
		_temp.x = _temp.x * -1.0f;
		_guard.transform.localScale = _temp;

	}

	void OnEnable()
	{
		Events.G.AddListener<Prisoner_EncounterEvent>(PrisonerEncounter);
	}

	void OnDisable ()
	{
		Events.G.RemoveListener<Prisoner_EncounterEvent>(PrisonerEncounter);
	}

	void PrisonerEncounter(Prisoner_EncounterEvent e) {
		_guardPuppetController.EnableKeyInput ();
	}
}

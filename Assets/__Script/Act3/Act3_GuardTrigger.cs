using UnityEngine;
using System.Collections;

public class Act3_GuardTrigger : MonoBehaviour {
	enum guardState {Act3Begin};
	guardState _guardState = guardState.Act3Begin;

	[SerializeField] GameObject _guard;
	[SerializeField] PuppetControl _guardPuppetController;
//	[SerializeField] MinMax _stairRange = new MinMax(0.2f, 9.8f); 
	KeyCode[] _guardKeyCodes;

	[SerializeField] AnimationCurve _screenTranslucencyCurve;
	[SerializeField] AnimationCurve _fullTranslucencyCurve;
	Color _tempColor;
	float _tempAlpha;

	[SerializeField] TextMesh _code;
	[SerializeField] Renderer _foodStorageWall;
	bool _codeOnce = true;
//	bool _isStairs = false;
//	float _stairTempPosition;

//	int _waveCnt = 0;
	Renderer _screenRenderer;
	bool _bombPlanted = false;

	void Start(){
		_guardKeyCodes = _guardPuppetController.GetKeyCodes ();

	}

	void Update(){
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


	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.name == "Glass") {
			_screenRenderer = other.gameObject.GetComponent <Renderer> ();
			_tempColor = _screenRenderer.material.color;
			StartCoroutine (ScreenFadeOut ());


//			_guardState = guardState.LeftUnlocked;
//			Events.G.Raise (new LeftCellUnlockedEvent());
//			_guard.SetActive (false);
		}else if (other.name == "EnterFoodStorage") {
			Events.G.Raise (new Plant_EnterFoodStorageEvent());
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
			_tempAlpha = _screenTranslucencyCurve.Evaluate ((Time.time - startTime)*2f);
			_tempColor.a = _tempAlpha;
			_screenRenderer.material.color = _tempColor;
//			Debug.Log (_screenTimer.PercentTimePassed);
			yield return null;
			if ((Time.time - startTime)*2f > 1.0f) {
				i = false;
			}
		}
	}

	IEnumerator ScreenFadeIn(){
		float startTime = Time.time;
		bool i = true;
		while (i) {
			_tempAlpha = _screenTranslucencyCurve.Evaluate (1.0f - (Time.time - startTime)*2f);
			_tempColor.a = _tempAlpha;
			_screenRenderer.material.color = _tempColor;
			//			Debug.Log (_screenTimer.PercentTimePassed);
			yield return null;
			if ((Time.time - startTime)*2f > 1.0f) {
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
			_screenRenderer = other.gameObject.GetComponent <Renderer> ();
			_tempColor = _screenRenderer.material.color;
			StartCoroutine (ScreenFadeIn());


			//			_guardState = guardState.LeftUnlocked;
			//			Events.G.Raise (new LeftCellUnlockedEvent());
			//			_guard.SetActive (false);
		}
//		if (other.tag == "Stairs") {
//			_isStairs = false;
//			_guardPuppetController.SetIsStairs (_isStairs);
//		}
//		if (other.name == "Bomb") {
//			_waveCnt = 0;
//		}
	}
}

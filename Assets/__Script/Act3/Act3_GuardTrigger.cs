using UnityEngine;
using System.Collections;

public class Act3_GuardTrigger : MonoBehaviour {
	enum guardState {Act3Begin};
	guardState _guardState = guardState.Act3Begin;

	[SerializeField] GameObject _guard;
	[SerializeField] GameObject _heldBomb;
	PuppetControl _guardPuppetController;
//	[SerializeField] MinMax _stairRange = new MinMax(0.2f, 9.8f); 
	KeyCode[] _guardKeyCodes;

//	[SerializeField] Renderer _stairRenderer;

	[SerializeField] AnimationCurve _screenTranslucencyCurve;
	[SerializeField] AnimationCurve _fullTranslucencyCurve;
	Color _tempColor;
	float _tempAlpha;
	[SerializeField] bool _isGuardTop = false;

	[SerializeField] Renderer _foodStorageWall;
	AudioController _audioController;
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
	Timer _bombTimer = new Timer (1f);
	bool _bombArea = false;
	bool _secretDoor = false;
	bool _foodStorage = false;
	bool _crawlExit = false;
	Vector3 _bombPlantPosition = new Vector3(-3.06f, -2.886f, 0.0f);
	GameObject _bombObject;
	[SerializeField] BoxCollider2D _groundCollider1;
	[SerializeField] BoxCollider2D _groundCollider2;

	[SerializeField] AudioClip _bombTick;
	[SerializeField] AudioClip _bombPlace;

	void Start(){
		_stairStartTimer = new Timer (1f);
		_guardPuppetController = _guard.GetComponent<PuppetControl> ();
		_guardKeyCodes = _guardPuppetController.GetKeyCodes ();
		_audioController = GameObject.FindObjectOfType<AudioController> ();

	}

	void Update(){
		if (Input.GetKeyDown (_guardKeyCodes [3])) {
			if (_isStairs) {
				_goToStart = true;
				_isStairs = false;
//				_stairRenderer.gameObject.GetComponentInChildren<HighlightSprite> ().DisableHighlight ();
				_guardPuppetController.DisableKeyInput ();
				_tempPosition = _guard.transform.position;
				_stairStartTimer.Reset ();
				Events.G.Raise (new GuardStairsStartEvent ());
			}
		}

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
			Events.G.Raise (new Plant_UpStairsEvent ());
		}
		else if (_climbStair && _isGuardTop) {
			_groundCollider1.enabled = false;
			_groundCollider2.enabled = false;
			_guard.transform.Translate ((Vector3.left + Vector3.down) * 2.0f * Time.deltaTime);
			Events.G.Raise (new Plant_DownStairsEvent ());		
		}
		if (_isGuardTop) {
			if (_secretDoor && Input.GetKeyDown (_guardKeyCodes [3])) {
				Events.G.Raise (new CallSecretDoorEvent ());
			}
			else if (_foodStorage && Input.GetKeyDown (_guardKeyCodes [3])) {
				Events.G.Raise (new Plant_EnterFoodStorageEvent ());
			}
			else if (_bombArea && !_bombPlanted && Input.GetKeyDown (_guardKeyCodes [3])) {
				_bombObject.GetComponentInChildren<HighlightSprite> ().DisableHighlight ();
				_heldBomb.transform.SetParent (_bombObject.transform);
				_bombPlantPosition.z = _heldBomb.transform.position.z;
				_bombPlanted = true;
				_heldBomb.transform.rotation = Quaternion.identity;
				_bombTimer.Reset ();
				_audioController.SingleSound (_bombPlace);
				_audioController.LoopSound (_bombTick);
			}
			else if (_crawlExit && _bombPlanted && Input.GetKeyDown (_guardKeyCodes [3])) {
				Events.G.Raise (new Plant_LeaveFoodStorageEvent ());
				_crawlExit = false;
			}
		}
	}

	void FixedUpdate(){
		if (_bombPlanted && !_bombTimer.IsOffCooldown) {
			_heldBomb.transform.localPosition = Vector3.Lerp (_heldBomb.transform.localPosition, _bombPlantPosition, _bombTimer.PercentTimePassed);
		}
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.tag == "Stairs") {
			_isStairs = true;
			other.GetComponentInChildren<HighlightSprite> ().EnableHighlight();
		} else if (other.tag == "SecretDoor") {
			other.GetComponentInChildren<HighlightSprite> ().EnableHighlight();
			_secretDoor = true;
		}

		if (other.name == "Glass") {
			_screenRenderer = other.gameObject.GetComponent <SpriteRenderer> ();
			_tempColor = _screenRenderer.color;
			StartCoroutine (ScreenFadeOut ());


		}
		else if (other.name == "EnterFoodStorage") {
			_foodStorage = true;
			other.GetComponentInChildren<HighlightSprite> ().EnableHighlight ();
		}
		else if (other.name == "LeaveFoodStorage" && _bombPlanted) {
			_tempColor = _foodStorageWall.material.color;
			StartCoroutine (WallFadeOut (_foodStorageWall));
		}
		else if (other.name == "Encounter") {
			_guardPuppetController.DisableContinuousWalk ();
			Events.G.Raise (new Guard_EncounterEvent ());
		}
		else if (other.name == "GuardExitEnd") {
			Events.G.Raise (new GuardAloneEndingEvent ());
		}
		else if (other.name == "BombPlant" && !_bombPlanted) {
			_bombArea = true;
			other.GetComponentInChildren<HighlightSprite> ().EnableHighlight();
			_bombObject = other.gameObject;
		}
		else if (other.name == "CrawlExit" && _bombPlanted) {
			_crawlExit = true;
			other.GetComponentInChildren<HighlightSprite> ().EnableHighlight ();
		}
	}
	IEnumerator ScreenFadeOut(){
		float startTime = Time.time;
		bool i = true;
		while (i) {
			_tempAlpha = _screenTranslucencyCurve.Evaluate ((Time.time - startTime));
			_tempColor.a = _tempAlpha;
			_screenRenderer.color = _tempColor;
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
			yield return null;
			if ((Time.time - startTime) > 1.0f) {
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


	void OnTriggerExit2D(Collider2D other) {
		if (other.name == "Glass") {
			_screenRenderer = other.gameObject.GetComponent <SpriteRenderer> ();
			_tempColor = _screenRenderer.color;
			StartCoroutine (ScreenFadeIn ());
		}
		else if (other.tag == "Stairs") {
			_isStairs = false;
			other.GetComponentInChildren<HighlightSprite> ().DisableHighlight ();
		}
		else if (other.tag == "SecretDoor") {
			other.GetComponentInChildren<HighlightSprite> ().DisableHighlight ();
			_secretDoor = false;
		}
		else if (other.name == "EnterFoodStorage") {
			_foodStorage = false;
			other.GetComponentInChildren<HighlightSprite> ().DisableHighlight ();
		}
		else if (other.name == "BombPlant" && !_bombPlanted) {
			_bombArea = false;
			other.GetComponentInChildren<HighlightSprite> ().DisableHighlight ();
		}
		else if (other.name == "CrawlExit" && _bombPlanted) {
			_crawlExit = false;
			other.GetComponentInChildren<HighlightSprite> ().DisableHighlight ();
		}
	}

	void SoldierFlip(){
		Vector3 _temp = _guard.transform.localPosition;
		if (_guard.transform.localScale.x >= 0) {
			_temp.x += 2.1f;
		}
		else {
			_temp.x -= 2.1f;
		}
		_guard.transform.localPosition = _temp;
		_temp = _guard.transform.localScale;
		_temp.x = _temp.x * -1.0f;
		_guard.transform.localScale = _temp;
	}

	void StopSecretDoor(StopSecretExitEvent e){
		_secretDoor = !e.Stopped;
	}

	void OnEnable()
	{
		Events.G.AddListener<Prisoner_EncounterEvent>(PrisonerEncounter);
		Events.G.AddListener<StopSecretExitEvent>(StopSecretDoor);
	}

	void OnDisable ()
	{
		Events.G.RemoveListener<Prisoner_EncounterEvent>(PrisonerEncounter);
		Events.G.RemoveListener<StopSecretExitEvent>(StopSecretDoor);

	}

	void PrisonerEncounter(Prisoner_EncounterEvent e) {
		_guardPuppetController.EnableKeyInput ();
	}
}

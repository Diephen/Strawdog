using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class TitleCardManager : MonoBehaviour {
	[SerializeField] SpriteRenderer _left;
	[SerializeField] SpriteRenderer _right;
	[SerializeField] AudioClip[] _titleCardClips;
	[SerializeField] Color _fadeToColor;
	AudioSource _audioSource;
	Color _originalColor;
	Timer _blackTimer = new Timer(1f);
	Timer _titleTextTimer = new Timer(2f);
	Timer _bodyTextTimer = new Timer(2f);
	bool _once = true;
	bool _isText = false;
	bool _isPressed = true;
	// 0: fade in
	// 1: swap out
	// 2: sawp in
	// 3: fade out
	int _fadeStates = 0;

	//Act Title Index
	//0 - Act 1
	//1 - Act 2
	//2 - Act 3-1
	//3 - Act 3-2
	//4 - Act 3-3
	//5 - Act 4-1
	//6 - Act 4-2
	//7 - Act 5
		
	float _tempAlpha;
	Color _tempTitleColor;
	Color _tempBodyColor;

	[SerializeField] Text[] _titleText = new Text[8];
	[SerializeField] Text[] _bodyText = new Text[8];

	[SerializeField] float[] _firstDuration = new float[8];
	[SerializeField] float[] _secondDuration = new float[8];
	Timer _durationTimer;

	void Awake(){
		_originalColor = _left.color;
		if(GameStateManager.gameStateManager._actTitleIndex == 0){
			_left.color = Color.black;
			_right.color = Color.black;
		}
	}

	// Use this for initialization
	void Start () {

		
		_tempTitleColor = _titleText[GameStateManager.gameStateManager._actTitleIndex].color;
		_tempBodyColor = _titleText[GameStateManager.gameStateManager._actTitleIndex].color;

		_audioSource = gameObject.GetComponent<AudioSource> ();
		_blackTimer.Reset ();

		_durationTimer = new Timer (_firstDuration[GameStateManager.gameStateManager._actTitleIndex]);

	}

	void Update(){
		if (_isText && (Input.GetKeyDown (KeyCode.Space) || _durationTimer.IsOffCooldown) && !_isPressed) {
			_isPressed = true;
//			if (GameStateManager.gameStateManager._actTitleIndex == 0) {
				_titleTextTimer.Reset ();
				_bodyTextTimer.Reset ();
				_durationTimer.CooldownTime = _secondDuration [GameStateManager.gameStateManager._actTitleIndex];
				_durationTimer.Reset ();
				if (_fadeStates == 0) {
					_fadeStates = 1;
				}
				else if (_fadeStates == 2) {
					_fadeStates = 3;
				}
//			}
			else {
				Events.G.Raise (new RetryEvent ());
				_isText = false;
				_titleTextTimer.Reset ();
				_bodyTextTimer.Reset ();
			}
		}
	}

	void FixedUpdate () {
		if(GameStateManager.gameStateManager._actTitleIndex == 0){
			_left.color = Color.Lerp (Color.black, _fadeToColor, _blackTimer.PercentTimePassed);
			_right.color = _left.color;
		} else {
			_left.color = Color.Lerp (_originalColor, _fadeToColor, _blackTimer.PercentTimePassed);
			_right.color = _left.color;
		}
			if (_blackTimer.IsOffCooldown && _once) {
				_titleTextTimer.Reset ();
				_bodyTextTimer.Reset ();
				_durationTimer.Reset ();
// 				_audioSource.clip = _titleCardClips [0];
//				_audioSource.Play ();
				_once = false;
				_isText = true;
			_isPressed = false;
			}
			if (_isText) {
//				if (GameStateManager.gameStateManager._actTitleIndex == 0) {
					if (_fadeStates == 0) {
						_tempAlpha = Mathf.Lerp (0.0f, 1.0f, _bodyTextTimer.PercentTimePassed);
						_tempBodyColor.a = _tempAlpha;
						_bodyText [GameStateManager.gameStateManager._actTitleIndex].color = _tempBodyColor;
					}
					else if (_fadeStates == 1) {
					_tempAlpha = Mathf.Lerp (0.0f, 1.0f, _bodyTextTimer.PercentTimeLeft);
						_tempBodyColor.a = _tempAlpha;
							
						_bodyText [GameStateManager.gameStateManager._actTitleIndex].color = _tempBodyColor;


						if (_bodyTextTimer.IsOffCooldown) {
							_titleTextTimer.Reset ();
						_fadeStates = 2;
						_isPressed = false;
						}
					}
					else if (_fadeStates == 2) {
					_tempAlpha = Mathf.Lerp (0.0f, 1.0f, _titleTextTimer.PercentTimePassed);

					_tempTitleColor.a = _tempAlpha;
					_titleText [GameStateManager.gameStateManager._actTitleIndex].color = _tempTitleColor;
					}
					else {
						_left.color = Color.Lerp (_fadeToColor, _originalColor, _titleTextTimer.PercentTimePassed);
						_right.color = _left.color;
						_tempAlpha = Mathf.Lerp (1.0f, 0.0f, _titleTextTimer.PercentTimePassed);
						_tempTitleColor.a = _tempAlpha;
						_titleText[GameStateManager.gameStateManager._actTitleIndex].color = _tempTitleColor;
						// Event Call to Next Scene Here
				if (GameStateManager.gameStateManager._actTitleIndex == 0) {
					Events.G.Raise (new Load1_1Event ());
				}
				else if (GameStateManager.gameStateManager._actTitleIndex == 1) {
					Events.G.Raise (new Load2_1Event ());
				}
			}
//			}
		}
	}
}

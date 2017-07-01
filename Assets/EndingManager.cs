using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndingManager : MonoBehaviour {
	[SerializeField] SpriteRenderer _left;
	[SerializeField] SpriteRenderer _right;
	[SerializeField] AudioClip[] _endClips;
	[SerializeField] Text _text;
	AudioSource _audioSource;
	Color _originalColor;
	Timer _blackTimer = new Timer(2.0f);
	Timer _textTimer = new Timer(4.0f);
	bool _once = true;
	bool _isText = false;
	bool _transition = false;
	float _tempAlpha;
	Color _tempTextColor;
	[SerializeField] bool _isDemoEnd = false;

	[SerializeField] Text[] _endDot;
	[SerializeField] Image[] _endStrike;
	[SerializeField] Text[] _endText;
	Color _endOriginalColor;
	Color _tempEndTextColor;

	// Use this for initialization
	void Start () {
		_originalColor = _left.color;
		_tempTextColor = _text.color;
		_endOriginalColor = _endDot [0].color;
		_tempEndTextColor = _endDot [0].color;
		_audioSource = gameObject.GetComponent<AudioSource> ();
		_blackTimer.Reset ();
	}
	
	void Update(){
		if (_isText && Input.GetKeyDown (KeyCode.Space)) {
			if (_isDemoEnd) {
				Events.G.Raise (new LoadVeryBeginningEvent ());
			} else {
				Events.G.Raise (new RetryEvent ());
			}
			_isText = false;
			_text.color = Color.white;
			_tempTextColor = Color.white;
			_transition = true;
			_textTimer.Reset ();
		}
	}

	void FixedUpdate () {
		if (!_transition) {
			_left.color = Color.Lerp (_originalColor, Color.black, _blackTimer.PercentTimePassed);
			_right.color = _left.color;
			if (_blackTimer.IsOffCooldown && _once) {
				_textTimer.Reset ();
				if (_isDemoEnd) {
					_audioSource.clip = _endClips [2];
				} else {
					_audioSource.clip = _endClips [Random.Range (0, 6)];
				}
				_audioSource.Play ();
				_once = false;
				_isText = true;
			}
			if (_isText) {
			
				_tempAlpha = Mathf.Lerp (0.0f, 1.0f, _textTimer.PercentTimePassed);
				_tempTextColor.a = _tempAlpha;
				_text.color = _tempTextColor;

				_tempEndTextColor.a = _tempAlpha;
				for (int i=0; i<_endDot.Length; i++){
					_endDot[i].color = _tempEndTextColor;
					if (GameStateManager._acquiredStates [i]) {
						_endText [i].color = _tempEndTextColor;
						_endStrike [i].color = _tempEndTextColor;
					}
				}
			}
		}
		else {
			_left.color = Color.Lerp (Color.black, _originalColor, _textTimer.PercentTimePassed);
			_right.color = _left.color;
			_tempAlpha = Mathf.Lerp (1.0f, 0.0f, _textTimer.PercentTimePassed);
			_tempTextColor.a = _tempAlpha;
			_text.color = _tempTextColor;

			_tempEndTextColor = Color.Lerp (Color.black, _endOriginalColor, _textTimer.PercentTimePassed);
			_tempEndTextColor.a = _tempAlpha;
			for (int i=0; i<_endDot.Length; i++){
				_endDot[i].color = _tempEndTextColor;
				if (GameStateManager._acquiredStates [i]) {
					_endText [i].color = _tempEndTextColor;
					_endStrike [i].color = _tempEndTextColor;
				}
			}
		}
	}
}

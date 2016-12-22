using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TitleCardManager : MonoBehaviour {
	[SerializeField] SpriteRenderer _left;
	[SerializeField] SpriteRenderer _right;
	[SerializeField] AudioClip[] _titleCardClips;
	[SerializeField] Text _text;
	[SerializeField] Color _fadeToColor;
	AudioSource _audioSource;
	Color _originalColor;
	Timer _blackTimer = new Timer(1f);
	Timer _textTimer = new Timer(2f);
	bool _once = true;
	bool _isText = false;
	bool _transition = false;
	float _tempAlpha;
	Color _tempTextColor;




	// Use this for initialization
	void Start () {
		_originalColor = _left.color;
		_tempTextColor = _text.color;

		_audioSource = gameObject.GetComponent<AudioSource> ();
		_blackTimer.Reset ();
		if (_fadeToColor == null) {
			_fadeToColor = Color.black;
		}
	}

	void Update(){
		if (_isText && Input.GetKeyDown (KeyCode.Space)) {
			Events.G.Raise (new RetryEvent ());
			_isText = false;
			_text.color = Color.white;
			_tempTextColor = Color.white;
			_transition = true;
			_textTimer.Reset ();
		}
	}

	void FixedUpdate () {
		if (!_transition) {
			_left.color = Color.Lerp (_originalColor, _fadeToColor, _blackTimer.PercentTimePassed);
			_right.color = _left.color;
			if (_blackTimer.IsOffCooldown && _once) {
				_textTimer.Reset ();
// 				_audioSource.clip = _titleCardClips [0];
//				_audioSource.Play ();
				_once = false;
				_isText = true;
			}
			if (_isText) {

				_tempAlpha = Mathf.Lerp (0.0f, 1.0f, _textTimer.PercentTimePassed);
				_tempTextColor.a = _tempAlpha;
				_text.color = _tempTextColor;

			}
		}
		else {
			_left.color = Color.Lerp (_fadeToColor, _originalColor, _textTimer.PercentTimePassed);
			_right.color = _left.color;
			_tempAlpha = Mathf.Lerp (1.0f, 0.0f, _textTimer.PercentTimePassed);
			_tempTextColor.a = _tempAlpha;
			_text.color = _tempTextColor;

		}
	}
}

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
	float _tempAlpha;
	Color _tempTextColor;
	// Use this for initialization
	void Start () {
		_originalColor = _left.color;
		_tempTextColor = _text.color;
		_audioSource = gameObject.GetComponent<AudioSource> ();
		_blackTimer.Reset ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		_left.color = Color.Lerp (_originalColor, Color.black, _blackTimer.PercentTimePassed);
		_right.color = _left.color;
		if (_blackTimer.IsOffCooldown && _once) {
			_textTimer.Reset ();
			_audioSource.clip = _endClips [Random.Range (0, 6)];
			_audioSource.Play ();
			_once = false;
			_isText = true;
		}
		if (_isText) {
			_tempAlpha = Mathf.Lerp (0.0f, 1.0f, _textTimer.PercentTimePassed);
			_tempTextColor.a = _tempAlpha;
			_text.color = _tempTextColor;
		}
	}
}

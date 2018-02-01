using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextReaction : MonoBehaviour {
	[SerializeField] Timer _FadeTimer;
	[SerializeField] Timer _TapTimer;
	[SerializeField] Color _TapColor;
	[SerializeField] SpriteRenderer _TextSprite;
	Color _EndAlpha;
	Color _StartAlpha;
	Color _OriginColor;
	Color _BlinkAlpha;
	bool _isFadeIn = false;
	bool _isFadeStart = false;
	bool _isTapping = false;
	bool _isDisableTap = true;
	bool _isBlinking = false;

	// Use this for initialization
	void Start () {
		_FadeTimer = new Timer (2f);
		_TapTimer = new Timer (0.1f);
		_TextSprite = GetComponent<SpriteRenderer> ();
		_OriginColor = _TextSprite.color;
		_EndAlpha = _TextSprite.color;
		_StartAlpha = _EndAlpha;
		_BlinkAlpha = _EndAlpha;
		_StartAlpha.a = 0f;
		_BlinkAlpha.a *= 0.7f;
		_TextSprite.color = _StartAlpha;
		print (_StartAlpha + " ## " + _EndAlpha);
		//_FadeTimer.Reset ();

	}
	
	// Update is called once per frame
	void Update () {
		
		if (!_FadeTimer.IsOffCooldown && _isFadeStart) {
			if (_isFadeIn) {
				print ("####Fade In");
				_TextSprite.color = Color.Lerp (_StartAlpha, _EndAlpha, _FadeTimer.PercentTimePassed);
			} else {
				print ("####Fade out");
				_TextSprite.color = Color.Lerp (_EndAlpha, _StartAlpha, _FadeTimer.PercentTimePassed);
			}
		} else if (_FadeTimer.IsOffCooldown && _isFadeStart) {
			_isFadeStart = false;
		}

		if (_TapTimer.IsOffCooldown && _isTapping && !_isDisableTap) {
			_TextSprite.color = _OriginColor;
			_isTapping = false;
		}

		if (!_isDisableTap && !_isFadeStart && _isBlinking) {
			_TextSprite.color = Color.Lerp(_EndAlpha, _StartAlpha, Mathf.PingPong(Time.time, 1));
		}
		
	}

	public void TextFadeIn(){
		if (!_isFadeIn) {
			_isFadeIn = true;
			_FadeTimer.Reset ();
			_isFadeStart = true;
			_isDisableTap = false;
		}
	}

	public void TextFadeOut(){
		if (_isFadeIn) {
			_isFadeIn = false;
			_FadeTimer.Reset ();
			_isFadeStart = true;
			_isDisableTap = true;
		}

	}

	public void Tap(){
		if (!_isDisableTap ) {
			_TapTimer.Reset ();
			//_TextSprite.color = _TapColor;
			_TextSprite.color = _StartAlpha;
			_isTapping = true;
		}

	}

	public void Blink(){
		_isBlinking = true;
	}

	public void Confirm(){
		if (_isBlinking) {
			_isBlinking = false;
		}
		_TextSprite.color = Color.white;
	}
}

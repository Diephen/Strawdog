using UnityEngine;
using System.Collections;

public class HideFeedback : MonoBehaviour {
	[SerializeField] SpriteRenderer[] _hideSprites;
	Color _tempColor;
	Color _origColor;
	Color _newColor;
	bool _lightUp = false;
	bool _once = false;
	[SerializeField] bool _house = false;
	GameObject _hideGameObject;
	Timer _transitionTimer;
	// Use this for initialization
	void Start () {
		_transitionTimer = new Timer (0.5f);
		if (_house) {
			for (int i = 0; i < _hideSprites.Length; i++) {
				_tempColor = _hideSprites [i].color;
				_tempColor.a = 0.0f;
				_newColor = _tempColor;
				_tempColor.a = 70.0f/255.0f;
				_origColor = _tempColor;
				_hideSprites [i].color = _newColor;
			}
		}
		else {
			for (int i = 0; i < _hideSprites.Length; i++) {
				_origColor = _hideSprites [i].color;
				_tempColor = _hideSprites [i].color;
				_tempColor.r = (_hideSprites [i].color.r / 5.0f) * 3.0f;
				_tempColor.g = (_hideSprites [i].color.g / 5.0f) * 3.0f;
				_tempColor.b = (_hideSprites [i].color.b / 5.0f) * 3.0f;
				_newColor = _tempColor;
				_hideSprites [i].color = _newColor;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (_lightUp) {
			if (_once) {
//				if (!_house) {
					for (int i = 0; i < _hideSprites.Length; i++) {
				_hideSprites [i].color = Color.Lerp (_newColor, _origColor, _transitionTimer.PercentTimePassed);
//						_tempColor = _hideSprites [i].color;
//						_tempColor.r = (_hideSprites [i].color.r / 3.0f) * 5.0f;
//						_tempColor.g = (_hideSprites [i].color.g / 3.0f) * 5.0f;
//						_tempColor.b = (_hideSprites [i].color.b / 3.0f) * 5.0f;
//						_hideSprites [i].color = _tempColor;
					}
//				}
//				else {
//					for (int i = 0; i < _hideSprites.Length; i++) {
//						_tempColor = _hideSprites [i].color;
//						_tempColor.a = 0.0f;
//						_hideSprites [i].color = _tempColor;
//					}
//				}

			}
		}
		else {
			if (_once) {
//				if (!_house) {
					for (int i = 0; i < _hideSprites.Length; i++) {
				_hideSprites [i].color = Color.Lerp (_newColor, _origColor, _transitionTimer.PercentTimeLeft);
//						_tempColor = _hideSprites [i].color;
//						_tempColor.r = (_hideSprites [i].color.r / 5.0f) * 3.0f;
//						_tempColor.g = (_hideSprites [i].color.g / 5.0f) * 3.0f;
//						_tempColor.b = (_hideSprites [i].color.b / 5.0f) * 3.0f;
//						_hideSprites [i].color = _tempColor;
					}
//				}
//				else {
//					for (int i = 0; i < _hideSprites.Length; i++) {
//						_tempColor = _hideSprites [i].color;
//						_tempColor.a = 198.0f / 255.0f;
//						_hideSprites [i].color = _tempColor;
//
//
//					}
//				}
//				_once = false;
			}
		}
		if(_transitionTimer.IsOffCooldown){
			_once = false;
		}
	}

	public void LightUp(GameObject hideObject){
		if (_hideGameObject != hideObject) {
			_hideGameObject = hideObject;
			_transitionTimer.Reset ();
			_lightUp = true;
			_once = true;
		}

	}

	public void LightDown(GameObject hideObject){
		if (hideObject == _hideGameObject) {
			_hideGameObject = null;
			_transitionTimer.Reset ();
			_lightUp = false;
			_once = true;
		}
	}
}

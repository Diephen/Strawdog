using UnityEngine;
using System.Collections;

public class Reveal : MonoBehaviour {
	float _fadeDuration = 6f;
	SpriteRenderer _thisRenderer;
	SpriteRenderer _childRenderer;
	Timer _fadeTime;
	bool _beginFade = false;

	Color _tempThis;
	Color _tempChild;

	float _thisAlpha; 
	float _childAlpha; 

	// Use this for initialization
	void Start () {
		_fadeTime = new Timer (_fadeDuration);
		_thisRenderer = gameObject.GetComponent<SpriteRenderer> ();
		_childRenderer = gameObject.transform.GetChild (0).gameObject.GetComponent<SpriteRenderer> ();
		_tempThis = _thisRenderer.color;
		_tempChild = _childRenderer.color;
		_thisAlpha = _tempThis.a;
		_childAlpha = _tempChild.a;
	}
	
	// Update is called once per frame
	void Update () {
		if (_beginFade) {
			_tempThis.a = Mathf.Lerp (_thisAlpha, 0.0f, _fadeTime.PercentTimePassed);
			_tempChild.a = Mathf.Lerp (_childAlpha, 1.0f, _fadeTime.PercentTimePassed);

			_thisRenderer.color = _tempThis;
			_childRenderer.color = _tempChild;
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Guard" && !_beginFade) {
			_fadeTime.Reset ();
			_beginFade = true;
		}
	}
}

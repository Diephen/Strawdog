using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeWindow : MonoBehaviour {
	[SerializeField] SpriteRenderer _NightSprite;
	[SerializeField] SpriteRenderer _BackSprite;
	[SerializeField] Color _EndColor;
	Color _StartColor;

	Color _StartAlpha;
	Color _EndAlpha;
	bool _isNight = false;
	Timer _nightTimer;

	// Use this for initialization
	void Start () {
		_StartAlpha = _NightSprite.color;
		_EndAlpha = _StartAlpha;
		_EndAlpha.a = 1.0f;
		_nightTimer = new Timer(15f);
		_StartColor = _BackSprite.color;
		
	}
	
	// Update is called once per frame
	void Update () {

		if (_isNight && !_nightTimer.IsOffCooldown) {
			// day --> night
			_NightSprite.color = Color.Lerp (_StartAlpha, _EndAlpha, _nightTimer.PercentTimePassed);
			// interior light change
			_BackSprite.color = Color.Lerp(_StartColor, _EndColor, _nightTimer.PercentTimePassed);
		}
		
		
	}


	void OnTriggerEnter2D(Collider2D other){
		if (other.name == "GuardStructure" && !_isNight) {
			_isNight = true;
			_nightTimer.Reset ();
		}

	}
}

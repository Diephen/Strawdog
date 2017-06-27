using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeMap : MonoBehaviour {
	Color _initAlpha;
	Color _lightAlpha;
	Timer _glowTimer; 
	bool _isGlow = false;
	SpriteRenderer _spriteRenderer;

	// Use this for initialization
	void Start () {
		_spriteRenderer = GetComponent<SpriteRenderer> ();
		_initAlpha = _spriteRenderer.color;
		_lightAlpha = _initAlpha;
		_lightAlpha.a = 0.8f;
		_glowTimer = new Timer (1f);
		
	}
	
	// Update is called once per frame
	void Update () {
		// map light up 
		if(_isGlow && !_glowTimer.IsOffCooldown){
			//Color.Lerp(_glowColor, _originColor, _doorGlowTimer.PercentTimeLeft)
			print("light up");
			_spriteRenderer.color = Color.Lerp(_initAlpha, _lightAlpha, _glowTimer.PercentTimePassed);
		}else if(!_isGlow && !_glowTimer.IsOffCooldown){
			print("dark");
			_spriteRenderer.color = Color.Lerp(_lightAlpha, _initAlpha, _glowTimer.PercentTimePassed);
		}
		
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.name == "GuardStructure" && !_isGlow) {
			_isGlow = true;
			_glowTimer.Reset ();
		}

	}

	void OnTriggerExit2D(Collider2D other){
		if (other.name == "GuardStructure" && _isGlow) {
			_isGlow = false;
			_glowTimer.Reset ();
		}

	}
		
}

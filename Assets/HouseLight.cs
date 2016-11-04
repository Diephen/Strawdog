using UnityEngine;
using System.Collections;

public class HouseLight : MonoBehaviour {
	[SerializeField] GameObject _light;
	SpriteRenderer _lightRenderer;
	[SerializeField] SpriteRenderer _glowOverRenderer;
	BoxCollider2D _lightCollider;
	bool _coloring = false;
	Timer _colorTimer;
	bool _once = false;
	Color _tempColor;

	void Start () {
		_lightRenderer = _light.GetComponent<SpriteRenderer> ();
		_lightCollider = _light.GetComponent<BoxCollider2D> ();
		_colorTimer = new Timer (3.0f);

	}
	
	void FixedUpdate () {
		if (_coloring) {
			
			_tempColor = Color.Lerp (Color.white, Color.red, _colorTimer.PercentTimePassed);
			_lightRenderer.color = _tempColor;
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "soldier") {
			_lightRenderer.enabled = true;
			_glowOverRenderer.enabled = true;
			_lightCollider.enabled = true;
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.tag == "soldier") {
			_lightRenderer.enabled = false;
			_glowOverRenderer.enabled = false;
			_lightCollider.enabled = false;
		}
	}

	void StartColor(LightCaughtEvent e){
		if (e.Id == 3 && !_once) {
			_colorTimer.Reset ();
			_coloring = true;
			_once = true;
		}
	}

	void StopColor(LightOffEvent e){
		_lightRenderer.color = Color.white;
		_coloring = false;
		_once = false;
	}

	void OnEnable(){
		Events.G.AddListener<LightCaughtEvent>(StartColor);
		Events.G.AddListener<LightOffEvent>(StopColor);
	}

	void OnDisable(){
		Events.G.AddListener<LightCaughtEvent>(StartColor);
		Events.G.AddListener<LightOffEvent>(StopColor);
	}
}

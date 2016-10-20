using UnityEngine;
using System.Collections;

public class HouseLight : MonoBehaviour {
	[SerializeField] GameObject _light;
	SpriteRenderer _lightRenderer;
	BoxCollider2D _lightCollider;
	// Use this for initialization
	void Start () {
		_lightRenderer = _light.GetComponent<SpriteRenderer> ();
		_lightCollider = _light.GetComponent<BoxCollider2D> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "soldier") {
			_lightRenderer.enabled = true;
			_lightCollider.enabled = true;
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.tag == "soldier") {
			_lightRenderer.enabled = false;
			_lightCollider.enabled = false;
		}
	}
}

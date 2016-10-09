using UnityEngine;
using System.Collections;

public class ParallaxScroll : MonoBehaviour {
	[SerializeField] float _speed = 1.5f;
	[SerializeField] string _triggerCharacterName;
	Transform _followChar;
	bool _triggered = true;

	Vector3 _originalPos;
	Vector3 _newPos;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (_triggered) {
			_newPos = ((_originalPos - _followChar.transform.position) / 2.0f) * _speed;
			transform.position -= _newPos;
			_originalPos = _followChar.transform.position;
		}
	}


	void OnTriggerEnter2D(Collider2D other) {
		if (other.name == _triggerCharacterName) {
			_followChar = other.gameObject.transform.parent;
			_triggered = true;
			_originalPos = _followChar.transform.position;
		}
	}
		
	void OnTriggerExit2D(Collider2D other) {
		if (other.name == _triggerCharacterName) {
			_triggered = false;
		}
	}
}

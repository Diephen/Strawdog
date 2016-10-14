using UnityEngine;
using System.Collections;

public class patrol : MonoBehaviour {
	[SerializeField] MinMax _patrolArea = new MinMax (130.0f, 150.0f);
	[SerializeField] float _speed = 2.0f;
	bool _isLeft = true;

	GameObject _flashlight;

	float _waitSeconds = 2.0f;
	Timer _flashRotationTimer;
	// Use this for initialization
	void Start () {
		_flashlight = gameObject.transform.GetChild (0).gameObject;
		_flashRotationTimer = new Timer (_waitSeconds);
	}
	
	// Update is called once per frame
	void Update () {
		if (!_flashRotationTimer.IsOffCooldown) {
		}

		if (_isLeft) {
			transform.Translate (Vector2.left * Time.deltaTime * _speed);
			if (transform.position.x < _patrolArea.Min) {
				_isLeft = false;
				_flashRotationTimer.Reset ();
			}
		}
		else {
			transform.Translate (Vector2.right * Time.deltaTime * _speed);
			if (transform.position.x > _patrolArea.Max) {
				_isLeft = true;
				_flashRotationTimer.Reset ();
			}
		}
	}
}

using UnityEngine;
using System.Collections;

public class patrol : MonoBehaviour {
	[SerializeField] MinMax _patrolArea = new MinMax (-20.0f, 20.0f);
	float _startPosition;
	[SerializeField] float _speed = 2.0f;
	bool _isLeft = true;

	MinMax _flashlightRot = new MinMax (0.0f, 180.0f);
	Quaternion _rotation;
	float angle;
	Transform _flashlight;
	Light _flashlightLight;
	float _lightSize;

	float _waitSeconds = 2.0f;
	Timer _flashRotationTimer;

	bool _stopAndLook = false;
	bool _reRotate = false;
	rotateTowards _rotateTowards;
	float _lastRotation;

	// Use this for initialization
	void Start () {
		_flashlight = gameObject.transform.GetChild (0);
		_rotateTowards = _flashlight.gameObject.GetComponent<rotateTowards> ();
		_flashlightLight = _flashlight.gameObject.GetComponent<Light> ();
		_flashRotationTimer = new Timer (_waitSeconds);

		_startPosition = transform.position.x;
	}
	
	// Update is called once per frame
	void Update () {
		if (!_stopAndLook) {
			if (!_flashRotationTimer.IsOffCooldown) {
				if (_isLeft) {
					//180 to 0
					angle = MathHelpers.LinMapFrom01 (_flashlightRot.Min, _flashlightRot.Max, _flashRotationTimer.PercentTimePassed);
					_rotation = Quaternion.Euler (angle, 90f, _flashlight.rotation.z);
					_flashlight.localRotation = _rotation;
				}
				else {
					angle = MathHelpers.LinMapFrom01 (_flashlightRot.Min, _flashlightRot.Max, _flashRotationTimer.PercentTimeLeft);
					_rotation = Quaternion.Euler (angle, 90f, _flashlight.rotation.z);
					_flashlight.localRotation = _rotation;
				}
			}
			else {
				if (_isLeft) {
					transform.Translate (Vector2.left * Time.deltaTime * _speed);
					if (transform.position.x < _startPosition + _patrolArea.Min) {
						_isLeft = false;
						_flashRotationTimer.Reset ();
					}
				}
				else {
					transform.Translate (Vector2.right * Time.deltaTime * _speed);
					if (transform.position.x > _startPosition +  _patrolArea.Max) {
						_isLeft = true;
						_flashRotationTimer.Reset ();
					}
				}
			}
		}
		else if (_stopAndLook && _reRotate) {
			if (_isLeft) {
				//180 to 0
				angle = MathHelpers.LinMapFrom01 (_lastRotation, _flashlightRot.Max, _flashRotationTimer.PercentTimePassed);
				_rotation = Quaternion.Euler (angle, 90f, _flashlight.rotation.z);

				_flashlight.localRotation = _rotation;
			}
			else {
				angle = MathHelpers.LinMapFrom01 (_flashlightRot.Min, _lastRotation, _flashRotationTimer.PercentTimeLeft);
				_rotation = Quaternion.Euler (angle, 90f, _flashlight.rotation.z);
				_flashlight.localRotation = _rotation;
			}
			_lightSize = MathHelpers.LinMapFrom01 (25.0f, _flashlightLight.spotAngle, _flashRotationTimer.PercentTimeLeft);
			_flashlightLight.spotAngle = _lightSize;
			if (_flashRotationTimer.IsOffCooldown) {
				_stopAndLook = false;
				_reRotate = false;
			}
		}
	}

	public void StopAndLook(){
		_stopAndLook = true;
		_rotateTowards.enabled = true;
	}

	public void CarryOn(){
		_rotateTowards.enabled = false;
		if (_flashlight.localEulerAngles.z > 300.0f) {
			_lastRotation = _flashlight.localEulerAngles.x;
		}
		else {
			_lastRotation = 180 - _flashlight.localEulerAngles.x;
		}

		StartCoroutine (Wait(_waitSeconds));
	}

	IEnumerator Wait(float waitTime) {
		yield return new WaitForSeconds(waitTime);
		_reRotate = true;
		_flashRotationTimer.Reset ();
	}
}

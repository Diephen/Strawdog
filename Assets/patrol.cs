using UnityEngine;
using System.Collections;
using System.IO;

public class patrol : MonoBehaviour {
	[SerializeField] MinMax _patrolArea = new MinMax (-1.0f, 1.0f);
	float _startPosition;
	[SerializeField] float _speed = 2.0f;
	bool _isLeft = true;

	MinMax _flashlightRot = new MinMax (0.0f, 180.0f);
	Quaternion _rotation;
	float angle;
	Transform _flashlight;
	Light _flashlightLight;
	float _lightSize;

	Timer _flashRotationTimer;
	Timer _waitTimer;
	bool _wait = false;

	Timer _caughtTimer;
	Color _currentColor;

	bool _stopAndLook = false;
	bool _reRotate = false;
	rotateTowards _rotateTowards;
	float _lastRotation;

	// Use this for initialization
	void Start () {
		_flashlight = gameObject.transform.GetChild (0);
		_rotateTowards = _flashlight.gameObject.GetComponent<rotateTowards> ();
		_flashlightLight = _flashlight.gameObject.GetComponent<Light> ();
		_flashRotationTimer = new Timer (2.0f);
		_caughtTimer = new Timer (1.0f);
		_waitTimer = new Timer (2.0f);
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
					if (transform.position.x > _startPosition + _patrolArea.Max) {
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
		else if (_rotateTowards.enabled) {
			Events.G.Raise (new LightCaughtEvent (_caughtTimer.PercentTimePassed));
			_flashlightLight.color = Color.Lerp (_currentColor, Color.red, _caughtTimer.PercentTimePassed);
			if (_caughtTimer.IsOffCooldown) {
				Events.G.Raise (new CaughtSneakingEvent ());
			}
		}
		if (_waitTimer.IsOffCooldown && _wait) {
			_currentColor = _flashlightLight.color;
			_flashRotationTimer.Reset ();
			_reRotate = true;
			_wait = false;
		}
	}

	public void StopAndLook(){
		_stopAndLook = true;
		_reRotate = false;
		_currentColor = _flashlightLight.color;
		_caughtTimer.Reset ();
		_rotateTowards.enabled = true;
	}

	public void CarryOn(){
		Events.G.Raise (new LightOffEvent ());
		_flashlightLight.color = Color.white;
		_rotateTowards.enabled = false;
		if (_flashlight.localEulerAngles.z > 300.0f) {
			_lastRotation = _flashlight.localEulerAngles.x;
		}
		else {
			_lastRotation = 180 - _flashlight.localEulerAngles.x;
		}
		_wait = true;
		_waitTimer.Reset ();
	}
}

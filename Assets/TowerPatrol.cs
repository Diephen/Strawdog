using UnityEngine;
using System.Collections;

public class TowerPatrol : MonoBehaviour {
	bool _isLeft = true;
	bool _wait = false;

	MinMax _towerPatrolRange = new MinMax (-70.0f, 80.0f);
	float angle;
	Vector3 crossProduce;

	Timer _towerTimer;
	Timer _towerCooldownTimer;
	Timer _caughtTimer;

	bool _lookAtFollow = false;
	GameObject _followPerson;

	float _currentRotationZ;
	bool _reRotate = false;

	SpriteRenderer _towerLightSprite;
	Color _currentColor;


	// Use this for initialization
	void Start () {
		_caughtTimer = new Timer (5.0f);
		_towerTimer = new Timer (5.0f);
		_towerCooldownTimer = new Timer (2.0f);
		_towerLightSprite = transform.GetChild (0).GetComponent<SpriteRenderer>();
		_currentColor = _towerLightSprite.color;
	}
	
	// Update is called once per frame
	void Update () {
		if (!_lookAtFollow && !_reRotate) {
			if (_isLeft) {
				//50 to -50
				angle = MathHelpers.LinMapFrom01 (_towerPatrolRange.Min, _towerPatrolRange.Max, _towerTimer.PercentTimePassed);
				transform.localRotation = Quaternion.Euler (0f, 0f, angle);
			}
			else {
				angle = MathHelpers.LinMapFrom01 (_towerPatrolRange.Min, _towerPatrolRange.Max, _towerTimer.PercentTimeLeft);
				transform.localRotation = Quaternion.Euler (0f, 0f, angle);
			}

			if (_towerCooldownTimer.IsOffCooldown && _wait) {
				_towerTimer.Reset ();
				_isLeft = !_isLeft;
				_wait = false;
			}
			else if (_towerTimer.IsOffCooldown && !_wait) {
				_towerCooldownTimer.Reset ();
				_wait = true;
			}
		}
		else if (_lookAtFollow) {
			Vector3 dir = _followPerson.transform.position - transform.position;
			dir.z = 0.0f;
			dir.Normalize ();
			angle = Vector3.Angle (Vector3.down, dir);
			crossProduce = Vector3.Cross (Vector3.down, dir);
			angle = crossProduce.z > 0 ? angle : -angle;
			transform.localRotation = Quaternion.Euler (0f, 0f, angle);
			_towerLightSprite.color = Color.Lerp (_currentColor, Color.red, _caughtTimer.PercentTimePassed);
			if (_caughtTimer.IsOffCooldown) {
				Events.G.Raise (new CaughtSneakingEvent ());
			}
		}
		else if (_reRotate) {
			_towerLightSprite.color = Color.Lerp (_currentColor, Color.white, _towerCooldownTimer.PercentTimePassed);
			if (_isLeft) {
				//50 to -50 -->
				angle = MathHelpers.LinMapFrom01 (_currentRotationZ, _towerPatrolRange.Max, _towerCooldownTimer.PercentTimePassed);
				transform.localRotation = Quaternion.Euler (0f, 0f, angle);
			}
			else {
				// <--
				angle = MathHelpers.LinMapFrom01 (_towerPatrolRange.Min, _currentRotationZ, _towerCooldownTimer.PercentTimeLeft);
				transform.localRotation = Quaternion.Euler (0f, 0f, angle);
			}
			if (_towerCooldownTimer.IsOffCooldown) {
				_reRotate = false;
			}
		}
	}


	void OnTriggerEnter2D(Collider2D other) {
		if (other.name == "FemaleStructure") {
			_lookAtFollow = true;
			_currentColor = _towerLightSprite.color;
			_caughtTimer.Reset ();
			_followPerson = other.gameObject;
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.name == "FemaleStructure") {
			ReRotate ();
		}
	}

	void ReRotate(){
		_currentRotationZ = transform.localEulerAngles.z;
		if (_currentRotationZ > 100.0f) {
			_currentRotationZ = _currentRotationZ - 360.0f;
		}
		_currentColor = _towerLightSprite.color;
		_reRotate = true;
		_lookAtFollow = false;
		_towerCooldownTimer.Reset ();
	}
}

using UnityEngine;
using System.Collections;

public class TowerPatrol : MonoBehaviour {
	float _speed = 2.0f;
	bool _isLeft = true;
	bool _wait = false;

	MinMax _towerPatrolRange = new MinMax (-50.0f, 50.0f);
	float angle;
	Vector3 crossProduce;

	Timer _towerTimer;
	Timer _towerCooldownTimer;

	bool _lookAtFollow = false;
	GameObject _followPerson;

	float _currentRotationZ;
	bool _reRotate = false;

	// Use this for initialization
	void Start () {
		_towerTimer = new Timer (5.0f);
		_towerCooldownTimer = new Timer (2.0f);
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
		}
		else if (_reRotate) {
			if (_isLeft) {
				//50 to -50 -->
				angle = MathHelpers.LinMapFrom01 (_currentRotationZ, _towerPatrolRange.Max, _towerCooldownTimer.PercentTimePassed);
				transform.localRotation = Quaternion.Euler (0f, 0f, angle);
			}
			else {
				// <--

				angle = MathHelpers.LinMapFrom01 (_towerPatrolRange.Min, _currentRotationZ, _towerCooldownTimer.PercentTimeLeft);
				transform.localRotation = Quaternion.Euler (0f, 0f, angle);
				Debug.Log ("z: " + _currentRotationZ);
				Debug.Log("angle: " +angle);


			}
			if (_towerCooldownTimer.IsOffCooldown) {
				_reRotate = false;
			}
		}
	}



	void OnTriggerEnter2D(Collider2D other) {
		if (other.name == "FemaleStructure") {
			_lookAtFollow = true;
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
		_reRotate = true;
		_lookAtFollow = false;
		_towerCooldownTimer.Reset ();
	}
}

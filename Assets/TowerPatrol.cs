using UnityEngine;
using System.Collections;

public class TowerPatrol : MonoBehaviour {
	float _speed = 2.0f;
	bool _isLeft = true;
	bool _wait = false;

	MinMax _towerPatrolRange = new MinMax (-50.0f, 50.0f);
	float angle;

	Timer _towerTimer;
	Timer _towerCooldownTimer;

	// Use this for initialization
	void Start () {
		_towerTimer = new Timer (5.0f);
		_towerCooldownTimer = new Timer (2.0f);
	}
	
	// Update is called once per frame
	void Update () {
			if (_isLeft) {
				//50 to -50
				angle = MathHelpers.LinMapFrom01 (_towerPatrolRange.Min, _towerPatrolRange.Max, _towerTimer.PercentTimePassed);
				transform.localRotation = Quaternion.Euler (0f, 0f, angle);
			Debug.Log (angle);
			}
			else {
				angle = MathHelpers.LinMapFrom01 (_towerPatrolRange.Min, _towerPatrolRange.Max, _towerTimer.PercentTimeLeft);
				transform.localRotation = Quaternion.Euler (0f, 0f, angle);
			}

		if (_towerTimer.IsOffCooldown && !_wait){
			_towerCooldownTimer.Reset ();
			Debug.Log ("tower Timer off");
			_wait = true;
		}
		else if(_towerCooldownTimer.IsOffCooldown && _wait){
			_towerTimer.Reset ();
			_isLeft = !_isLeft;
			Debug.Log ("cooldown timer off");
			_wait = false;
		}
	}
}

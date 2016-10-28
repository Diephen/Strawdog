using UnityEngine;
using System.Collections;
using System.IO;

public class patrol : MonoBehaviour {
	[SerializeField] MinMax _patrolArea = new MinMax (-1.0f, 1.0f);
	float _startPosition;
	[SerializeField] float _speed = 2.0f;
	bool _isLeft = true;
	bool _wait = false;
	bool _turn = true;
	Timer _waitTimer;
	Timer _turnTimer;
	[SerializeField] guardPatrol _guardPatrol;
	[SerializeField] int _id;

	void Start () {
		_startPosition = transform.position.x;
		_waitTimer = new Timer (4.0f);
		_turnTimer = new Timer (2.0f);
	}
	
	void Update () {
		if (!_wait) {
			if (_isLeft) {
				transform.Translate (Vector2.left * Time.deltaTime * _speed);
				if (transform.position.x < _startPosition + _patrolArea.Min) {
					_isLeft = false;
					_waitTimer.Reset ();
					_turnTimer.Reset ();
					_wait = true;
				}
			}
			else {
				transform.Translate (Vector2.right * Time.deltaTime * _speed);
				if (transform.position.x > _startPosition + _patrolArea.Max) {
					_isLeft = true;
					_waitTimer.Reset ();
					_turnTimer.Reset ();
					_wait = true;
				}
			}
		}
		else {
			if (_waitTimer.IsOffCooldown) {
				_wait = false;
				_turn = true;

			} else if (_turnTimer.IsOffCooldown && _turn) {
				_guardPatrol.Turn (!_isLeft);
				_turn = false;
			}
		}
	}

	void Wait(LightCaughtEvent e){
		if (e.Id == _id) {
			_waitTimer.Reset ();
			_wait = true;
		}
	}

	void OnEnable(){
		Events.G.AddListener<LightCaughtEvent>(Wait);
	}
	void OnDisable(){
		Events.G.RemoveListener<LightCaughtEvent>(Wait);
	}
}

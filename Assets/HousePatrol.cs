using UnityEngine;
using System.Collections;

public class HousePatrol : MonoBehaviour {
	[SerializeField] MinMax _patrolArea = new MinMax (-10.0f, 13.2f);
	float _speed = 2.8f;
	[SerializeField] Transform _soldier;
	bool _isLeft = true;
	bool _stop = true;
	bool _caught = false;
	Timer _caughtTimer = new Timer (5.0f);
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (!_stop) {
			if (_isLeft) {
				_soldier.Translate (Vector2.left * Time.deltaTime * _speed);
				if (_soldier.localPosition.x <= _patrolArea.Min) {
					_isLeft = false;
				}
			}
			else {
				_soldier.Translate (Vector2.right * Time.deltaTime * _speed);
				if (_soldier.localPosition.x >= _patrolArea.Max) {
					_isLeft = true;
				}
			}
		}
		else if (_caught)
		{
			Events.G.Raise (new LightCaughtEvent (_caughtTimer.PercentTimePassed));
			if (_caughtTimer.IsOffCooldown) {
				Events.G.Raise (new CaughtSneakingEvent ());
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Prisoner") {
			_caughtTimer.Reset ();
			_stop = false;
			_caught = false;
		}
	}

	public void Stop(){
		if (_stop == false && _caught == false) {
			_caughtTimer.Reset ();
			_stop = true;
			_caught = true;
		}
	}

	public void CarryOn(){
		_stop = false;
		_caught = false;
	}
}

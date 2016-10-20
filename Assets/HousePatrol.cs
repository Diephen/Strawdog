using UnityEngine;
using System.Collections;

public class HousePatrol : MonoBehaviour {
	[SerializeField] MinMax _patrolArea = new MinMax (-10.0f, 13.2f);
	float _speed = 2.8f;
	[SerializeField] Transform _soldier;
	bool _isLeft = true;
	bool _stop = false;
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
	}

	public void Stop(){
		_stop = true;
	}

	public void CarryOn(){
		_stop = false;
	}
}

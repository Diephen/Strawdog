using UnityEngine;
using System.Collections;

public class WalkUpExecution : MonoBehaviour {
	Timer _walkTimer;
	bool _moveLine = false;
	bool _moveToExec = false;
	[SerializeField] bool _isTurnToBeShot;
	bool _gotShot = false;
	// Use this for initialization
	void Start () {
		_walkTimer = new Timer (2.4f);
	}
	
	// Update is called once per frame
	void Update () {
		if (_moveLine && !_gotShot && !_isTurnToBeShot) {
			// wait for a second before walking
			if (_walkTimer.TimePassed >= 1.0f) {
				gameObject.transform.Translate (Vector2.right * Time.deltaTime * 2.0f);
				if (_walkTimer.IsOffCooldown) {
					_moveLine = false;
				}
			}
		} 

		if (_gotShot) {
			gameObject.SetActive (false);
		}

		if (_moveToExec) {
			gameObject.transform.Translate (Vector2.right * Time.deltaTime * 2.0f);	
		}
	}


	void OnTriggerEnter2D(Collider2D other) {
		if (other.name == "ExecutionReady") {
			_moveToExec = true;
			_isTurnToBeShot = true;
		} else if (other.name == "ExecutionSpot") {
			_moveToExec = false;
		}
	}

	void OnEnable(){
		Events.G.AddListener<ShootEvent>(Shot);
	}

	void OnDisable(){
		Events.G.RemoveListener<ShootEvent>(Shot);
	}

	void Shot(ShootEvent e){
		_moveLine = true;
		_walkTimer.Reset ();
		if (_isTurnToBeShot) {
			_gotShot = true;
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckControl : MonoBehaviour {
	[SerializeField] TruckWheel[] _Wheels;
	[SerializeField] float _DrivingSpeedLimit;
	[SerializeField] Transform _TruckTrans;
	Animator _TruckAnim;
	float _Speed;
	Timer _DrivingTimer;
	bool _isDriving = false;
	bool _isStart = false;


	// Use this for initialization
	void Start () {
		_Wheels = GetComponentsInChildren<TruckWheel> ();
		_DrivingTimer = new Timer (5f);
		_TruckAnim = GetComponent<Animator> ();
		
	}
	
	// Update is called once per frame
	void Update () {
		#if UNITY_EDITOR
		if(Input.GetKeyDown(KeyCode.Space)){
			print("## Truck start to move");
			//StartTruck();
			Initiate();
		}
		#endif

		// driving 
		if (_isDriving) {
			if (_Speed < _DrivingSpeedLimit) {
				_Speed += 0.1f * Time.deltaTime;
				_TruckTrans.Translate (_Speed, 0f, 0f);

			}
		} else {
			if (_Speed > 0f) {
				_Speed -= 0.2f * Time.deltaTime;
				_TruckTrans.Translate (_Speed, 0f, 0f);

			} else {
				_Speed = 0f;
			}

		}


		if (_isDriving && _DrivingTimer.IsOffCooldown) {
			_isDriving = false;
			StopTruck ();
		}
		
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.name == "GuardStructure" && !_isStart) {
			print ("## truck start driving");
			Initiate ();
		}

	}

	void Initiate(){
		_TruckAnim.Play ("Truck_Start");
	}

	void StartTruck(){
		foreach (TruckWheel tw in _Wheels) {
			tw.WheelSpin ();
		}
		_isDriving = true;
		_isStart = true;
		_DrivingTimer.Reset ();

	}

	void StopTruck(){
		foreach (TruckWheel tw in _Wheels) {
			tw.StopSpin ();
		}
		_isDriving = false;

	}
		

}

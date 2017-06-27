using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckWheel : MonoBehaviour {
	[SerializeField] float _Speed = -1.0f;
	float _CurSpeed = 0;
	bool _isSpinning = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (_isSpinning) {
			if (_CurSpeed > _Speed) {
				_CurSpeed -= 2*Time.deltaTime;
			}
			transform.Rotate (new Vector3 (0f, 0f, _CurSpeed));
		} else {
			if (_CurSpeed < 0) {
				_CurSpeed += 5 * Time.deltaTime;
				//transform.Rotate (new Vector3 (0f, 0f, _CurSpeed));
				transform.Rotate (new Vector3 (0f, 0f, _CurSpeed));
			} else {
				_CurSpeed = 0f;
			}

		}


	}

	public void WheelSpin(){
		_isSpinning = true;
		
		
	}

	public void StopSpin(){
		_isSpinning = false;
	}

}

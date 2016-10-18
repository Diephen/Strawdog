using UnityEngine;
using System.Collections;
//using System.IO.Ports;

public class SecretDoor_Flaps : MonoBehaviour {
	[SerializeField] Transform _leftFlap;
	[SerializeField] Transform _rightFlap;
	[SerializeField] Transform _door;
	Vector3 _tempLeftScale;
	Vector3 _tempRightScale;
	Vector3 _tempDoorPos;
	float _doorPosX;

	[SerializeField] MinMax _distanceRange = new MinMax (98.5f, 125.0f);
	[SerializeField] MinMax _parallaxRange = new MinMax (0.4f, 1.2f);
	[SerializeField] MinMax _doorRange = new MinMax (45.8f, 48.1f);

	bool _startParralax = false;
	float CamPosLinMap01;
	float LeftFlapWidth;


	void Start () {
		_tempLeftScale = _leftFlap.localScale;
		_tempRightScale = _rightFlap.localScale;
		_tempDoorPos = _door.localPosition;
	}
	
	void Update () {
		if (_startParralax) {
			CamPosLinMap01 = MathHelpers.LinMapTo01 (_distanceRange.Min, _distanceRange.Max, Camera.main.transform.position.x);
			LeftFlapWidth = MathHelpers.LinMapFrom01 (_parallaxRange.Min, _parallaxRange.Max, CamPosLinMap01);
			_doorPosX = MathHelpers.LinMapFrom01 (_doorRange.Min, _doorRange.Max, CamPosLinMap01);

			_tempLeftScale.x = LeftFlapWidth;
			_tempRightScale.x = 1.4f - LeftFlapWidth;
			_leftFlap.localScale = _tempLeftScale;
			_rightFlap.localScale = _tempRightScale;

			_tempDoorPos.x = _doorPosX;
			_door.localPosition = _tempDoorPos;
		}
	}


	void OnTriggerEnter2D(Collider2D other) {
		if (other.name == "FemaleStructure" || other.name == "GuardStructure") {
			_startParralax = true;
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.name == "FemaleStructure" || other.name == "GuardStructure") {
			_startParralax = false;
		}
	}
}

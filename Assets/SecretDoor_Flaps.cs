using UnityEngine;
using System.Collections;
//using System.IO.Ports;

public class SecretDoor_Flaps : MonoBehaviour {
	[SerializeField] Transform _leftFlap;
	[SerializeField] Transform _rightFlap;
	[SerializeField] Transform _door;
	[SerializeField] Transform _leftMain;
	[SerializeField] Transform _rightMain;
	Vector3 _tempLeftScale;
	Vector3 _tempRightScale;
	Vector3 _tempDoorPos;
	float _doorPosX;

	[SerializeField] MinMax _distanceRange = new MinMax (98.5f, 125.0f);
	[SerializeField] MinMax _parallaxRange = new MinMax (0.4f, 1.2f);
	[SerializeField] MinMax _doorRange = new MinMax (45.8f, 48.1f);

	Vector3 _leftMovePos;
	float _currentLeft;
	float _goalLeft;
	Vector3 _rightMovePos;
	float _currentRight;
	float _goalRight;
	float _moveDistance = 10.0f;
	float _tempRMove;
	float _tempLMove;

	MinMax _secretScale = new MinMax(0.8f, 1.1f);

	bool _startParralax = false;
	float CamPosLinMap01;
	float LeftFlapWidth;

	Timer _transitionTimer = new Timer (2.0f);
	bool _transition = false;
	bool _secretOn = false;
	float _doorScaleX;
	Vector3 _tempSecretScale;


	void Start () {
		_tempLeftScale = _leftFlap.localScale;
		_tempRightScale = _rightFlap.localScale;
		_tempDoorPos = _door.localPosition;
		_tempSecretScale = _door.localScale;
		_leftMovePos = _leftMain.position;
		_rightMovePos = _rightMain.position;
		_currentLeft = _leftMovePos.x;
		_goalLeft = _currentLeft - _moveDistance;
		_currentRight = _rightMovePos.x;
		_goalRight = _currentRight + _moveDistance;
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

		if (_transition) {
			if (_secretOn) {
				_doorScaleX = MathHelpers.LinMapFrom01 (_secretScale.Min, _secretScale.Max, _transitionTimer.PercentTimePassed);
				_tempSecretScale.x = _doorScaleX;
				_tempSecretScale.y = _doorScaleX;
				_door.localScale = _tempSecretScale;

				_tempLMove = MathHelpers.LinMapFrom01 (_currentLeft, _goalLeft, _transitionTimer.PercentTimePassed);
				_tempRMove = MathHelpers.LinMapFrom01 (_currentRight, _goalRight, _transitionTimer.PercentTimePassed);
				_leftMovePos.x = _tempLMove;
				_rightMovePos.x = _tempRMove;
				_leftMain.position = _leftMovePos;
				_rightMain.position = _rightMovePos;
			}
			else {
				_doorScaleX = MathHelpers.LinMapFrom01 (_secretScale.Min, _secretScale.Max, _transitionTimer.PercentTimeLeft);
				_tempSecretScale.x = _doorScaleX;
				_tempSecretScale.y = _doorScaleX;
				_door.localScale = _tempSecretScale;

				_tempLMove = MathHelpers.LinMapFrom01 (_currentLeft, _goalLeft, _transitionTimer.PercentTimeLeft);
				_tempRMove = MathHelpers.LinMapFrom01 (_currentRight, _goalRight, _transitionTimer.PercentTimeLeft);
				_leftMovePos.x = _tempLMove;
				_rightMovePos.x = _tempRMove;
				_leftMain.position = _leftMovePos;
				_rightMain.position = _rightMovePos;
			}
			if (_transitionTimer.IsOffCooldown) {
				_transition = false;
			}
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


	void CallSecretDoor(CallSecretDoorEvent e){
		if (!_transition) {
			_transitionTimer.Reset ();
			_transition = true;
			_secretOn = !_secretOn;
			Events.G.Raise (new TransitionSecretDoorEvent (_secretOn));
			if (_secretOn) {
				Events.G.Raise (new DisableMoveEvent ());
			}
			else {
				Events.G.Raise (new EnableMoveEvent ());
			}
		}
	}

	void OnEnable(){
		Events.G.AddListener<CallSecretDoorEvent>(CallSecretDoor);
	}
	void OnDisable(){
		Events.G.RemoveListener<CallSecretDoorEvent>(CallSecretDoor);
	}
}

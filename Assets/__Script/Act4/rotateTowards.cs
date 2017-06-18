using UnityEngine;
using System.Collections;

public class rotateTowards : MonoBehaviour {
	[SerializeField] Transform target;
	[SerializeField] Transform _guardTarget;
	[SerializeField] float speed;
	[SerializeField] AnimationCurve _spotAngleCurve;
	Quaternion _rotation;
	Light _spotLight;
	[SerializeField] Color _warningColor;
	[SerializeField] Color _alertColor;
	Color _originalColor;

	float angle;
	bool _dontChangeColorOfLight = false;

	[SerializeField] MinMax _patrolArea = new MinMax (0.0f, 180.0f);
	[SerializeField] AnimationCurve _patrolCurve;
	[SerializeField] float _patrolDuration = 12.0f;
	Timer _patrolTimer;

//	Timer _alertTimer;

	bool _isStraying = false;
	bool _isWaitingToShoot = false;
	bool _isWarning = false;

	void Start() {
		_spotLight = gameObject.GetComponent <Light> ();
		_originalColor = _spotLight.color;
//		_alertTimer = new Timer (2.0f);
		_patrolTimer = new Timer (_patrolDuration);
	}

	void Update () {

		if (_isStraying && !_isWaitingToShoot) {
			Vector2 dir = target.position - transform.position;

			angle = Vector2.Angle (Vector2.right, dir);


			angle = dir.y < 0 ? angle : -angle;

			_rotation = Quaternion.Euler (angle, 90f, transform.rotation.z);
			transform.localRotation = _rotation;
			_spotLight.spotAngle = MathHelpers.LinMapFrom01 (40.0f, 150.0f, _spotAngleCurve.Evaluate (MathHelpers.LinMapTo01 (0.0f, 180.0f, angle)));
		}
		if (!_isStraying && !_isWaitingToShoot) {

			angle = MathHelpers.LinMapFrom01 (_patrolArea.Min, _patrolArea.Max, _patrolCurve.Evaluate (_patrolTimer.PercentTimePassed));
//
			_rotation = Quaternion.Euler (angle, 90f, transform.rotation.z);
			transform.localRotation = _rotation;
			if (_patrolTimer.IsOffCooldown) {
				_patrolTimer.Reset ();
			}
		}

		if(_isWaitingToShoot && !_isWarning){
			Vector2 dir = _guardTarget.position - transform.position;

			angle = Vector2.Angle (Vector2.right, dir);


			angle = dir.y < 0 ? angle : -angle;

			_rotation = Quaternion.Euler (angle, 90f, transform.rotation.z);
			transform.localRotation = _rotation;
			_spotLight.spotAngle = MathHelpers.LinMapFrom01 (70.0f, 200.0f, _spotAngleCurve.Evaluate (MathHelpers.LinMapTo01 (0.0f, 180.0f, angle)));
		}

		if(_isWarning){
			if(_spotLight.spotAngle < 116.0f){
				_spotLight.spotAngle += 0.4f;
			}
		}
	}

	public void DontChangeColorOfLight(){
		_dontChangeColorOfLight = true;
	}

	void SetStray(AboutToStrayOutOfLineEvent e) {
		if (e.Straying && !_dontChangeColorOfLight) {
			_spotLight.color = _alertColor;
		} //else {
			//_spotLight.color = _originalColor;
//			_alertTimer.Reset ();
		//}
	}

	void FollowPrisoner(ExecutionBreakFree e){
		if(!_dontChangeColorOfLight){
			_isStraying = true;
			_spotLight.color = _warningColor;
		}
	}

	public void FocusLightOnGuard(){
		_isWaitingToShoot = true;
	}

	public void WalkCloser(){
		_isWarning = true;
	}

	void OnEnable ()
	{
		Events.G.AddListener<AboutToStrayOutOfLineEvent>(SetStray);
		Events.G.AddListener<ExecutionBreakFree>(FollowPrisoner);
	}

	void OnDisable ()
	{
		Events.G.RemoveListener<AboutToStrayOutOfLineEvent> (SetStray);
		Events.G.RemoveListener<ExecutionBreakFree> (FollowPrisoner);
	}


}

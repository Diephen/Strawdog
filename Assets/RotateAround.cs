using UnityEngine;
using System.Collections;
using Giverspace;

public class RotateAround : MonoBehaviour {
	MinMax _rotateRange = new MinMax(-90.0f, 160.0f);
//	MinMax _rotateRange = new MinMax(-30.0f, 160.0f);
	Timer _rotateTimer;
	float _rotateDuration = 9.0f;
	Vector3 _eulerRotation;

	Light _dLight;

	[SerializeField] AnimationCurve _lightIntensityCurve;
	// Use this for initialization
	void Start () {
		_dLight = GetComponent<Light> ();
		_rotateTimer = new Timer (_rotateDuration);
		_eulerRotation = transform.eulerAngles;
		_rotateTimer.Reset ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		_eulerRotation.x = MathHelpers.LinMapFrom01 (_rotateRange.Min, _rotateRange.Max, _rotateTimer.PercentTimeLeft);
//		Debug.Log (_rotation);
		transform.rotation = Quaternion.Euler (_eulerRotation);
		_dLight.intensity = Mathf.Lerp (0.0f, 2.0f, _rotateTimer.PercentTimeLeft);


		if (_rotateTimer.IsOffCooldown) {
			Events.G.Raise (new TitleEndedEvent ());
		}
	}
}

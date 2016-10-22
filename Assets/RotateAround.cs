using UnityEngine;
using System.Collections;

public class RotateAround : MonoBehaviour {
	MinMax _rotateRange = new MinMax(-90.0f, 160.0f);
	Timer _rotateTimer;
	float _rotateDuration = 7.0f;
	Vector3 _eulerRotation;
	// Use this for initialization
	void Start () {
		_rotateTimer = new Timer (_rotateDuration);
		_eulerRotation = transform.eulerAngles;
		_rotateTimer.Reset ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		_eulerRotation.x = MathHelpers.LinMapFrom01 (_rotateRange.Min, _rotateRange.Max, _rotateTimer.PercentTimeLeft);
//		Debug.Log (_rotation);
		transform.rotation = Quaternion.Euler (_eulerRotation);
		if (_rotateTimer.IsOffCooldown) {
			Events.G.Raise (new TitleEndedEvent ());
		}
	}
}

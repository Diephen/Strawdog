using UnityEngine;
using System.Collections;

public class rotateTowards : MonoBehaviour {
	[SerializeField] Transform target;
	[SerializeField] float speed;
	[SerializeField] AnimationCurve _spotAngleCurve;
	Quaternion _rotation;
	Light _spotLight;

	void Start() {
		_spotLight = gameObject.GetComponent <Light> ();
	}

	void Update () {
		Vector2 dir = target.position - transform.position;

		float angle = Vector2.Angle(Vector2.right, dir);


		angle = dir.y < 0 ? angle : -angle;

		_rotation = Quaternion.Euler (angle, 90f, transform.rotation.z);
		transform.localRotation = _rotation;
		_spotLight.spotAngle = MathHelpers.LinMapFrom01 (40.0f, 150.0f, _spotAngleCurve.Evaluate (MathHelpers.LinMapTo01 (0.0f, 180.0f, angle)));




	}
}

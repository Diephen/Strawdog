using UnityEngine;
using System.Collections;

public class rotateTowards : MonoBehaviour {
	[SerializeField] Transform target;
	[SerializeField] float speed;
	Quaternion _rotation;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
//		Vector3 targetDir = target.position - transform.position;
//		float step = speed * Time.deltaTime;
//		Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
//		Debug.DrawRay(transform.position, newDir, Color.red);
//		transform.rotation = Quaternion.LookRotation(newDir);
//

		Vector2 dir = target.position;

		float angle = Vector2.Angle(Vector2.right, dir);

		angle = dir.y < 0 ? -angle : angle;

		_rotation = Quaternion.Euler (angle, 90f, transform.rotation.z);
		transform.localRotation = _rotation;
//		transform.Rotate(Vector3.forward, speed * Time.deltaTime * angle);
	}
}

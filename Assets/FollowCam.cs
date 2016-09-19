using UnityEngine;
using System.Collections;

public class FollowCam : MonoBehaviour {
	
	[SerializeField] Transform _followObj;
	Vector3 CameraPos;
	[SerializeField] bool[] XYZ = new bool[3] {false, false, false};

	Vector3 _posDiff;

	void Start () {
		CameraPos = gameObject.transform.position;

		_posDiff = transform.position - _followObj.position;

	}
	
	// Update is called once per frame
	void Update () {
		if (XYZ [0]) {
			CameraPos.x = _followObj.position.x + _posDiff.x;
		} 
		if (XYZ [1]) {
			CameraPos.y = _followObj.position.y + _posDiff.y;
		}
		if (XYZ [2]) {
			CameraPos.z = _followObj.position.z + _posDiff.z;
		}
		gameObject.transform.position = CameraPos;
	}
}

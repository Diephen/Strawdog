using UnityEngine;
using System.Collections;

public class FollowCam : MonoBehaviour {
	
	[SerializeField] Transform _followGuard;
	[SerializeField] Transform _followPrisoner;
	Transform _followObj;
	Vector3 CameraPos;
	[SerializeField] bool[] XYZ = new bool[3] {false, false, false};

	Vector3 _posDiff;

	bool cameraToggle = false;

	float timer = 0f;

	[SerializeField] float _leftCameraPos = 2f;
	[SerializeField] float _rightCameraPos = -2f;

	void Start () {
		CameraPos = gameObject.transform.position;
		_followObj = _followGuard;
//		_posDiff = transform.position - _followGuard.position;

	}
	
	// Update is called once per frame
	void Update () {

//		if (Input.GetKeyDown (KeyCode.T)) {
//			timer = 0f;
//			cameraToggle = !cameraToggle;
//		}
			

		if (XYZ [0]) {
			if (cameraToggle) {
				timer += Time.deltaTime;
				CameraPos.x = Mathf.Lerp (CameraPos.x, _followObj.position.x + _leftCameraPos, timer);
			} else {
				timer += Time.deltaTime;
				CameraPos.x = Mathf.Lerp (CameraPos.x, _followObj.position.x + _rightCameraPos, timer);
			}

//			CameraPos.x = _followObj.position.x + _rightCameraPos;
			//			CameraPos.x = _followObj.position.x + _posDiff.x;
		} 
		if (XYZ [1]) {
			CameraPos.y = _followObj.position.y + _posDiff.y;
		}
		if (XYZ [2]) {
			CameraPos.z = _followObj.position.z + _posDiff.z;
		}
		gameObject.transform.position = CameraPos;
	}


	void OnEnable ()
	{
		Events.G.AddListener<GuardEngaginPrisonerEvent>(OnGuardEngagePrisoner);
	}

	void OnDisable ()
	{
		Events.G.RemoveListener<GuardEngaginPrisonerEvent>(OnGuardEngagePrisoner);
	}
		

	void OnGuardEngagePrisoner (GuardEngaginPrisonerEvent e)
	{
		if (e.Engaged) {
			Debug.Log ("P: Prisoner Engaged");
			timer = 0f;
			_followObj = _followPrisoner;
			cameraToggle = !cameraToggle;
		}
	}
}

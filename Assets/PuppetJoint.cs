using UnityEngine;
using System.Collections;

public class PuppetJoint : MonoBehaviour {
	[SerializeField] Vector3 jointPos;
	[SerializeField] Transform target;
	[SerializeField] Vector3 relativePos;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//transform.position = target.position;
	}

	public void RegisterPos(){
		jointPos = transform.position;
	}

	public void RestorePos(){
		Vector3 npos = transform.position;
		npos = jointPos;
		transform.position = npos;
	}
}

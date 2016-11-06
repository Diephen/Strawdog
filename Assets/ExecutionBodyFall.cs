using UnityEngine;
using System.Collections;

public class ExecutionBodyFall : MonoBehaviour {
	ShotDeathPrisonerHandle m_SBHandle;
	// Use this for initialization
	void Start () {
		m_SBHandle = GetComponentInParent<ShotDeathPrisonerHandle> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Fall(){
		m_SBHandle.PlayBodyFall ();
	}
}

using UnityEngine;
using System.Collections;

public class AnimationEventTest : MonoBehaviour {
	[SerializeField] Animator m_Anim;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetPushDown(){
		m_Anim.SetBool ("IsTorture", true);
	}
}

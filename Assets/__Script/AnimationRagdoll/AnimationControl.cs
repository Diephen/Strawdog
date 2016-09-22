using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimationControl : MonoBehaviour {
	[SerializeField] Animator m_AnimController;   // the animation controller 
	[SerializeField] GameObject m_Structure;
	//[SerializeField] List<Rigidbody2D> m_RigidList;  // all the rigidbody of the controllers \
	[SerializeField] Rigidbody2D[] m_RigidArray;
	[SerializeField] int m_RigidNum;

	[SerializeField] bool m_IsAnimating = false;
	// Use this for initialization
	void Start () {
		// get all the rigidbodies 
		m_RigidArray = m_Structure.GetComponentsInChildren<Rigidbody2D>();
		Debug.Log ("All rigid = " + m_RigidArray.Length);
		if (m_AnimController == null) {
			m_AnimController = gameObject.GetComponent<Animator> ();
		}
		SwitchToRigidBody ();
		//SwitchToAnimation();

	}

	void Awake(){
		
	}

	// Update is called once per frame
	void Update () {


		// test 
		if(Input.GetKeyDown(KeyCode.X)){
			Debug.Log ("Toggle Animtion and Rigidbody");
			m_IsAnimating = !m_IsAnimating;

			if (m_IsAnimating) {
				SwitchToAnimation ();
			} else {
				SwitchToRigidBody ();
			}
		}
	}

	void SwitchToAnimation(){
		m_AnimController.enabled = true;
		foreach (Rigidbody2D _rig in m_RigidArray) {
			_rig.isKinematic = true;	
		}
	}

	void SwitchToRigidBody(){
		m_AnimController.enabled = false;
		foreach (Rigidbody2D _rig in m_RigidArray) {
			
			if (_rig.isKinematic) {
				_rig.isKinematic = false;	
			}

		}
	}
}
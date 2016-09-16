using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	[SerializeField] private Transform target;
	[SerializeField] private float m_Speed;
	[SerializeField] private bool m_IsMoving;
	[SerializeField] private bool m_IsMovingRight;


	// Use this for initialization
	void Awake () {
		m_IsMoving = false;
		m_IsMovingRight = false;
		transform.LookAt (target);

	}

	void Update(){
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			m_IsMovingRight = false;
			m_IsMoving = true;
		}

		if (Input.GetKeyUp (KeyCode.LeftArrow)) {
			//m_IsMovingRight = true;
			m_IsMoving = false;
		}

		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			m_IsMovingRight = true;
			m_IsMoving = true;
		}

		if (Input.GetKeyUp (KeyCode.RightArrow)) {
			//m_IsMovingRight = true;
			m_IsMoving = false;
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
		if (m_IsMoving) {
			if (m_IsMovingRight) {
				transform.Translate (Vector3.right * m_Speed * Time.deltaTime);
			} else {
				transform.Translate (- Vector3.right * m_Speed * Time.deltaTime);
			}
			transform.LookAt (target);

		}

	
	}
}

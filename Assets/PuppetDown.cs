using UnityEngine;
using System.Collections;

public class PuppetDown : MonoBehaviour {

	[SerializeField] Transform m_FingerL;  
	[SerializeField] Transform m_FingerM; 
	[SerializeField] Transform m_FingerR; 
	[SerializeField] GameObject m_Head; 
	[SerializeField] float m_MoveDist;

	[SerializeField] GameObject m_FingerLF;  
	[SerializeField] GameObject m_FingerMF; 
	[SerializeField] GameObject m_FingerRF; 
	[SerializeField] GameObject hiddenThing; 
	[SerializeField] AudioClip walk;
	[SerializeField] AudioClip jump;


	AudioSource m_as;

	Transform m_Torso;

	bool jumping = false;

	bool collapsed = false;

	Rigidbody2D m_TorosRB;


	Rigidbody2D m_TorosRBA;
	Rigidbody2D m_TorosRBB;
	Rigidbody2D m_TorosRBC;
	Rigidbody2D hiddinRB;

	Vector3 posA;
	Vector3 posB;
	Vector3 posC;
	Vector3 posD;

	// Use this for initialization
	void Start () {
		m_Torso = m_Head.transform;
		m_TorosRB = m_Head.GetComponent<Rigidbody2D>();
		m_TorosRBA = m_FingerLF.GetComponent<Rigidbody2D> ();
		m_TorosRBB = m_FingerMF.GetComponent<Rigidbody2D> ();
		m_TorosRBC = m_FingerRF.GetComponent<Rigidbody2D> ();
		hiddinRB = hiddenThing.GetComponent<Rigidbody2D> ();

		posA = m_FingerL.position;
		posB = m_FingerM.position;
		posC = m_FingerR.position;
		posD = hiddenThing.transform.position;
		m_as = gameObject.GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown (KeyCode.D)) {
			if (jumping == false) {
				m_as.clip = walk;
				m_as.Play ();
			}
		}
		if (Input.GetKeyUp (KeyCode.A) || Input.GetKeyUp (KeyCode.D)) {
			if (jumping == false) {
//				m_as.clip = walk;
				m_as.Stop ();
			}
		}

		if (Input.GetKey (KeyCode.A) && Input.GetKey (KeyCode.S) && Input.GetKey (KeyCode.D) && collapsed == false &&jumping == false) {
			HandDown ();
			collapsed = true;


		} 
		if((Input.GetKeyUp (KeyCode.A) || Input.GetKeyUp (KeyCode.S) || Input.GetKeyUp (KeyCode.D)) && collapsed) {
			HandUp ();
			collapsed = false;

		}

		if (Input.GetKeyDown (KeyCode.Space) && jumping ==false && collapsed == false) {
			jumping = true;
			m_TorosRBA.isKinematic = false;
			m_TorosRBB.isKinematic = false;
			m_TorosRBC.isKinematic = false;
			hiddinRB.isKinematic = false;
			m_TorosRBA.AddForce (Vector2.up * 62.0f, ForceMode2D.Impulse);
			m_TorosRBB.AddForce (Vector2.up * 62.0f, ForceMode2D.Impulse);
			m_TorosRBC.AddForce (Vector2.up * 62.0f, ForceMode2D.Impulse);
			hiddinRB.AddForce (Vector2.up * 62.0f, ForceMode2D.Impulse);
//			Debug.Log ("Forced");
			m_as.clip = jump;
		}

		if ((m_FingerL.position.y < posA.y)&&(m_TorosRBA.isKinematic==false)) {
			m_FingerL.position = posA;
			m_TorosRBA.isKinematic = true;
		}

		if ((m_FingerM.position.y < posB.y)&&(m_TorosRBB.isKinematic==false)) {
			m_FingerM.position = posB;
			m_TorosRBB.isKinematic = true;
		}
		if ((m_FingerR.position.y < posC.y)&&(m_TorosRBC.isKinematic==false)) {
			m_FingerR.position = posC;
			m_TorosRBC.isKinematic = true;
			jumping = false;
			m_as.Play ();
		}

		if ((hiddenThing.transform.position.y < posD.y)&&(hiddinRB.isKinematic==false)) {
			hiddenThing.transform.position = posD;
			hiddinRB.isKinematic = true;

		}
	}


	void HandDown(){
		Vector3 posL = m_FingerL.position;
		Vector3 posM = m_FingerM.position;
		Vector3 posR = m_FingerR.position;
		Vector3 posT = m_Torso.position;
		Vector3 posMy = hiddenThing.transform.position;
		posL.y -= m_MoveDist;
		posM.y -= m_MoveDist;
		posR.y -= m_MoveDist;
		posT.y -= m_MoveDist;
		posMy.y -= m_MoveDist;
		m_FingerL.position = posL;
		m_FingerM.position = posM;
		m_FingerR.position = posR;
		hiddenThing.transform.position = posMy;
		m_Torso.position = posT;
	}

	void HandUp(){
		Vector3 posL = m_FingerL.position;
		Vector3 posM = m_FingerM.position;
		Vector3 posR = m_FingerR.position;
		Vector3 posMy = hiddenThing.transform.position;
		Vector3 posT = m_Torso.position;
		posL.y += m_MoveDist;
		posM.y += m_MoveDist;
		posR.y += m_MoveDist;
		posT.y += m_MoveDist;
		posMy.y += m_MoveDist;
		m_FingerL.position = posL;
		m_FingerM.position = posM;
		m_FingerR.position = posR;
		hiddenThing.transform.position = posMy;
		m_Torso.position = posT;
	}
}

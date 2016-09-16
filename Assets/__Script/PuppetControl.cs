using UnityEngine;
using System.Collections;

public class PuppetControl : MonoBehaviour {
	[SerializeField] Animator m_MouthAnim;
	[SerializeField] GameObject[] m_Finger = new GameObject[3];
	[SerializeField] KeyCode[] m_ListenKey = new KeyCode[] {KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.Space};
	[SerializeField] float[] m_MoveDistance = new float[3];
	[SerializeField] float[] m_CrouchMoveDistance = new float[3];
	[SerializeField] float[] m_PickupMoveDistance = new float[3];
	[SerializeField] bool[] m_Continuous = new bool[] {true, false, true};
	bool[] m_Pressed = new bool[] { false, false, false };
	Vector3[] oldPos = new Vector3[3];
	Vector3[] newPos = new Vector3[3];
	Vector3[] crouchPos = new Vector3[3];
	Vector3[] PickupPos = new Vector3[3];

	[SerializeField] float m_MoveSpeed = 2.0f;

	enum charState {idle, left, right, crouch, pickup};
	bool crouchStart = false;
	charState m_charState = charState.idle;
	float[] startTime = new float[3];
	float[] distCovered = new float[3];

	// Use this for initialization
	void Start (){
		for(int i = 0; i < oldPos.Length; i++){
			//Define localPosition of travel for Lerp
			oldPos[i] = m_Finger [i].transform.localPosition;
			newPos[i] = oldPos[i];
			newPos [i].y += m_MoveDistance [i];
			//Define localPosition of travel for crouch Lerp
			crouchPos [i] = oldPos [i];
			crouchPos [i].y += m_CrouchMoveDistance [i];
			//Define localPosition of travel for Pickup Lerp
			PickupPos [i] = oldPos [i];
			PickupPos [i].y += m_PickupMoveDistance [i];
		}
	}
	
	// Update is called once per frame
	void Update () {

	//Handle Pickup
		if (Input.GetKeyDown (m_ListenKey [3])) {
			startTime [0] = Time.time;
			startTime [1] = Time.time;
			startTime [2] = Time.time;
			m_charState = charState.pickup;
			Pickup ();
		} else if (Input.GetKeyUp (m_ListenKey [3])) {
			m_charState = charState.idle;
		}

	//Handle Crouch
		if (Input.GetKey (m_ListenKey [0]) &&
		   Input.GetKey (m_ListenKey [1]) &&
		   Input.GetKey (m_ListenKey [2]) &&
		   m_charState != charState.pickup) {
			m_charState = charState.crouch;
			if (crouchStart == false) {
				crouchStart = true;
				startTime [0] = Time.time;
				startTime [1] = Time.time;
				startTime [2] = Time.time;
			}
		} else {
			crouchStart = false;
		}
			
		if (m_charState != charState.pickup) {
			//A-Key
			if (m_Continuous [0] && Input.GetKey (m_ListenKey [0])
			   && (m_charState == charState.idle || m_charState == charState.left)) {
				m_charState = charState.left;
				Walk (m_charState);
			}
			if (Input.GetKeyDown (m_ListenKey [0])) {
				m_Pressed [0] = true;
				startTime [0] = Time.time;
			} else if (Input.GetKeyUp (m_ListenKey [0])) {
				m_Pressed [0] = false;
				startTime [0] = Time.time;
				m_charState = charState.idle;
			}

			//S-Key
			if (Input.GetKeyDown (m_ListenKey [1])) {
				m_Pressed [1] = true;
				startTime [1] = Time.time;
				// talk 
				if(m_MouthAnim != null){
					Debug.Log ("speak");
					m_MouthAnim.SetTrigger ("TriggerSpeak");
				}
			} else if (Input.GetKeyUp (m_ListenKey [1])) {
				m_Pressed [1] = false;
				startTime [1] = Time.time;
				m_charState = charState.idle;
			}

			// D-Key
			if (m_Continuous [2] && Input.GetKey (m_ListenKey [2])
			   && (m_charState == charState.idle || m_charState == charState.right)) {
				m_charState = charState.right;
				Walk (m_charState);
			}
			if (Input.GetKeyDown (m_ListenKey [2])) {
				m_Pressed [2] = true;
				startTime [2] = Time.time;
			} else if (Input.GetKeyUp (m_ListenKey [2])) {
				m_Pressed [2] = false;
				startTime [2] = Time.time;
				m_charState = charState.idle;
			}
		}

		distCovered [0] = (Time.time - startTime [0]);
		distCovered [1] = (Time.time - startTime [1]);
		distCovered [2] = (Time.time - startTime [2]);

		if (m_charState == charState.crouch) {
			m_Finger [0].transform.localPosition = Vector3.Lerp (m_Finger [0].transform.localPosition, crouchPos [0], distCovered [0]);
			m_Finger [1].transform.localPosition = Vector3.Lerp (m_Finger [1].transform.localPosition, crouchPos [1], distCovered [1]);
			m_Finger [2].transform.localPosition = Vector3.Lerp (m_Finger [2].transform.localPosition, crouchPos [2], distCovered [2]);
		} else if (m_charState == charState.pickup) {
			m_Finger [0].transform.localPosition = Vector3.Lerp (m_Finger [0].transform.localPosition, PickupPos [0], distCovered [0]);
			m_Finger [1].transform.localPosition = Vector3.Lerp (m_Finger [1].transform.localPosition, PickupPos [1], distCovered [1]);
			m_Finger [2].transform.localPosition = Vector3.Lerp (m_Finger [2].transform.localPosition, PickupPos [2], distCovered [2]);
		} else {
			if (m_Pressed [0]) {
				m_Finger [0].transform.localPosition = Vector3.Lerp (m_Finger [0].transform.localPosition, newPos [0], distCovered [0]);
			} else {
				m_Finger [0].transform.localPosition = Vector3.Lerp (m_Finger [0].transform.localPosition, oldPos [0], distCovered [0]);
			}
			if (m_Pressed [1]) {
				m_Finger [1].transform.localPosition = Vector3.Lerp (m_Finger [1].transform.localPosition, newPos [1], distCovered [1]);
			} else {
				m_Finger [1].transform.localPosition = Vector3.Lerp (m_Finger [1].transform.localPosition, oldPos [1], distCovered [1]);
			}
			if (m_Pressed [2]) {
				m_Finger [2].transform.localPosition = Vector3.Lerp (m_Finger [2].transform.localPosition, newPos [2], distCovered [2]);
			} else {
				m_Finger [2].transform.localPosition = Vector3.Lerp (m_Finger [2].transform.localPosition, oldPos [2], distCovered [2]);
			}
		}
	}

	void Walk(charState dir){
		if (dir == charState.left) {
			transform.Translate (Vector2.left * m_MoveSpeed * Time.deltaTime);
		} else if (dir == charState.right) {
			transform.Translate (Vector2.right * m_MoveSpeed * Time.deltaTime);
		}
	}

	void Pickup() {
		
	}
}

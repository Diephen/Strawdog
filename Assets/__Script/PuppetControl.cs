using UnityEngine;
using System.Collections;

public class PuppetControl : MonoBehaviour {
	[SerializeField] Animator m_MouthAnim;
	[SerializeField] GameObject[] m_Finger = new GameObject[4];
	//1 for walk, 2 for speak
	[SerializeField] AudioClip[] m_Audio = new AudioClip[3];
	[SerializeField] AudioSource[] m_AudioSource = new AudioSource[3];
	[SerializeField] KeyCode[] m_ListenKey = new KeyCode[] {KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.Space};
	[SerializeField] float[] m_MoveDistance = new float[3];
	[SerializeField] float[] m_CrouchMoveDistance = new float[4];
	[SerializeField] float[] m_PickupMoveDistance = new float[4];
	[SerializeField] bool[] m_Continuous = new bool[] {true, false, true};
	bool[] m_Pressed = new bool[] { false, false, false, false};
	Vector2[] oldPos = new Vector2[4];
	Vector2[] newPos = new Vector2[4];
	Vector2[] crouchPos = new Vector2[4];
	Vector2[] PickupPos = new Vector2[4];

	[SerializeField] float m_MoveSpeed = 2.0f;

	enum charState {idle, left, right, crouch, pickup};
	bool crouchStart = false;
	bool _isWalking = false;
	charState m_charState = charState.idle;
	float[] startTime = new float[4];
	float[] distCovered = new float[4];

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
			startTime [3] = Time.time;
			m_charState = charState.pickup;
			Pickup ();
			m_Pressed [3] = true;
		} else if (Input.GetKeyUp (m_ListenKey [3])) {
			m_charState = charState.idle;
			m_Pressed [3] = false;
		}

	//Handle Crouch
		if (Input.GetKey (m_ListenKey [0]) &&
		   Input.GetKey (m_ListenKey [1]) &&
		   Input.GetKey (m_ListenKey [2]) &&
		   m_charState != charState.pickup) {
			m_charState = charState.crouch;
			if (crouchStart == false) {
				m_AudioSource [1].clip = m_Audio [2];
				m_AudioSource [1].Play ();
				crouchStart = true;
				startTime [0] = Time.time;
				startTime [1] = Time.time;
				startTime [2] = Time.time;
				startTime [3] = Time.time;
			}
			m_Pressed [3] = true;
		} else {
			crouchStart = false;
			m_Pressed [3] = false;
		}
			
		if (m_charState != charState.pickup) {
			//Get Key Up D Moved Higher to have smooth audio transition
			if (Input.GetKeyUp (m_ListenKey [2])) {
				m_Pressed [2] = false;
				startTime [2] = Time.time;
				m_charState = charState.idle;
			}

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
				if (!m_AudioSource[1].isPlaying) {
					m_AudioSource [1].clip = m_Audio [1];
					m_AudioSource [1].Play ();
				}
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
			} // Get Key up [2] Moved up for smooth transition

			if (_isWalking == true) {
				if (m_AudioSource[0].isPlaying == false) {
					m_AudioSource[0].clip = m_Audio [0];
					m_AudioSource[0].Play ();
				}
			}
			if (m_charState != charState.left && m_charState != charState.right) {
				_isWalking = false;
				m_AudioSource[0].Stop ();
			}
		}

		distCovered [0] = (Time.time - startTime [0]);
		distCovered [1] = (Time.time - startTime [1]);
		distCovered [2] = (Time.time - startTime [2]);
		distCovered [3] = (Time.time - startTime [3]);

		if (m_charState == charState.crouch) {
			m_Finger [0].transform.localPosition = Vector2.Lerp (m_Finger [0].transform.localPosition, crouchPos [0], distCovered [0]);
			m_Finger [1].transform.localPosition = Vector2.Lerp (m_Finger [1].transform.localPosition, crouchPos [1], distCovered [1]);
			m_Finger [2].transform.localPosition = Vector2.Lerp (m_Finger [2].transform.localPosition, crouchPos [2], distCovered [2]);
			//Hidden String for Head
			m_Finger [3].transform.localPosition = Vector2.Lerp (m_Finger [3].transform.localPosition, crouchPos [3], distCovered [3]);
		} else if (m_charState == charState.pickup) {
			m_Finger [0].transform.localPosition = Vector2.Lerp (m_Finger [0].transform.localPosition, PickupPos [0], distCovered [0]);
			m_Finger [1].transform.localPosition = Vector2.Lerp (m_Finger [1].transform.localPosition, PickupPos [1], distCovered [1]);
			m_Finger [2].transform.localPosition = Vector2.Lerp (m_Finger [2].transform.localPosition, PickupPos [2], distCovered [2]);
			//Hidden String for Head
			m_Finger [3].transform.localPosition = Vector2.Lerp (m_Finger [3].transform.localPosition, PickupPos [3], distCovered [3]);
		} else {
			if (m_Pressed [0]) {
				m_Finger [0].transform.localPosition = Vector2.Lerp (m_Finger [0].transform.localPosition, newPos [0], distCovered [0]);
			} else {
				m_Finger [0].transform.localPosition = Vector2.Lerp (m_Finger [0].transform.localPosition, oldPos [0], distCovered [0]);
			}
			if (m_Pressed [1]) {
				m_Finger [1].transform.localPosition = Vector2.Lerp (m_Finger [1].transform.localPosition, newPos [1], distCovered [1]);
			} else {
				m_Finger [1].transform.localPosition = Vector2.Lerp (m_Finger [1].transform.localPosition, oldPos [1], distCovered [1]);
			}
			if (m_Pressed [2]) {
				m_Finger [2].transform.localPosition = Vector2.Lerp (m_Finger [2].transform.localPosition, newPos [2], distCovered [2]);
			} else {
				m_Finger [2].transform.localPosition = Vector2.Lerp (m_Finger [2].transform.localPosition, oldPos [2], distCovered [2]);
			}
			if (m_Pressed [3] == false) {
				m_Finger [3].transform.localPosition = Vector2.Lerp (m_Finger [3].transform.localPosition, oldPos [3], distCovered [3]);
			}
		}
	}

	void Walk(charState dir){
		_isWalking = true;
		if (dir == charState.left) {
			transform.Translate (Vector2.left * m_MoveSpeed * Time.deltaTime);
		} else if (dir == charState.right) {
			transform.Translate (Vector2.right * m_MoveSpeed * Time.deltaTime);
		}
	}

	void Pickup() {
		
	}
}

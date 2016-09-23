﻿using UnityEngine;
using System.Collections;

public class PuppetControl : MonoBehaviour {
	[SerializeField] Animator m_MouthAnim;
	[SerializeField] GameObject[] m_Finger = new GameObject[3];
	//add animators 
	private Animator[] m_Animator = new Animator[4];
	//1 for walk, 2 for speak
	[SerializeField] AudioClip[] m_Audio = new AudioClip[3];
	[SerializeField] AudioSource[] m_AudioSource = new AudioSource[3];
	[SerializeField] KeyCode[] m_ListenKey = new KeyCode[] {KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.Space};
	[SerializeField] float[] m_MoveDistance = new float[3];

	[SerializeField] float[] m_CrouchMoveDistance = new float[3];
	[SerializeField] float[] m_PickupMoveDistance = new float[3];
	[SerializeField] bool[] m_Continuous = new bool[] {true, false, true};
	bool[] m_Pressed = new bool[] { false, false, false, false};
	Vector3[] oldPos = new Vector3[3];
	Vector3[] newPos = new Vector3[3];
	Vector3[] crouchPos = new Vector3[3];
	Vector3[] PickupPos = new Vector3[3];

	[SerializeField] float m_MoveSpeed = 2.0f;

	enum charState {idle, left, right, crouch, pickup};
	bool crouchStart = false;
	bool _isWalking = false;
	bool isSpeak = false;
	charState m_charState = charState.idle;
	float[] startTime = new float[4];
	float[] distCovered = new float[4];

	// GameObject for Body Spotlight
	[SerializeField] GameObject _spotlight;
	//Light Controller Script
	[SerializeField] LightControl _lightControl;

	// Will be used to selectively disable functionality to be replaced with animation
	public bool[] _stateHandling = new bool[] {true, true, true, true, true, true, true};

	// Use this for initialization
	void Start (){
		StringCalculation ();
	}

	void StringCalculation(){
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
			// init animator
			if (m_Finger [i].GetComponentsInChildren<Animator> () != null) {
				m_Animator [i] = m_Finger [i].GetComponentInChildren<Animator> ();
			} else {
				m_Animator [i] = null;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		Pickup ();
		Crouch ();
		KeyHandle ();
		//LerpHandle ();
	}

	void FixedUpdate(){
		// jiggling -> the walk function should be called here 
		ContinuousKeyHandle ();
		LerpHandle ();
	}

	void Walk(charState dir){
		_isWalking = true;
		if (dir == charState.left) {
			transform.Translate (Vector3.left * m_MoveSpeed * Time.deltaTime);
		} else if (dir == charState.right) {
			transform.Translate (Vector3.right * m_MoveSpeed * Time.deltaTime);
		}
	}

	void Pickup() {
		//Handle Pickup
		if (Input.GetKeyDown (m_ListenKey [3])) {
			startTime [0] = Time.time;
			startTime [1] = Time.time;
			startTime [2] = Time.time;
			startTime [3] = Time.time;
			m_charState = charState.pickup;
		} else if (Input.GetKeyUp (m_ListenKey [3])) {
			m_charState = charState.idle;
		}
	}

	void Crouch() {
		//Handle Crouch
		//TODO: add the delay time for the three keys 
		// if any of the three keys is pressed, wait for deltatime for the other keys to happen 
		// else -- press single key ??

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
			}
		} else {
			crouchStart = false;
		}
	}

	void ContinuousKeyHandle(){
		if (m_charState != charState.pickup) {
			//A-Key
			if (m_Continuous [0] && Input.GetKey (m_ListenKey [0])
			   && (m_charState == charState.idle || m_charState == charState.left)) {
				m_charState = charState.left;
				Walk (m_charState);
			}
			// D-Key
			if (m_Continuous [2] && Input.GetKey (m_ListenKey [2])
			   && (m_charState == charState.idle || m_charState == charState.right)) {
				m_charState = charState.right;
				Walk (m_charState);
			}
		}
	}

	void KeyHandle() {
		if (m_charState != charState.pickup) {
			//Get Key Up D Moved Higher to have smooth audio transition
			if (Input.GetKeyUp (m_ListenKey [2])) {
				m_Pressed [2] = false;
				startTime [2] = Time.time;
				m_charState = charState.idle;
			}


			if (Input.GetKeyDown (m_ListenKey [0])) {
				/// string animation
				if (m_Animator [0] != null) {
					m_Animator [0].SetBool ("IsPull", true);
				}
				m_Pressed [0] = true;
				startTime [0] = Time.time;
			} else if (Input.GetKeyUp (m_ListenKey [0])) {
				if (m_Animator [0] != null) {
					m_Animator [0].SetBool ("IsPull", false);
				}
				m_Pressed [0] = false;
				startTime [0] = Time.time;
				m_charState = charState.idle;
			}

			//S-Key
			if (Input.GetKeyDown (m_ListenKey [1]) && m_charState != charState.crouch) {
				m_Pressed [1] = true;
				startTime [1] = Time.time;
				_lightControl.SpotlightFlicker (_spotlight);
				if (!m_AudioSource [1].isPlaying) {
					m_AudioSource [1].clip = m_Audio [1];
					m_AudioSource [1].Play ();
				}
				// talk 
				if (m_MouthAnim != null && !isSpeak) {
					Debug.Log ("speak");

					//m_MouthAnim.SetTrigger ("TriggerSpeak");
					m_MouthAnim.SetBool ("IsSpeak", true);
					isSpeak = true;
				}
			} else if (Input.GetKeyUp (m_ListenKey [1]) && m_charState != charState.crouch) {
				m_Pressed [1] = false;
				startTime [1] = Time.time;
				m_charState = charState.idle;
				if (isSpeak) {
					m_MouthAnim.SetBool ("IsSpeak", false);
					isSpeak = false;
				}

			} else if (m_charState == charState.crouch) {
				m_MouthAnim.SetBool ("IsSpeak", false);
				isSpeak = false;
			}


			if (Input.GetKeyDown (m_ListenKey [2])) {
				m_Pressed [2] = true;
				startTime [2] = Time.time;
				if (m_Animator [2] != null) {
					m_Animator [2].SetBool ("IsPull", true);
				}
			} // Get Key up [2] Moved up for smooth transition
			else if(Input.GetKeyUp(m_ListenKey[2])){
				if (m_Animator [2] != null) {
					m_Animator [2].SetBool ("IsPull", false);
				}
				m_Pressed [2] = false;
				startTime [2] = Time.time;
				m_charState = charState.idle;
			}

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
	}

	void LerpHandle() {
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
}

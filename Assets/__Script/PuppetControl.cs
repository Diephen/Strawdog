﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum CharacterIdentity {Guard, Prisoner, Both};

public class PuppetControl : MonoBehaviour {
	[SerializeField] CharacterIdentity _whoAmI;
	[SerializeField] bool m_flip;
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

	enum charState {idle, left, right, crouch, pickup, anim};
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

	Timer _DHoldTimer;

	[SerializeField] bool _disableKeys = false;


	// debug moving speed 
	bool _speedUp = false;

	// Will be used to selectively disable functionality to be replaced with animation
	/* 
	 * [0]: PickUp Down
	 * [1]: PickUp Up
	 * [2]: Crouch Down
	 * [3]: Walk Left
	 * [4]: Walk Right
	 * [5]: Left Arm Down
	 * [6]: Left Arm Up
	 * [7]: Speak Down
	 * [8]: Speak Up
	 * [9]: Right Arm Down
	 * [10]: Right Arm Up
	 */
	public bool[] _stateHandling = new bool[11] {true, true, true, true, true, true, true, true, true, true, true};




	void Awake(){

		_DHoldTimer = new Timer (0.3f);
	}

	// Use this for initialization
	void Start (){
		StringCalculation ();
		/*if (_whoAmI == CharacterIdentity.Guard) {
			if (m_flip) {
				m_ListenKey [2] = GameStateManager.gameStateManager._gLeftKey;
				m_ListenKey [0] = GameStateManager.gameStateManager._gRightKey;
			}
			else {
				m_ListenKey [0] = GameStateManager.gameStateManager._gLeftKey;
				m_ListenKey [2] = GameStateManager.gameStateManager._gRightKey;
			}
			m_ListenKey [3] = GameStateManager.gameStateManager._gDownKey;
			m_ListenKey [1] = GameStateManager.gameStateManager._gUpKey;
		}
		else if (_whoAmI == CharacterIdentity.Prisoner) {
			if (m_flip) {
				m_ListenKey [0] = GameStateManager.gameStateManager._pRightKey;
				m_ListenKey [2] = GameStateManager.gameStateManager._pLeftKey;
			}
			else {
				m_ListenKey [2] = GameStateManager.gameStateManager._pRightKey;
				m_ListenKey [0] = GameStateManager.gameStateManager._pLeftKey;
			}
			m_ListenKey [3] = GameStateManager.gameStateManager._pDownKey;
			m_ListenKey [1] = GameStateManager.gameStateManager._pUpKey;
		}*/
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
		if(!_disableKeys){
			Pickup ();
			Crouch ();
			KeyHandle ();
			//LerpHandle ();
		}

		#if UNITY_EDITOR
		if(Input.GetKeyDown(KeyCode.LeftControl)){
			_speedUp = !_speedUp;
		}
		if(_speedUp){
			m_MoveSpeed = 10f;
		}else{
			m_MoveSpeed = 3f;
		}

		#endif
	}

	void FixedUpdate(){
		if (!_disableKeys) {
			// jiggling -> the walk function should be called here 
			ContinuousKeyHandle ();
			LerpHandle ();
		}

	}

	void Walk(charState dir){
		_isWalking = true;
		if (dir == charState.left) {
			Events.G.Raise (new IsWalkingEvent (_whoAmI, _isWalking, true));
			transform.Translate (Vector3.left * m_MoveSpeed * Time.deltaTime);
		} else if (dir == charState.right) {
			Events.G.Raise (new IsWalkingEvent (_whoAmI, _isWalking, false));
			transform.Translate (Vector3.right * m_MoveSpeed * Time.deltaTime);
		}
	}

	void Pickup() {
		//Handle Pickup
		if (Input.GetKeyDown (m_ListenKey [3])) {
			if (_stateHandling [0] == true) {
				startTime [0] = Time.time;
				startTime [1] = Time.time;
				startTime [2] = Time.time;
				startTime [3] = Time.time;
				m_charState = charState.pickup;
			} else {
				Events.G.Raise (new PickUpPressedEvent (_whoAmI));
			}
		} else if (Input.GetKeyUp (m_ListenKey [3])) {
			if (_stateHandling [1] == true) {
				m_charState = charState.idle;
			} else {
				Events.G.Raise (new PickupReleasedEvent (_whoAmI));
				m_charState = charState.idle;
			}
		}
	}

	void Crouch() {
		//Handle Crouch
		//TODO: add the delay time for the three keys 
		// if any of the three keys is pressed, wait for deltatime for the other keys to happen 
		// else -- press single key ??

		if (Input.GetKey (m_ListenKey [3]) )
		{
			if (_stateHandling [2] == true) {
				m_charState = charState.crouch;
				Events.G.Raise (new CrouchHideEvent (_whoAmI));
				if (crouchStart == false) {
					m_AudioSource [1].clip = m_Audio [2];
					m_AudioSource [1].Play ();
					crouchStart = true;
				}
			} else {
				Events.G.Raise (new CrouchPressedEvent (_whoAmI));
			}
		} else {
			Events.G.Raise (new CrouchReleaseHideEvent (_whoAmI));
			crouchStart = false;
		}
	}

	void ContinuousKeyHandle(){
		if (m_charState != charState.pickup) {
			if(m_flip){
				//A-Key
				if (m_Continuous [0] && Input.GetKey (m_ListenKey [2])
					&& (m_charState == charState.idle || m_charState == charState.left)) {
					WalkLeftKeyPress();
				}
				// D-Key
				if (m_Continuous [2] && Input.GetKey (m_ListenKey [0])
					&& (m_charState == charState.idle || m_charState == charState.right)) {
					WalkRightKeyPress();
				}
			} else {
				//A-Key
				if (m_Continuous [0] && Input.GetKey (m_ListenKey [0])
					&& (m_charState == charState.idle || m_charState == charState.left)) {
					WalkLeftKeyPress();
				}
				// D-Key
				if (m_Continuous [2] && Input.GetKey (m_ListenKey [2])
					&& (m_charState == charState.idle || m_charState == charState.right)) {
					WalkRightKeyPress();
				}
			}
		
		}
	}

	void WalkRightKeyPress(){
		if (_stateHandling [4] == true) {
			m_charState = charState.right;
			Walk (m_charState);
		} else {
			//Jung-Ho: Let me know if you need to use this
			//					Events.G.Raise (new WalkLeftEvent (_whoAmI));
		}
	}

	void WalkLeftKeyPress(){
		if (_stateHandling [3] == true) {
			m_charState = charState.left;
			Walk (m_charState);
		} else {
			//Jung-Ho: Let me know if you need to use this
			//					Events.G.Raise (new WalkRightEvent (_whoAmI));
		}
	}

	void KeyHandle() {
		if (m_charState != charState.pickup ) {
			if (Input.GetKeyDown (m_ListenKey [0]) && m_charState != charState.crouch) {
				if (m_Animator [0] != null) {
					m_Animator [0].SetBool ("IsPull", true);
				}
				if (_stateHandling [5] == true) {
					/// string animation

					m_Pressed [0] = true;
					startTime [0] = Time.time;
				} else {
					Events.G.Raise (new APressedEvent (_whoAmI));
				}
			} else if (Input.GetKeyUp (m_ListenKey [0])) {
				if (m_Animator [0] != null) {
					m_Animator [0].SetBool ("IsPull", false);
				}
				if (_stateHandling [6] == true) {
					m_Pressed [0] = false;
					startTime [0] = Time.time;

				} else {
					Events.G.Raise (new AReleasedEvent (_whoAmI));
				}
				m_charState = charState.idle;
			}

			//S-Key
			if (Input.GetKeyDown (m_ListenKey [1]) && m_charState != charState.crouch) {
				if (m_Animator [1] != null) {
					m_Animator [1].SetBool ("IsPull", true);
				}
				if (_stateHandling [7] == true) {
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
				} else {
					Events.G.Raise (new SPressedEvent (_whoAmI));
				}
			} else if (Input.GetKeyUp (m_ListenKey [1])) {
				if (m_Animator [1] != null) {
					m_Animator [1].SetBool ("IsPull", false);
				}
				if (_stateHandling [8] == true) {
					m_Pressed [1] = false;
					startTime [1] = Time.time;

					if (isSpeak) {
						m_MouthAnim.SetBool ("IsSpeak", false);
						isSpeak = false;
					}
				} else {
					Events.G.Raise (new SReleasedEvent (_whoAmI));
				}
				m_charState = charState.idle;

			} else if (m_charState == charState.crouch) {
				m_MouthAnim.SetBool ("IsSpeak", false);
				isSpeak = false;
			}


			if (Input.GetKeyDown (m_ListenKey [2])&& m_charState != charState.crouch ) {
				if (m_Animator [2] != null) {
					m_Animator [2].SetBool ("IsPull", true);
				}
				if (_stateHandling [9] == true) {
					m_Pressed [2] = true;
					startTime [2] = Time.time;

				} else {
					Events.G.Raise (new DPressedEvent (_whoAmI));
					_DHoldTimer.Reset ();
				}
			}
			if (Input.GetKey (m_ListenKey [2])&& m_charState != charState.crouch ) {
				if (_stateHandling [9] == false) {
					if (_DHoldTimer.IsOffCooldown) {
						Events.G.Raise (new DHoldEvent (_whoAmI));
					}
				}
			}
			else if(Input.GetKeyUp(m_ListenKey[2])){
				if (m_Animator [2] != null) {
					m_Animator [2].SetBool ("IsPull", false);
				}
				//Get Key Up D Moved Higher to have smooth audio transition
				if (_stateHandling [10] == true) {
					m_Pressed [2] = false;
					startTime [2] = Time.time;

				} else {
					Events.G.Raise (new DReleasedEvent (_whoAmI));
				}
				m_charState = charState.idle;
			}

			if (_isWalking == true) {
				if (m_AudioSource[0]!= null && m_AudioSource[0].isPlaying == false) {
					m_AudioSource[0].clip = m_Audio [0];
					m_AudioSource[0].Play ();
				}
			}
			if (m_charState != charState.left && m_charState != charState.right) {
				_isWalking = false;
				Events.G.Raise (new IsWalkingEvent (_whoAmI, _isWalking, true));
				if (m_AudioSource [0] != null) {
					m_AudioSource[0].Stop ();
				}

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

	public void MoveRight(){
		transform.Translate (Vector3.right);
	}


	void OnTriggerStay2D(Collider2D other) {
		if(m_flip){
			if (other.name == "STOPLeft") {
				_stateHandling [4] = false;
			}
			else if (other.name == "STOPRight") {
				_stateHandling [3] = false;
			}
			if (_whoAmI == CharacterIdentity.Guard) {
				if (other.name == "STOPLeftGuard") {
					_stateHandling [4] = false;
				}
				else if (other.name == "STOPRightGuard") {
					_stateHandling [3] = false;
				}
			} else if (_whoAmI == CharacterIdentity.Prisoner) {
				if (other.name == "STOPLeftPrisoner") {
					_stateHandling [4] = false;
				}
				else if (other.name == "STOPRightPrisoner") {
					_stateHandling [3] = false;
				}
			}
		} else {
			if (other.name == "STOPLeft") {
				_stateHandling [3] = false;
			}
			else if (other.name == "STOPRight") {
				_stateHandling [4] = false;
			}
			if (_whoAmI == CharacterIdentity.Guard) {
				if (other.name == "STOPLeftGuard") {
					_stateHandling [3] = false;
				}
				else if (other.name == "STOPRightGuard") {
					_stateHandling [4] = false;
				}
			} else if (_whoAmI == CharacterIdentity.Prisoner) {
				if (other.name == "STOPLeftPrisoner") {
					_stateHandling [3] = false;
				}
				else if (other.name == "STOPRightPrisoner") {
					_stateHandling [4] = false;
				}
			}
//		else if (other.name == "open-left") {
//			_stateHandling [3] = false;
//		}
//		else if (other.name == "open-right") {
//			_stateHandling [4] = false;
//		}
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if(m_flip){
			if (other.name == "STOPLeft") {
				_stateHandling [4] = true;
			}
			else if (other.name == "STOPRight") {
				_stateHandling [3] = true;
			}
			if (_whoAmI == CharacterIdentity.Guard) {
				if (other.name == "STOPLeftGuard") {
					_stateHandling [4] = true;
				}
				else if (other.name == "STOPRightGuard") {
					_stateHandling [3] = true;
				}
			} else if (_whoAmI == CharacterIdentity.Prisoner) {
				if (other.name == "STOPLeftPrisoner") {
					_stateHandling [4] = true;
				}
				else if (other.name == "STOPRightPrisoner") {
					_stateHandling [3] = true;
				}
			}
		} else {
			if (other.name == "STOPLeft") {
				_stateHandling [3] = true;
			}
			else if (other.name == "STOPRight") {
				_stateHandling [4] = true;
			}
			if (_whoAmI == CharacterIdentity.Guard) {
				if (other.name == "STOPLeftGuard") {
					_stateHandling [3] = true;
				}
				else if (other.name == "STOPRightGuard") {
					_stateHandling [4] = true;
				}
			} else if (_whoAmI == CharacterIdentity.Prisoner) {
				if (other.name == "STOPLeftPrisoner") {
					_stateHandling [3] = true;
				}
				else if (other.name == "STOPRightPrisoner") {
					_stateHandling [4] = true;
				}
			}
		}
//		else if (other.name == "open-left") {
//			_stateHandling [3] = true;
//		}
//		else if (other.name == "open-right") {
//			_stateHandling [4] = true;
//		}
	}

	public void DisableKeyInput(){
		_disableKeys = true; 
		m_AudioSource [0].Stop ();
	}

	public void EnableKeyInput(){
		_disableKeys = false;
	}

	public KeyCode[] GetKeyCodes(){
		return m_ListenKey;
	}

	public void DisableContinuousWalk() {
		m_Continuous [0] = false;
		m_Continuous [2] = false;
	}

	public void EnableContinuousWalk() {
		m_Continuous [0] = true;
		m_Continuous [2] = true;
	}

	void EnableMove(EnableMoveEvent e){
		if (e.WhoAmI == CharacterIdentity.Both || e.WhoAmI == _whoAmI) {
			print ("## Enable move" + e.WhoAmI.ToString ());
			_stateHandling [4] = true;
			_stateHandling [3] = true;
			_stateHandling [7] = true;
		}
	}
	void DisableMove(DisableMoveEvent e){
		if (e.WhoAmI == CharacterIdentity.Both || e.WhoAmI == _whoAmI) {
			
			_stateHandling [4] = false;
			_stateHandling [3] = false;
		}
	}

	public void SwitchWalk(AudioClip _audioClip){
		m_Audio [0] = _audioClip;
		m_AudioSource [0].clip = m_Audio [0];
	}

	void OnEnable(){
		Events.G.AddListener<EnableMoveEvent>(EnableMove);
		Events.G.AddListener<DisableMoveEvent>(DisableMove);
	}

	void OnDisable ()
	{
		Events.G.RemoveListener<EnableMoveEvent> (EnableMove);
		Events.G.RemoveListener<DisableMoveEvent> (DisableMove);
	}

	public void StopWalkAudio(){
		m_charState = charState.anim;
	}
}

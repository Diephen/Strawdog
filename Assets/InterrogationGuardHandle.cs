using UnityEngine;
using System.Collections;

public class InterrogationGuardHandle : MonoBehaviour {
	enum IG_GuardState{
		Idle = 0,
		Question,           // waiting for answer when idling 
		PushQuestion,       // waiting for answer when pushing prisoner
		WalkToRight,
		BackToIdle,
		EndKickAfterIdle,
		EndKickAfterPush,
	}
	IG_GuardState m_GS;
	Animator m_Anim;
	[SerializeField] InterrogationType m_Type = InterrogationType.prisoner_no;
	[SerializeField] Interrogation m_ItrSceneManager;
	[SerializeField] InterrogationPrisonerHandler m_IPHandle;
	[SerializeField] InteractionSound m_ItrAudio;
	[SerializeField] Transform m_StartX;
	[SerializeField] Transform m_EndX;
	[SerializeField] GameObject m_Bottom;
	//[SerializeField] SpriteRenderer m_LeftArm;
	[SerializeField] float m_Speed;
	//[SerializeField] float[] m_WaitTime;

	SpriteRenderer[] m_BottomSpr;
	bool _callOnce = true;

	float dir = 1;
	float m_CurWaitTime = -1;
	float m_StartTime = -1;
	bool m_IsAnswer = false;
	bool m_IsWalk = false;
	bool m_IsAtStart = true;
	bool m_IsEnd = false;
	bool m_interroAggressiveOnce = false;
	// 
	bool m_IsInterrogationEnd = false;
	bool m_IsInterrogationPaused = false;
	bool m_IsNewComboGenerated = false;

	bool m_WalkOnce = false;


	[SerializeField] Symbol[] m_ComboPool;
	[SerializeField] Sprite[] m_Symbols;
	[SerializeField] InterrogationLight[] m_Lights;
	Timer m_ComboTimer;

	// check key inputs 
	int m_CurrentKey = -1;
	int m_CurrentLight = -1;


	// Use this for initialization
	void Awake () {
		
		m_Anim = GetComponent<Animator> ();
		//m_CurWaitTime = 2f;
		//m_StartTime = Time.time;
		m_BottomSpr = m_Bottom.GetComponentsInChildren<SpriteRenderer>();

		// max number 
		m_ComboTimer = new Timer (5f);
		//m_Lights = FindObjectsOfType<InterrogationLight> ();
	}

	void Start(){
		m_GS = IG_GuardState.Idle;
		ChangeState ();
		//m_ComboTimer.Reset ();
		m_CurrentLight = 0;
	}
	
	// Update is called once per frame
	void Update () {
		// 
		//Behaviour ();
		UpdateBehaviour();
		//CheckWalkPos ();

		//print ("Timer val: " + m_ComboTimer.TimeLeft);
		if (!m_IsInterrogationEnd && !m_IsEnd) {
			if (!m_IsInterrogationPaused) {
				if (m_ComboTimer.IsOffCooldown && !m_IsNewComboGenerated) {
					// if time is up fail state 
					// GenerateCombo ();
					m_GS = IG_GuardState.Idle;
					ChangeState ();
					UpdateLights (false);

					foreach (Symbol sb in m_ComboPool) {
						StartCoroutine (sb.HideSymbol(0.8f));
					}
				} else if (!m_ComboTimer.IsOffCooldown) {
					if (m_CurrentKey == 4) {
						m_CurrentKey = -1;
						m_GS = IG_GuardState.Idle;
						ChangeState ();
						UpdateLights (true);
						foreach (Symbol sb in m_ComboPool) {
							StartCoroutine (sb.HideSymbol (1f));
						}
						//m_ComboTimer.Reset ();
					}

					//
					if (m_CurrentLight == m_Lights.Length) {
						print ("###### End interrogation #######");
						m_IsInterrogationEnd = true;
					}

					//
					if (Input.anyKeyDown) {
						//print ("Check the current key input");
						if (Input.GetKeyDown (KeyCode.W)) {
							CheckKeyInput (3);
						} else if (Input.GetKeyDown (KeyCode.A)) {
							CheckKeyInput (1);
						} else if (Input.GetKeyDown (KeyCode.S)) {
							CheckKeyInput (0);
						} else if (Input.GetKeyDown (KeyCode.D)) {
							CheckKeyInput (2);
						} else {
							//print ("#### Error input ####");
						}
					}

				}// end of checking timer 
			} else {
				if (Input.GetKeyDown (KeyCode.W)) {
					if (!m_IsAnswer && m_GS == IG_GuardState.PushQuestion) {
						m_IsAnswer = true;
						print ("Answer!!!");
					}
				}
			}


		} else if (!m_IsEnd && m_IsInterrogationEnd) {
			// when get all combo correct before scene ends
			if(!m_WalkOnce){
				m_WalkOnce = true;
				m_GS = IG_GuardState.WalkToRight;
				ChangeState ();
				foreach (Symbol sb in m_ComboPool) {
					StartCoroutine (sb.HideSymbol(0.2f));
				}
			}

		} else {
			// when scene ends && not able to get all combo correct 
			foreach (Symbol sb in m_ComboPool) {
				StartCoroutine (sb.HideSymbol(0.2f));
			}
//			foreach (InterrogationLight igt in m_Lights) {
//				igt.TurnLightOff ();
//			}
		}

	
	}

	// generate combination 
	void GenerateCombo(){
		int randomNum;
		//int preNum = 0;
		for (int i = 0; i < 4; i++) {
			//print ("##### " + i + " #####");
			randomNum = Random.Range (0, 4);
			if (m_ComboPool [i].SetSprite (m_Symbols [randomNum], randomNum)) {
			} else {
				i -= 1;
				//print ("Redo");
			}
		}
		m_ComboTimer.Reset ();
		m_CurrentKey = 0;

	}

	// check if using the correct keys  && lights resposes 
	void CheckKeyInput(int keyIdx){
		if (m_CurrentKey > -1 && m_CurrentKey < 4) {
			// print ("### " + m_ComboPool [m_CurrentKey].GetSymbolIdx ());
			if (keyIdx == m_ComboPool [m_CurrentKey].GetSymbolIdx () && !m_ComboPool [m_CurrentKey].IsUnclock) {
				m_ComboPool [m_CurrentKey].HitSymbol ();
				m_CurrentKey += 1;

			} else {
				print ("### Miss the symbol");
				foreach (Symbol sb in m_ComboPool) {
					sb.MissSymbol ();
					StartCoroutine (sb.HideSymbol(0.8f));
					m_CurrentKey = -1;
				}
				//GenerateCombo ();
				// go to the push down 
				UpdateLights (false);
				m_GS = IG_GuardState.PushQuestion;
				m_IsInterrogationPaused = true;
				ChangeState ();
			}
		} 

	}

	// check the lights 
	void UpdateLights(bool isTurnOnLight){
		if (isTurnOnLight) {
			if (m_CurrentLight > -1 && m_CurrentLight < m_Lights.Length) {
				m_Lights [m_CurrentLight].TurnLightOn ();
				m_CurrentLight += 1;
				// guard out hand down 

			} 
		} else {
			if (m_CurrentLight - 1 >= 0) {
				m_CurrentLight -= 1;
				m_Lights [m_CurrentLight].TurnLightOff ();
			}

//			foreach (InterrogationLight lgt in m_Lights) {
//				lgt.TurnLightOff ();
//			}
		}

	}


	// old code, with guard AI
	bool CheckStateEnd(){
		print ("#### " + (Time.time - m_StartTime));
		if (m_CurWaitTime > 0) {
			if (Time.time - m_StartTime >= m_CurWaitTime) {
				return true;
			} else {
				return false;
			}
		} else {
			return false;
		}
	}

	void UpdateBehaviour(){
		switch (m_GS) {
		case IG_GuardState.Idle:
			if (!CheckStateEnd()) {
				print ("Idle");
			} else {
				print ("End Idle");
				if (!m_IsEnd && !m_IsInterrogationEnd ) {
					m_IsNewComboGenerated = false;
					m_GS = IG_GuardState.Question;
					ChangeState ();
				} else if(m_IsEnd) {
					EndSequence ();
				}

			}
			break;
		case IG_GuardState.Question:
			if (m_IsEnd) {
				EndSequence ();
			}
			break;
		case IG_GuardState.WalkToRight:
			if (m_IsWalk && !m_IsAtStart) {
				CheckWalkPos ();
			} else {
				//m_IsWalk = false;
				Events.G.Raise (new TriggerTakenAwayEvent ());
				m_GS = IG_GuardState.Idle;
				ChangeState ();
			}
			break;
		case IG_GuardState.PushQuestion:
			if (!m_IsEnd) {
				if (m_IsAnswer) {
					m_GS = IG_GuardState.BackToIdle;
					ChangeState ();
					m_IsAnswer = false;
					m_interroAggressiveOnce = true;
				} else {
					// question idling
					if (m_interroAggressiveOnce) {
						m_ItrAudio.PlayInterroAggressive ();
						m_interroAggressiveOnce = false;
					}
					print ("Pushing Question Idle");
				}
			} else {
				EndSequence ();
			}

			break;
		case IG_GuardState.BackToIdle:
			if (CheckStateEnd ()) {
				if (!m_IsEnd) {
					m_GS = IG_GuardState.Idle;
					ChangeState ();
					// back to ask questions 
					//GenerateCombo();
					m_IsInterrogationPaused = false;
					//m_ComboTimer.Reset ();
				} else {
					EndSequence ();
				}
			}
			break;
		case IG_GuardState.EndKickAfterIdle:
			if (CheckStateEnd ()) {
				// goes to the end 
				m_GS = IG_GuardState.EndKickAfterPush;
				ChangeState ();
			}
			break;
		case IG_GuardState.EndKickAfterPush:
			if (CheckStateEnd ()) {
				//m_GS = IG_GuardState.EndKickAfterIdle;
				if (_callOnce) {
					m_ItrSceneManager.NextScene ();
					_callOnce = false;
				}
			}
			break;
		}

	}




	// ################ Old Code ###################### //
	// check behaviour only when state change
	void ChangeState(){
		switch (m_GS) {
		case IG_GuardState.Idle:
			m_CurWaitTime = 1.2f;
			m_StartTime = Time.time;
			m_IsNewComboGenerated = true;
			m_Anim.Play ("IG-Idle");
			break;
		case IG_GuardState.Question:
			if (!m_IsEnd && !m_IsInterrogationEnd) {
				GenerateCombo ();
				m_IsNewComboGenerated = false;
				//m_CurWaitTime = 4f;
				//m_StartTime = Time.time;
				m_Anim.Play ("IG-Question");
				m_ItrAudio.PlayInterroNicer ();
			}
			break;
		case IG_GuardState.WalkToRight:
			//m_CurWaitTime = 3f;
			//m_StartTime = Time.time;
			m_Anim.Play ("IG-Walk");
			m_IsWalk = true;
			m_IsAtStart = false;
			m_ItrAudio.PlayInterroSteps ();
			break;
		case IG_GuardState.PushQuestion:
			m_CurWaitTime = -1f;
			m_StartTime = Time.time;
			m_Anim.Play ("IG-ForceRead");
			Events.G.Raise (new InterrogationQuestioningEvent (true));
			m_IPHandle.ForceToRead ();
			break;
		case IG_GuardState.BackToIdle:
			m_CurWaitTime = 3f;
			m_StartTime = Time.time;
			m_Anim.Play ("IG-BackToIdle");
			m_IPHandle.BackToIdle ();
			break;
		case IG_GuardState.EndKickAfterIdle:
			m_CurWaitTime = 1.5f;
			m_StartTime = Time.time;
			m_Anim.Play ("IG-WalkToKick");
			//m_IPHandle.EndScene ();
			break;
		case IG_GuardState.EndKickAfterPush:
			m_CurWaitTime = 2f;
			m_StartTime = Time.time;
			m_Anim.Play ("IG-Kick");
			m_IPHandle.EndScene ();
			break;
		}
		if (m_GS != IG_GuardState.PushQuestion) {
			Events.G.Raise (new InterrogationQuestioningEvent (false));
		}
	}

	/*
	void Behaviour(){
		switch (m_GS) {
		case IG_GuardState.Idle:
			if (!CheckStateEnd()) {
				print ("Idle");

			} else {
				if (!m_IsEnd) {
					m_GS = IG_GuardState.Question;
					ChangeState ();
				} else {
					EndSequence ();
				}

			}
			break;
		case IG_GuardState.Question:
			if (!CheckStateEnd ()) {
				if (m_IsAnswer && !m_IsEnd) {
					m_GS = IG_GuardState.WalkToRight;
					ChangeState ();
					m_IsAnswer = false;
				} else {
					// question idling
					print("Question Idle");
				}
			} else {
				if (!m_IsEnd) {
					m_GS = IG_GuardState.PushQuestion;
					ChangeState ();
				} else {
					EndSequence ();
				}
			}
			break;
		case IG_GuardState.WalkToRight:
			if (m_IsWalk && !m_IsAtStart) {
				CheckWalkPos ();
			} else {
				m_IsWalk = false;
				if (!m_IsEnd) {
					m_GS = IG_GuardState.Idle;
					ChangeState ();
				} else {
					EndSequence ();
				}
			}
			break;
		case IG_GuardState.PushQuestion:
			if (!m_IsEnd) {
				if (m_IsAnswer) {
					m_GS = IG_GuardState.BackToIdle;
					ChangeState ();
					m_IsAnswer = false;
					m_interroAggressiveOnce = true;
				} else {
					// question idling
					if (m_interroAggressiveOnce) {
						m_ItrAudio.PlayInterroAggressive ();
						m_interroAggressiveOnce = false;
					}
					print ("Pushing Question Idle");
				}
			} else {
				EndSequence ();
			}

			break;
		case IG_GuardState.BackToIdle:
			if (CheckStateEnd ()) {
				if (!m_IsEnd) {
					m_GS = IG_GuardState.Idle;
					ChangeState ();
				} else {
					EndSequence ();
				}
			}
			break;
		case IG_GuardState.EndKickAfterIdle:
			if (CheckStateEnd ()) {
				// goes to the end 
				m_GS = IG_GuardState.EndKickAfterPush;
				ChangeState ();
			}
			break;
		case IG_GuardState.EndKickAfterPush:
			if (CheckStateEnd ()) {
				//m_GS = IG_GuardState.EndKickAfterIdle;
				if (_callOnce) {
					m_ItrSceneManager.NextScene ();
					_callOnce = false;
				}
			}
			break;
		}
			
	}
	*/

	public void EndInterrogation(){
		if (!m_IsEnd) {
			m_IsEnd = true;
			// if got all the answer right -- link back to the cell with note reading
			// guard let go of prisoner 
			if(m_IsInterrogationEnd){
				
			}

			// if didn't get the answer right -- link to the execution 
			// guard kick the prisoner
			else{
				
			}


		}
	}

	// checkeck which state to go to after the sequence ends 
	void EndSequence(){
		if (m_IsEnd && !m_IsInterrogationEnd) {
			print ("Check ending");
			if (m_GS == IG_GuardState.Idle) {
				m_GS = IG_GuardState.EndKickAfterIdle;
			}
			if (m_GS == IG_GuardState.Question ) {
				m_GS = IG_GuardState.EndKickAfterIdle;
			}
			if (m_GS == IG_GuardState.WalkToRight) {
				m_GS = IG_GuardState.Idle;
			}
			if (m_GS == IG_GuardState.PushQuestion) {
				m_GS = IG_GuardState.EndKickAfterPush;
			}
			if (m_GS == IG_GuardState.BackToIdle) {
				m_GS = IG_GuardState.EndKickAfterIdle;
			}
			ChangeState ();
		}
	
	}

	void HideLegSprites(){
		foreach (SpriteRenderer spr in m_BottomSpr) {
			spr.sortingOrder = spr.sortingOrder - 10;
		}
		//m_LeftArm.sortingOrder = m_LeftArm.sortingOrder - 10;
	}

	void ShowLegSprites(){
		foreach (SpriteRenderer spr in m_BottomSpr) {
			spr.sortingOrder = spr.sortingOrder + 10;
		}
		//m_LeftArm.sortingOrder = m_LeftArm.sortingOrder + 10;
	}

	void Flip(){
		Vector3 nscale = transform.localScale;
		nscale.x = - nscale.x;
		transform.localScale = nscale;
	}

	void CheckWalkPos(){
		Vector3 npos = transform.position;
		if (dir > 0) {
			if (transform.position.x + dir * m_Speed * Time.deltaTime < m_EndX.position.x) {
				npos.x += dir * m_Speed * Time.deltaTime;
				transform.position = npos;
			} else {
				npos.x = m_EndX.position.x;
				transform.position = npos;
				dir = -1;
				Flip ();
			}
		} else {
			if (transform.position.x + 2*dir * m_Speed * Time.deltaTime > m_StartX.position.x) {
				npos.x += dir * m_Speed * Time.deltaTime;
				transform.position = npos;
			} else {
				npos.x = m_StartX.position.x;
				transform.position = npos;
				dir = 1;
				Flip ();
				m_IsAtStart = true;
			}
		}
	}
}

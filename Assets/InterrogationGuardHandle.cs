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
	[SerializeField] Interrogation m_ItrSceneManager;
	[SerializeField] InterrogationPrisonerHandler m_IPHandle;
	[SerializeField] InteractionSound m_ItrAudio;
	[SerializeField] Transform m_StartX;
	[SerializeField] Transform m_EndX;
	[SerializeField] GameObject m_Bottom;
	//[SerializeField] SpriteRenderer m_LeftArm;
	[SerializeField] float m_Speed;
	[SerializeField] float[] m_WaitTime;

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


	[SerializeField] Symbol[] m_ComboPool;
	[SerializeField] Sprite[] m_Symbols;
	Timer m_ComboTimer;

	// check key inputs 
	int m_CurrentKey = -1;



	// Use this for initialization
	void Awake () {
		
		m_Anim = GetComponent<Animator> ();
		//m_CurWaitTime = 2f;
		//m_StartTime = Time.time;
		m_BottomSpr = m_Bottom.GetComponentsInChildren<SpriteRenderer>();

		// max number 
		m_ComboTimer = new Timer (5f);
	}

	void Start(){
		m_GS = IG_GuardState.Idle;
		ChangeState ();
		m_ComboTimer.Reset ();
	}
	
	// Update is called once per frame
	void Update () {
		// 
		//Behaviour ();
		//CheckWalkPos ();
		if(Input.GetKeyDown(KeyCode.W)){
			if (!m_IsAnswer && (m_GS == IG_GuardState.Question || m_GS == IG_GuardState.PushQuestion)) {
				m_IsAnswer = true;
				print ("Answer!!!");
			}
		}

		//print ("Timer val: " + m_ComboTimer.TimeLeft);

		if (m_ComboTimer.IsOffCooldown) {
			GenerateCombo ();

		} else {
			if (m_CurrentKey == 4){
				//m_CurrentKey = 0;
				GenerateCombo ();
				//m_ComboTimer.Reset ();
			}
			if (Input.anyKeyDown) {
				print ("Check the current key input");
				if (Input.GetKeyDown (KeyCode.W)) {
					CheckKeyInput (3);
				} else if (Input.GetKeyDown (KeyCode.A)) {
					CheckKeyInput (1);
				} else if (Input.GetKeyDown (KeyCode.S)) {
					CheckKeyInput (0);
				} else if (Input.GetKeyDown (KeyCode.D)) {
					CheckKeyInput (2);
				} else {
					print ("#### Error input ####");
				}
			}

		}
	
	}

	// generate combination 
	void GenerateCombo(){
		int randomNum;
		//int preNum = 0;
		for (int i = 0; i < 4; i++) {
			//print ("##### " + i + " #####");
			randomNum = Random.Range (0, 4);
			//print (randomNum);
//			while(!m_ComboPool [i].SetSprite (m_Symbols [randomNum], randomNum)){
//				randomNum = Random.Range (0, 4);
//			}
//			i++;
			if (m_ComboPool [i].SetSprite (m_Symbols [randomNum], randomNum)) {
			} else {
				i -= 1;
				print ("Redo");
			}
		}
			
		m_ComboTimer.Reset ();
		m_CurrentKey = 0;
	}

	// check if using the correct keys 
	void CheckKeyInput(int keyIdx){
		if (m_CurrentKey > -1 && m_CurrentKey < 4) {
			print ("### " + m_ComboPool [m_CurrentKey].GetSymbolIdx ());
			if (keyIdx == m_ComboPool [m_CurrentKey].GetSymbolIdx () && !m_ComboPool [m_CurrentKey].IsUnclock) {
				m_ComboPool [m_CurrentKey].HitSymbol ();
				m_CurrentKey += 1;
			} else {
				print ("### Miss the symbol");
				foreach (Symbol sb in m_ComboPool) {
					sb.MissSymbol ();
				}
				//GenerateCombo ();
			}
		} 

	}


	// old code, with guard AI
	bool CheckStateEnd(){
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

	// check behaviour only when state change
	void ChangeState(){
		switch (m_GS) {
		case IG_GuardState.Idle:
			m_CurWaitTime = 2f;
			m_StartTime = Time.time;
			m_Anim.Play ("IG-Idle");
			break;
		case IG_GuardState.Question:
			m_CurWaitTime = 4f;
			m_StartTime = Time.time;
			m_Anim.Play ("IG-Question");
			m_ItrAudio.PlayInterroNicer ();
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

	public void EndInterrogation(){
		if (!m_IsEnd) {
			m_IsEnd = true;
		}
	}

	// checkeck which state to go to after the sequence ends 
	void EndSequence(){
		if (m_IsEnd) {
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
			if (transform.position.x + dir * m_Speed * Time.deltaTime > m_StartX.position.x) {
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

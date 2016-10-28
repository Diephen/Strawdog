using UnityEngine;
using System.Collections;

public class IterrogationGuardHandle : MonoBehaviour {
	enum IG_GuardState{
		Idle = 0,
		Question,           // waiting for answer when idling 
		PushQuestion,       // waiting for answer when pushing prisoner
		WalkToRight,
		WalkToLeft,
		BackToIdle,
		Engage,
		EndKick
	}
	IG_GuardState m_GS;
	Animator m_Anim;
	[SerializeField] Transform m_StartX;
	[SerializeField] Transform m_EndX;
	[SerializeField] GameObject m_Bottom;
	[SerializeField] SpriteRenderer m_LeftArm;
	[SerializeField] float m_Speed;
	[SerializeField] float[] m_WaitTime;

	SpriteRenderer[] m_BottomSpr;

	float dir = 1;
	float m_CurWaitTime = -1;
	float m_StartTime = -1;
	bool m_IsAnswer = false;
	bool m_IsWalk = false;
	bool m_IsAtStart = true;
	// Use this for initialization
	void Awake () {
		m_GS = IG_GuardState.Idle;
		m_Anim = GetComponent<Animator> ();
		m_CurWaitTime = 2f;
		m_StartTime = Time.time;
		//ChangeState ();
		m_BottomSpr = m_Bottom.GetComponentsInChildren<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		Behaviour ();
		//CheckWalkPos ();
		if(Input.GetKeyDown(KeyCode.S)){
			if (!m_IsAnswer) {
				m_IsAnswer = true;
				print ("Answer!!!");
			}
		}
	
	}

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
			break;
		case IG_GuardState.WalkToRight:
			//m_CurWaitTime = 3f;
			//m_StartTime = Time.time;
			m_Anim.Play ("IG-Walk");
			m_IsWalk = true;
			m_IsAtStart = false;
			break;
		case IG_GuardState.PushQuestion:
			m_CurWaitTime = -1f;
			m_StartTime = Time.time;
			m_Anim.Play ("IG-ForceRead");
			break;
		case IG_GuardState.BackToIdle:
			m_CurWaitTime = 2f;
			m_StartTime = Time.time;
			m_Anim.Play ("IG-BackToIdle");
			break;
		}
	}

	void Behaviour(){
		switch (m_GS) {
		case IG_GuardState.Idle:
			if (!CheckStateEnd()) {
				print ("Idle");
			} else {
				m_GS = IG_GuardState.Question;
				ChangeState ();
			}
			break;
		case IG_GuardState.Question:
			if (!CheckStateEnd ()) {
				if (m_IsAnswer) {
					m_GS = IG_GuardState.WalkToRight;
					ChangeState ();
					m_IsAnswer = false;
				} else {
					// question idling
					print("Question Idle");
				}
			} else {
				m_GS = IG_GuardState.PushQuestion;
				ChangeState ();
			}
			break;
		case IG_GuardState.WalkToRight:
			if (m_IsWalk && !m_IsAtStart) {
				CheckWalkPos ();
			} else {
				m_IsWalk = false;
				m_GS = IG_GuardState.Idle;
				ChangeState ();
			}
			break;
		case IG_GuardState.PushQuestion:
			if (m_IsAnswer) {
				m_GS = IG_GuardState.BackToIdle;
				ChangeState ();
				m_IsAnswer = false;
			} else {
				// question idling
				print("Pushing Question Idle");
			}
			break;
		case IG_GuardState.BackToIdle:
			if (CheckStateEnd ()) {
				m_GS = IG_GuardState.Idle;
				ChangeState ();
			}
			break;
		}


		
	}

	void HideLegSprites(){
		foreach (SpriteRenderer spr in m_BottomSpr) {
			spr.sortingOrder = spr.sortingOrder - 10;
		}
		m_LeftArm.sortingOrder = m_LeftArm.sortingOrder - 10;
	}

	void ShowLegSprites(){
		foreach (SpriteRenderer spr in m_BottomSpr) {
			spr.sortingOrder = spr.sortingOrder + 10;
		}
		m_LeftArm.sortingOrder = m_LeftArm.sortingOrder + 10;
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

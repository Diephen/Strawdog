﻿using UnityEngine;
using System.Collections;

public class PrisonerHandle : MonoBehaviour {
	[SerializeField] Animator m_PrisonerAnim;
	[SerializeField] GuardHandle m_GuardHandle;
	[SerializeField] AnimationControl m_AnimCtrl;
	//[SerializeField] Animator m_GuardAnim;

	private bool m_IsStartTorture = false;
 	
	void OnEnable ()
	{
		Events.G.AddListener<GuardEnteringCellEvent>(OnGuardEnterCell);
		Events.G.AddListener<GuardEngaginPrisonerEvent>(OnGuardEngagePrisoner);
	}

	void OnDisable ()
	{
		Events.G.RemoveListener<GuardEnteringCellEvent>(OnGuardEnterCell);
		Events.G.RemoveListener<GuardEngaginPrisonerEvent>(OnGuardEngagePrisoner);
	}

	void OnGuardEnterCell (GuardEnteringCellEvent e)
	{
		Debug.Log("Guard Entered, Activate Prisoner Control");
		// 


	}

	void OnGuardEngagePrisoner (GuardEngaginPrisonerEvent e)
	{
		if (e.Engaged) {
			Debug.Log ("G: Prisoner Engaged");
			StartTorture ();
		} else {
			Debug.Log ("Prisoner Disengaged");
		}
	}

	void ActivatePrisoner(){
		
	}


	void StartTorture(){
		if (m_PrisonerAnim != null && !m_IsStartTorture) {
			m_AnimCtrl.SetAnimation (true);
			m_PrisonerAnim.SetTrigger ("TriggerStartTorture");
			m_IsStartTorture = true;
		}
	}

	public void Torture(){
		m_PrisonerAnim.SetBool("IsTorture", true);
	}

	public void ReleaseTorture(){
		m_PrisonerAnim.SetBool ("IsTorture", false);
	}

	public void Resist(){
		// prisoner needs to be called too
		// m_PrisonerAnim.SetBool("IsTorture", true);
		// call prisoner
		m_PrisonerAnim.SetTrigger("TriggerResist");
		m_GuardHandle.PushBack ();
		//m_GuardAnim.SetTrigger ("TriggerBack");
	}

	public void ReleaseResist(){
	
	}

	public void Leave(){
		// D walk need to be called 
		// animation set back 
		// prisoner animation set back
		m_IsStartTorture = false;	
		m_AnimCtrl.SetAnimation(false);
		m_GuardHandle.LeaveCalledByPrisoner ();

	}

	public void Speak(){
		// not implemented yet 
		Debug.Log("Speak in torture");
	}

	void BackToIdle(){

	}
}

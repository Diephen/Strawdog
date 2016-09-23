﻿using UnityEngine;
using System.Collections;

public class GuardHandle : MonoBehaviour {
	[SerializeField] Animator m_GuardAnim;
	[SerializeField] AnimationControl m_AnimCtrl;

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
		Debug.Log("Guard Entered");

	}

	void OnGuardEngagePrisoner (GuardEngaginPrisonerEvent e)
	{
		if (e.Engaged) {
			Debug.Log ("P: Prisoner Engaged");
			StartTorture ();
		} else {
			Debug.Log ("P: Prisoner Disengaged");
		}
	}


	void StartTorture(){
		if (m_GuardAnim != null && !m_IsStartTorture) {
			m_AnimCtrl.SetAnimation (true);	
			m_IsStartTorture = true;
		}
	}
}

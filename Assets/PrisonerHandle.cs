using UnityEngine;
using System.Collections;

public class PrisonerHandle : MonoBehaviour {
	[SerializeField] Animator m_PrisonerAnim;
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
			m_IsStartTorture = true;
		}
	}
}

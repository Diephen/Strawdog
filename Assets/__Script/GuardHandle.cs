using UnityEngine;
using System.Collections;

public class GuardHandle : MonoBehaviour {
	[SerializeField] Animator m_GuardAnim;
	[SerializeField] PrisonerHandle m_PrisonerHandle;
	[SerializeField] AnimationControl m_AnimCtrl;
	[SerializeField] PuppetControl m_PC;
	[SerializeField] AudioSource m_DoorAudio;

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
		if (m_DoorAudio != null && !m_DoorAudio.isPlaying) {
			m_DoorAudio.Play ();
		}
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
			m_GuardAnim.SetTrigger ("TriggerStartTorture");
			m_IsStartTorture = true;
		}
	}

	public void Torture(){
		// prisoner needs to be called too
		m_GuardAnim.SetBool("IsTorture", true);
		m_PrisonerHandle.Torture ();
		// call prisoner
	}

	public void ReleaseTorture(){
		m_GuardAnim.SetBool("IsTorture", false);
		m_PrisonerHandle.ReleaseTorture ();
	}

	public void Leave(){
		// D walk need to be called 
		// animation set back 
		// prisoner animation set back
		m_AnimCtrl.SetAnimation(false);
		m_IsStartTorture = false;
		m_PrisonerHandle.LeaveCalledByGuard ();

	}
	public void LeaveCalledByPrisoner(){
		m_AnimCtrl.SetAnimation(false);
		m_IsStartTorture = false;
		m_PC.MoveRight ();
	}

	public void Speak(){
		// not implemented yet 
		Debug.Log("Speak in torture");
	}

	public void PushBack(){
		m_GuardAnim.SetTrigger ("TriggerBack");
	}
		
	void BackToIdle(){
		
	}
}

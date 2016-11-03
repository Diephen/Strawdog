using UnityEngine;
using System.Collections;

public class Cell_PrisonerHandle : MonoBehaviour {
	[SerializeField] Animator m_Anim;
	[SerializeField] GuardHandle m_GuardHandle;
	[SerializeField] AnimationControl m_AnimCtrl;
	[SerializeField] InteractionSound m_ItrAudio;
	[SerializeField] LightControl m_LightCtrl;
	[SerializeField] bool m_IsStartWithAnimation = false;

	[SerializeField] PuppetControl _prisonerPuppetControl;

//	bool m_isResisting = false;
//	bool m_isUnderTorture = false;
	//[SerializeField] Animator m_GuardAnim;

//	private bool m_IsStartTorture = false;

	void Start(){
		if (m_Anim == null && GetComponent<Animator> ()) {
			m_Anim = GetComponent<Animator> ();
		}
		if(m_IsStartWithAnimation){
			m_AnimCtrl.SetAnimation(true);
		}
	}
 	
	void OnEnable ()
	{
		Events.G.AddListener<LeftCellUnlockedEvent>(LeftCellUnlocked);
	}

	void OnDisable ()
	{
		Events.G.RemoveListener<LeftCellUnlockedEvent>(LeftCellUnlocked);
	}

//	void OnGuardEnterCell (GuardEnteringCellEvent e)
//	{
//		Debug.Log("Guard Entered, Activate Prisoner Control");
//		// 
//
//
//	}
//
//	public bool GetisUnderTorture(){
//		return m_isUnderTorture;
//	}

	void EnablePrisonerMovement(){
		_prisonerPuppetControl.EnableKeyInput ();
		m_AnimCtrl.SetAnimation (false);
	}

	void LeftCellUnlocked (LeftCellUnlockedEvent e)
	{
		print ("enable prisoner");
		m_Anim.Play ("p-jc-GetUp");
	}

//	void ActivatePrisoner(){
//		
//	}
//
//
//	void StartTorture(){
//		if (m_PrisonerAnim != null && !m_IsStartTorture) {
//			m_AnimCtrl.SetAnimation (true);
//			m_PrisonerAnim.SetTrigger ("TriggerStartTorture");
//			m_IsStartTorture = true;
//		}
//	}
//
//	public void Torture(){
//		m_isUnderTorture = true;
//		m_PrisonerAnim.SetBool("IsTorture", true);
//		if (m_isResisting) {
//			ReleaseResist ();
//		}
//		m_LightCtrl.TurnOnFlicker ();
//		//m_LightCtrl.ToggleSpotFlicker ();
//	}
//
//	public void ReleaseTorture(){
//		m_isUnderTorture = false;
//		m_PrisonerAnim.SetBool ("IsTorture", false);
//		//m_LightCtrl.ToggleSpotFlicker ();
//		m_LightCtrl.TurnOffFlicker();
//	}
//
//	public void Resist(){
//		// prisoner needs to be called too
//		// m_PrisonerAnim.SetBool("IsTorture", true);
//		// call prisoner
//		//m_PrisonerAnim.SetTrigger("TriggerResist");
//		if(!m_isUnderTorture){
//			m_PrisonerAnim.SetBool("IsResist", true);
//			//m_GuardHandle.PushBack ();
//			m_isResisting = true;
//			m_ItrAudio.PlayResist ();
//		}
//
//	}
//
//	public void ReleaseResist(){
//		m_isResisting = false;
//		m_PrisonerAnim.SetBool("IsResist", false);
//	
//	}
//
//	public void Leave(){
//		// D walk need to be called 
//		// animation set back 
//		// prisoner animation set back
//		if(m_isResisting){
//			m_PrisonerAnim.SetBool("IsResist", false);
//			m_IsStartTorture = false;	
//			m_AnimCtrl.SetAnimation(false);
//			m_GuardHandle.LeaveCalledByPrisoner ();
//			m_ItrAudio.PlayBreakOut ();
//			m_LightCtrl.TurnOffFlicker ();
//		}
//
//
//	}
//
//	public void DrownStruggle(float holdTime){
//		m_PrisonerAnim.SetFloat ("TortureHold", holdTime);
//	}
//
//	public void LeaveCalledByGuard(){
//		m_isResisting = false;
//		m_PrisonerAnim.SetBool("IsResist", false);
//		m_IsStartTorture = false;	
//		m_AnimCtrl.SetAnimation(false);
//		m_ItrAudio.PlayChain ();
//		//m_GuardHandle.LeaveCalledByPrisoner ();
//		m_LightCtrl.TurnOffFlicker ();
//	
//	}
//
//	public void DisableAnim(){
//		m_AnimCtrl.SetAnimation(false);
//	} 
//
//	public void Speak(){
//		// not implemented yet 
//		Debug.Log("Speak in torture");
//	}
//
//	void BackToIdle(){
//
//	}
}

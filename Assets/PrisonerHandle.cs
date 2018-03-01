using UnityEngine;
using System.Collections;

public class PrisonerHandle : MonoBehaviour {
	[SerializeField] PuppetControl m_PCtrl;
	[SerializeField] Animator m_PrisonerAnim;
	[SerializeField] GuardHandle m_GuardHandle;
	[SerializeField] AnimationControl m_AnimCtrl;
	[SerializeField] InteractionSound m_ItrAudio;
	[SerializeField] LightControl m_LightCtrl;
	[SerializeField] TextReaction[] m_DesText;
	bool _isfaded = false;

	bool m_isResisting = false;
	bool m_isUnderTorture = false;
	//[SerializeField] Animator m_GuardAnim;

	private bool m_IsStartTorture = false;

	void Awake(){
//		foreach (GameObject g in m_DesText) {
//			g.SetActive (false);
//		}
	}

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
//
//	public bool GetisUnderTorture(){
//		return m_isUnderTorture;
//	}


	void OnGuardEngagePrisoner (GuardEngaginPrisonerEvent e)
	{
		if (e.Engaged) {
			Debug.Log ("P: Prisoner Engaged");
			StartTorture ();
			// show text 
			_isfaded = false;
			foreach (TextReaction tr in m_DesText) {
				tr.TextFadeIn ();
			}

		} else {
			Debug.Log ("P:Prisoner Disengaged");
		}
	}

	void ActivatePrisoner(){
		
	}

	void LateUpdate(){
		if (m_IsStartTorture) {
			m_PrisonerAnim.SetBool("IsStartTorture", false);
		}

	}


	void StartTorture(){
		if (m_PrisonerAnim != null && !m_IsStartTorture) {
			m_AnimCtrl.SetAnimation (true);
			//m_PrisonerAnim.SetTrigger ("TriggerStartTorture");
			m_PrisonerAnim.Play ("TortureStart");
			//m_PrisonerAnim.SetBool("IsStartTorture", true);
			m_IsStartTorture = true;

		}
	}

	public void Torture(){
		m_isUnderTorture = true;

		m_PrisonerAnim.SetBool("IsTorture", true);
		if (m_isResisting) {
			ReleaseResist ();
		}
		m_LightCtrl.TurnOnFlicker ();
		//m_LightCtrl.ToggleSpotFlicker ();
	}

	public void ReleaseTorture(){
		m_isUnderTorture = false;

		m_PrisonerAnim.SetBool ("IsTorture", false);
		//m_LightCtrl.ToggleSpotFlicker ();
		m_LightCtrl.TurnOffFlicker();

		if (_isfaded) {
			_isfaded = false;
			foreach (TextReaction tr in m_DesText) {
				tr.TextFadeIn ();
			}
		}
	}

	public void Resist(){
		// prisoner needs to be called too
		// m_PrisonerAnim.SetBool("IsTorture", true);
		// call prisoner
		//m_PrisonerAnim.SetTrigger("TriggerResist");
		if(!m_isUnderTorture){
			m_PrisonerAnim.SetBool("IsResist", true);
			//m_GuardHandle.PushBack ();
			m_isResisting = true;
			m_ItrAudio.PlayResist ();
		}

	}

	public void ReleaseResist(){
		m_isResisting = false;
		m_PrisonerAnim.SetBool("IsResist", false);
	
	}

	public void Leave(){
		// D walk need to be called 
		// animation set back 
		// prisoner animation set back
		if(m_isResisting){
			
			m_PrisonerAnim.SetBool("IsResist", false);
			m_IsStartTorture = false;	

			m_GuardHandle.LeaveCalledByPrisoner ();
			m_ItrAudio.PlayBreakOut ();
			m_AnimCtrl.SetAnimation(false);
			m_LightCtrl.TurnOffFlicker ();
			// hide text
			_isfaded = true;
			foreach (TextReaction tr in m_DesText) {
				tr.TextFadeOut ();
			}
		}


	}

	public void DrownStruggle(float holdTime){
		m_PrisonerAnim.SetFloat ("TortureHold", holdTime);
		if (holdTime >= 2f) {
			_isfaded = true;
			foreach (TextReaction tr in m_DesText) {
				tr.TextFadeOut ();
			}
		}
	}

	public void LeaveCalledByGuard(){
		m_isResisting = false;
		m_PrisonerAnim.SetBool("IsResist", false);
		m_IsStartTorture = false;	
		m_AnimCtrl.SetAnimation(false);
		m_ItrAudio.PlayChain ();
		//m_GuardHandle.LeaveCalledByPrisoner ();
		m_LightCtrl.TurnOffFlicker ();
		// hide text
		_isfaded = true;
		foreach (TextReaction tr in m_DesText) {
			tr.TextFadeOut ();
		}
	
	}

	public void DisableAnim(){
		m_AnimCtrl.SetAnimation(false);
	} 

	public void Speak(){
		// not implemented yet 
		Debug.Log("Speak in torture");
	}

	void BackToIdle(){

	}

	public void ButtonPressed(int idx){
		m_DesText [idx].Tap ();
		
	}
}

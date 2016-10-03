using UnityEngine;
using System.Collections;

public class GuardHandle : MonoBehaviour {
	[SerializeField] Animator m_GuardAnim;
	[SerializeField] PrisonerHandle m_PrisonerHandle;
	[SerializeField] AnimationControl m_AnimCtrl;
	[SerializeField] PuppetControl m_PC;
	[SerializeField] AudioSource m_DoorAudio;
	[SerializeField] InteractionSound m_ItrAudio;
	[SerializeField] GameObject m_Bubbles;

	float m_AStartHoldTime = -1f;
	float m_AHoldTime = 0f;
	private bool m_IsStartTorture = false;
	private bool m_IsTorture = false;

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
			Debug.Log ("G: Prisoner Engaged");
			StartTorture ();
		} else {
			Debug.Log ("G: Prisoner Disengaged");
		}
	}

	void Update(){
		if (m_IsTorture) {
			m_AHoldTime = Time.time - m_AStartHoldTime;
			// set the hold time
			m_GuardAnim.SetFloat ("TortureHold", m_AHoldTime);
			m_PrisonerHandle.DrownStruggle (m_AHoldTime);
			if (m_AHoldTime >= 0.8f) {
				Debug.Log ("drowning");
				if (!m_Bubbles.activeSelf) {
					m_Bubbles.SetActive (true);
					m_ItrAudio.PlayDrown ();
				}

			}

			if (m_AHoldTime >= 5f) {
				Debug.Log ("Faint");
				//m_GuardAnim.SetTrigger ("TriggerFaint");
				if (m_Bubbles.activeSelf) {
					m_Bubbles.SetActive (false);
					m_ItrAudio.PlayFaint ();
				}

			}


		} else {
			m_AStartHoldTime = 0f;
//			if (m_AHoldTime > 0) {
//				m_AHoldTime -= Time.deltaTime;
//			} else {
//				m_AStartHoldTime = 0f;
//			}
//			m_GuardAnim.SetFloat ("TortureHold", m_AHoldTime);

		}
	}

	void StartTorture(){
		if (m_GuardAnim != null && !m_IsStartTorture) {
			Debug.Log ("Start Torture");
			m_AnimCtrl.SetAnimation (true);	
			m_GuardAnim.SetTrigger ("TriggerStartTorture");
			m_IsStartTorture = true;
		}
	}
		

	public void Torture(){
		// prisoner needs to be called too
		m_AStartHoldTime = Time.time;
		m_IsTorture = true;
		m_GuardAnim.SetBool("IsTorture", true);
		m_PrisonerHandle.Torture ();
		m_ItrAudio.PlayDunkIn ();
		// call prisoner
	}

	public void ReleaseTorture(){
		
		m_GuardAnim.SetBool("IsTorture", false);
		m_GuardAnim.SetFloat ("TortureHold", -1f);
		m_PrisonerHandle.ReleaseTorture ();
		if (m_IsTorture) {
			m_ItrAudio.PlayDunkOut ();
			if (m_Bubbles.activeSelf) {
				m_Bubbles.SetActive (false);
				m_ItrAudio.StopDrown ();
			}
			m_IsTorture = false;
		}



	}

	public void ReleasePrisoner(){
		m_PrisonerHandle.ReleaseTorture ();
	}

	public void Leave(){
		// D walk need to be called 
		// animation set back 
		// prisoner animation set back
		// m_AnimCtrl.SetAnimation(false);
		if(!m_IsTorture){
			m_IsStartTorture = false;
			m_GuardAnim.SetTrigger ("TriggerBack");
			m_PrisonerHandle.LeaveCalledByGuard ();
			m_PC.MoveRight ();
		}


	}

	public void LeaveCalledByPrisoner(){
		// m_AnimCtrl.SetAnimation(false);
		m_IsStartTorture = false;
		m_GuardAnim.SetTrigger ("TriggerBack");
		m_PC.MoveRight ();
	}


	public void DisableAnim(){
		m_AnimCtrl.SetAnimation(false);
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

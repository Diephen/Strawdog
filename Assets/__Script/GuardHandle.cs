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
	[SerializeField] LightControl m_LightCtrl;
	[SerializeField] AudioSource m_TortureAudio;

	[SerializeField] Fading m_Fading;
	[SerializeField] InteractionProgress m_ProgressBar;
	[SerializeField] TextReaction[] m_DesText;
	[SerializeField] TextReaction[] m_LeaveHint;
	Timer m_leaveTimer;

	float m_AStartHoldTime = -1f;
	float m_AHoldTime = 0f;
	private bool m_IsStartTorture = false;
	private bool m_IsTorture = false;
	private bool m_IsFaint = false;

	bool m_EngagedPrisoner = false;
	bool m_isFirstEngaged = false;

	void Awake(){
		m_leaveTimer = new Timer(5f);
	}

	void OnEnable ()
	{
		Events.G.AddListener<GuardEnteringCellEvent>(OnGuardEnterCell);
		Events.G.AddListener<GuardEngaginPrisonerEvent>(OnGuardEngagePrisoner);
		Events.G.AddListener<GuardLeavingCellEvent>(OnGuardLeaveCell);

	}

	void OnDisable ()
	{
		Events.G.RemoveListener<GuardEnteringCellEvent>(OnGuardEnterCell);
		Events.G.RemoveListener<GuardEngaginPrisonerEvent>(OnGuardEngagePrisoner);
		Events.G.RemoveListener<GuardLeavingCellEvent>(OnGuardLeaveCell);
	}

	void OnGuardEnterCell (GuardEnteringCellEvent e)
	{
		Debug.Log("Guard Entered");
		if (m_DoorAudio != null && !m_DoorAudio.isPlaying) {
			m_DoorAudio.Play ();
		}
	}

	void OnGuardLeaveCell (GuardLeavingCellEvent e){
		if (m_DoorAudio != null && !m_DoorAudio.isPlaying) {
			m_DoorAudio.Play ();
		}
		foreach (TextReaction tr in m_LeaveHint) {
			tr.Confirm();

		}


	}

	void OnGuardEngagePrisoner (GuardEngaginPrisonerEvent e)
	{
		if (e.Engaged) {
			Debug.Log ("G: Prisoner Engaged");
			StartTorture ();
			m_isFirstEngaged = true;
			//m_IsStartTorture = false;
			m_leaveTimer.Reset ();
			foreach (TextReaction tr in m_DesText) {
				tr.TextFadeIn ();
			}

		} else {
			Debug.Log ("G: Prisoner Disengaged");
		}
		//Checks if the guard engaged prisoner for fading out
		m_EngagedPrisoner = true;
		m_PC.StopWalkAudio ();
	}

	void Update(){
		if (m_IsTorture) {
			m_AHoldTime = Time.time - m_AStartHoldTime;
			// set the hold time
			m_GuardAnim.SetFloat ("TortureHold", m_AHoldTime);
			m_ProgressBar.IncTime (m_AHoldTime);
			m_PrisonerHandle.DrownStruggle (m_AHoldTime);
			if (m_AHoldTime >= 0.8f && !m_IsFaint) {
				Debug.Log ("drowning");
				if (!m_Bubbles.activeSelf) {
					m_Bubbles.SetActive (true);
					m_ItrAudio.PlayDrown ();
				}

			}

			if (m_AHoldTime >= 5f) {
				//m_GuardAnim.SetTrigger ("TriggerFaint");
				if(!m_IsFaint){
					if (m_Bubbles.activeSelf) {
						m_Bubbles.SetActive (false);
						Debug.Log ("Faint");
						m_ItrAudio.PlayFaint ();
						m_ProgressBar.IncTime (10f);
					}
					m_ItrAudio.StopDrown ();
					m_PC.DisableKeyInput ();
					m_LightCtrl.TurnOffFlicker ();
					m_IsFaint = true;
					//Fading to Black
					Events.G.Raise(new Act1EndedEvent());
					GameStateManager.gameStateManager._TorturedPrisoner = true;
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

		if (m_leaveTimer.IsOffCooldown && m_isFirstEngaged) {
			foreach (TextReaction tr in m_LeaveHint) {
				tr.TextFadeIn ();
				tr.Blink ();
			}
			m_isFirstEngaged = false;
		}
	}

	void StartTorture(){
		if (m_GuardAnim != null && !m_IsStartTorture) {
			Debug.Log ("Start Torture");
			m_AnimCtrl.SetAnimation (true);	
			//m_GuardAnim.SetTrigger ("TriggerStartTorture");
			m_GuardAnim.SetBool("IsBack", false);
			m_IsStartTorture = true;
			Events.G.Raise (new UIProgressBar (true));
			Events.G.Raise (new DisableMoveEvent (CharacterIdentity.Guard));
			StartCoroutine (m_ProgressBar.FadeIn (3f));
		}
	}
		

	public void Torture(){
		// prisoner needs to be called too
		m_AStartHoldTime = Time.time;
		m_IsTorture = true;
		m_GuardAnim.SetBool("IsTorture", true);
		m_PrisonerHandle.Torture ();
		m_ItrAudio.PlayDunkIn ();
		if (m_TortureAudio != null) {
			m_TortureAudio.Play ();
		}
		// call prisoner
	}

	public void ReleaseTorture(){
		m_GuardAnim.SetBool("IsTorture", false);
		m_GuardAnim.SetFloat ("TortureHold", -1f);
		if (!m_IsFaint) {
			m_ProgressBar.IncTime (0f);
		}
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
		foreach (TextReaction tr in m_DesText) {
			tr.TextFadeOut ();
		}
	}

	public void Leave(){
		// D walk need to be called 
		// animation set back 
		// prisoner animation set back
		// m_AnimCtrl.SetAnimation(false);
		if(!m_IsTorture){
			m_IsStartTorture = false;
			//m_GuardAnim.SetTrigger ("TriggerBack");
			m_GuardAnim.SetBool("IsBack", true);
			m_PrisonerHandle.LeaveCalledByGuard ();
			m_PC.MoveRight ();
			Events.G.Raise (new UIProgressBar (false));
			//StartCoroutine (m_ProgressBar.FadeOut (2f));
		}
		foreach (TextReaction tr in m_DesText) {
			tr.TextFadeOut ();

		}


	}

	public void LeaveCalledByPrisoner(){
		// m_AnimCtrl.SetAnimation(false);
		m_IsStartTorture = false;
		//m_GuardAnim.SetTrigger ("TriggerBack");
		m_GuardAnim.SetBool("IsBack", true);
		m_PC.MoveRight ();
		StartCoroutine (m_ProgressBar.FadeOut (2f));
		Events.G.Raise (new UIProgressBar (false));
		foreach (TextReaction tr in m_DesText) {
			tr.TextFadeOut ();
		}
	}


	public void DisableAnim(){
		m_AnimCtrl.SetAnimation(false);
		m_GuardAnim.SetBool("IsBack", false);
		Events.G.Raise (new EnableMoveEvent (CharacterIdentity.Guard));
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

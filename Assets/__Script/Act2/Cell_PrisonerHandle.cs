using UnityEngine;
using System.Collections;

public class Cell_PrisonerHandle : MonoBehaviour {
	[SerializeField] Animator m_Anim;
	[SerializeField] GuardHandle m_GuardHandle;
	[SerializeField] AnimationControl m_AnimCtrl;
	[SerializeField] InteractionSound m_ItrAudio;
	[SerializeField] LightControl m_LightCtrl;
	[SerializeField] bool m_IsStartWithAnimation = false;
	[SerializeField] BoxCollider2D m_StopRightForPrisoner;
	[SerializeField] BoxCollider2D m_StopLeftForGuard;

	[SerializeField] PuppetControl _prisonerPuppetControl;

	[SerializeField] Transform m_SleepPos;

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
		if (m_StopRightForPrisoner != null) {
			m_StopRightForPrisoner.enabled = false;
		}
	}
 	
	void OnEnable ()
	{
		Events.G.AddListener<LeftCellUnlockedEvent>(LeftCellUnlocked);
		Events.G.AddListener<SleepInCellEvent> (SleepInCell);
	}

	void OnDisable ()
	{
		Events.G.RemoveListener<LeftCellUnlockedEvent>(LeftCellUnlocked);
		Events.G.RemoveListener<SleepInCellEvent> (SleepInCell);
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
		// collider fix 
		if(m_StopLeftForGuard!= null){
			m_StopLeftForGuard.enabled = false;
		}

		if (m_StopRightForPrisoner != null) {
			m_StopRightForPrisoner.enabled = true;
		}


	}

	void SleepInCell(SleepInCellEvent e){
		transform.position = m_SleepPos.position;
		m_AnimCtrl.SetAnimation(true);
		m_Anim.Play("p-jc-Sleep");
	}


}

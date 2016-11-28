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
	bool m_IsCaught = false;
	bool m_IsSleep = false;

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
		Events.G.AddListener<DragPrisonerInJail> (OnDragged);
		Events.G.AddListener<PrisonerEncounterSoldierExplore> (OnEncounterSoldier);
		Events.G.AddListener<Act2_SoldierDragPrisonerExplore> (OnDraggedExplore);
	}

	void OnDisable ()
	{
		Events.G.RemoveListener<LeftCellUnlockedEvent>(LeftCellUnlocked);
		Events.G.RemoveListener<SleepInCellEvent> (SleepInCell);
		Events.G.RemoveListener<DragPrisonerInJail> (OnDragged);
		Events.G.RemoveListener<PrisonerEncounterSoldierExplore> (OnEncounterSoldier);
		Events.G.RemoveListener<Act2_SoldierDragPrisonerExplore> (OnDraggedExplore);
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

	void OnDragged(DragPrisonerInJail e){
		m_Anim.Play ("p-jc-DragUp");
	}

	void OnCallGuard(){
		Events.G.Raise (new CallGuardInCell ());
	}

	void SleepInCell(SleepInCellEvent e){
		if (!m_IsSleep) {
			m_IsSleep = true;
			_prisonerPuppetControl.DisableKeyInput ();
			transform.position = m_SleepPos.position;
			m_AnimCtrl.SetAnimation(true);
			m_Anim.Play("p-jc-Sleep");
		}
	}

	void OnEncounterSoldier(PrisonerEncounterSoldierExplore e){
		//m_Anim.Play ("p-jc-Sleep");
		Events.G.Raise (new DisableMoveEvent (CharacterIdentity.Prisoner));
	}

	void OnDraggedExplore(Act2_SoldierDragPrisonerExplore e){
		print ("P Hands UP");
		m_AnimCtrl.SetAnimation(true);
		m_Anim.Play ("p-exp-GetCaught");
		_prisonerPuppetControl.DisableKeyInput ();
	}


}

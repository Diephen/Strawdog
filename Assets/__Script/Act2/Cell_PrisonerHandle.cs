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


}

﻿using UnityEngine;
using System.Collections;

public class AnimationInjectionEncounter : AnimationInjectionBase {
	[SerializeField] PuppetControl _puppetControl;
	[SerializeField] GuardEncounterHandle m_GuardHandle;
	[SerializeField] PrisonerEncounterHandle m_PrisonerHandle;
	private bool isEngaged = false;

	public void SetEngage(){
		if (!isEngaged) {
			isEngaged = true;
			CheckInteractionState ();
		}
	}

	public void SetLeave(){
		if (isEngaged) {
			isEngaged = false;
			CheckInteractionState ();
		}
	}

	void CheckInteractionState(){
		if (isEngaged) {
			state = 1;
			for (int i = 0; i < _animInject.Length; i++) {
				_tempStateHandling [i] = _puppetControl._stateHandling [i];
				_puppetControl._stateHandling [i] = _animInject [i];
			}
		} else {
			state = 0;
			for (int i = 0; i < _animInject.Length; i++) {
				_puppetControl._stateHandling [i] = _tempStateHandling [i];
			}
		}
	}

	void Start(){
		//m_GuardHandle = GetComponent<GuardEncounterHandle> ();
	}

	void Update(){

	}

	//Have public scripts that will be called in place of the original function
	// each of these scripts are containers for different scripts that will be
	// swapped based on the state of the interaction

	//	protected override void PickUpPressed(PickUpPressedEvent e){
	//		base.PickUpPressed(e);
	//	} 
	//		
	//	protected override void PickupReleased(PickupReleasedEvent e){
	//		base.PickupReleased (e);
	//	}

	protected override void CrouchPressed(CrouchPressedEvent e){
		base.CrouchPressed (e);
		if (e.WhoAmI == CharacterIdentity.Guard) {
			Debug.Log ("Shoot");
			m_GuardHandle.Shoot ();
		}


	}
	//	protected override void WalkLeft(){
	//
	//	}
	//	protected override void WalkRight(){
	//
	//	}
	protected override void APressed(APressedEvent e){
		base.APressed (e);
		if (e.WhoAmI == CharacterIdentity.Guard) {
			Debug.Log ("Hold Gun");
			m_GuardHandle.HandUp ();
			// enable collider box 
		}

	}
	protected override void AReleased(AReleasedEvent e){
		base.AReleased (e);
		if (e.WhoAmI == CharacterIdentity.Guard) {
			Debug.Log ("Release Gun");
			m_GuardHandle.HandDown ();
			// disable collider 

		}



	}
	//	protected override void SPressed(){
	//	}
	//	protected override void SReleased(){
	//	}
	protected override void DPressed(DPressedEvent e){
		base.DPressed (e);
		//m_GuardHandle.LeaveDog ();
	}
	//
	protected override void DHold(DHoldEvent e){
		// choose to leave 
		base.DHold(e);
		//m_GuardHandle.LeaveDog ();
	}
	//
	//	protected override void DReleased(){
	//		
	//	}
}

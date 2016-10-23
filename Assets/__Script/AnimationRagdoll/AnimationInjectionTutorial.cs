using UnityEngine;
using System.Collections;

public class AnimationInjectionTutorial : AnimationInjectionBase {
	[SerializeField] PuppetControl _puppetControl;
	[SerializeField] GuardTutorialHandle m_GuardHandle;

//	[SerializeField] bool[] _animInject = new bool[11];
//	[SerializeField] GuardTutorialHandle m_guard;
//	bool[] _tempStateHandling = new bool[11];
//	int state = 0;
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
		m_GuardHandle = GetComponent<GuardTutorialHandle> ();
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

//	protected override void CrouchPressed(){
//	}
//	protected override void WalkLeft(){
//
//	}
//	protected override void WalkRight(){
//
//	}
	protected override void APressed(APressedEvent e){
		base.APressed (e);
		Debug.Log ("PetDog");
		m_GuardHandle.PetDog ();

	}
	protected override void AReleased(AReleasedEvent e){
		base.AReleased (e);
		Debug.Log ("Release Dog");
		m_GuardHandle.ReleasePet ();

	}
//	protected override void SPressed(){
//	}
//	protected override void SReleased(){
//	}
	protected override void DPressed(DPressedEvent e){
		base.DPressed (e);
		m_GuardHandle.LeaveDog ();
	}
//
	protected override void DHold(DHoldEvent e){
		// choose to leave 
		base.DHold(e);
		m_GuardHandle.LeaveDog ();
	}
//
//	protected override void DReleased(){
//		
//	}
}

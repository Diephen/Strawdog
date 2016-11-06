using UnityEngine;
using System.Collections;

public class AnimationInjectionExecution : AnimationInjectionBase {
	[SerializeField] PuppetControl _puppetControl;
	[SerializeField] ExecutionGuardHandle m_GuardHandle;
	[SerializeField] ExecutionPrisonerHandle m_PrisonerHandle;
	[SerializeField] Act4_GuardTrigger m_GuardTrigger;
	private bool isEngaged = false;
	//private bool isPrisonerDead = false;

//	void OnEnable(){
//		Events.G.AddListener<PickUpPressedEvent>(PickUpPressed);
//		Events.G.AddListener<PickupReleasedEvent>(PickupReleased);
//		Events.G.AddListener<CrouchPressedEvent>(CrouchPressed);
//		Events.G.AddListener<WalkLeftEvent>(WalkLeft);
//		Events.G.AddListener<WalkRightEvent>(WalkRight);
//		Events.G.AddListener<APressedEvent>(APressed);
//		Events.G.AddListener<AReleasedEvent>(AReleased);
//		Events.G.AddListener<SPressedEvent>(SPressed);
//		Events.G.AddListener<SReleasedEvent>(SReleased);
//		Events.G.AddListener<DPressedEvent>(DPressed);
//		Events.G.AddListener<DHoldEvent>(DHold);
//		Events.G.AddListener<DReleasedEvent>(DReleased);
//
//	}
//
//	void OnDisable(){
//		Events.G.RemoveListener<PickUpPressedEvent>(PickUpPressed);
//		Events.G.RemoveListener<PickupReleasedEvent>(PickupReleased);
//		Events.G.RemoveListener<CrouchPressedEvent>(CrouchPressed);
//		Events.G.RemoveListener<WalkLeftEvent>(WalkLeft);
//		Events.G.RemoveListener<WalkRightEvent>(WalkRight);
//		Events.G.RemoveListener<APressedEvent>(APressed);
//		Events.G.RemoveListener<AReleasedEvent>(AReleased);
//		Events.G.RemoveListener<SPressedEvent>(SPressed);
//		Events.G.RemoveListener<SReleasedEvent>(SReleased);
//		Events.G.RemoveListener<DPressedEvent>(DPressed);
//		Events.G.RemoveListener<DHoldEvent>(DHold);
//		Events.G.RemoveListener<DReleasedEvent>(DReleased);
//	}



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
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//	protected override void PickUpPressed(PickUpPressedEvent e){
	//		base.PickUpPressed(e);
	//	} 
	//		
	//	protected override void PickupReleased(PickupReleasedEvent e){
	//		base.PickupReleased (e);
	//	}

	protected override void CrouchPressed(CrouchPressedEvent e){
		base.CrouchPressed(e);
		if (e.WhoAmI == CharacterIdentity.Guard) {
			//m_GuardHandle.Shoot ();
			m_GuardTrigger.Shoot();
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
		if (e.WhoAmI == CharacterIdentity.Prisoner) {
			m_PrisonerHandle.Struggle (true);
		} else {
			//m_GuardHandle.HandUp ();
			m_GuardTrigger.HandUp();
		}


	}
	protected override void AReleased(AReleasedEvent e){
		base.AReleased (e);
		if (e.WhoAmI == CharacterIdentity.Prisoner) {
			
		} else {
			//m_GuardHandle.HandDown ();
			m_GuardTrigger.HandDown ();
		}


	}
	//	protected override void SPressed(){
	//	}
	//	protected override void SReleased(){
	//	}
	protected override void DPressed(DPressedEvent e){
		base.DPressed (e);
		if (e.WhoAmI == CharacterIdentity.Prisoner) {
			m_PrisonerHandle.Struggle (false);
		} else {
			//m_GuardHandle.HandDown ();
		}
	}
	//
	protected override void DHold(DHoldEvent e){
		// choose to leave 
		base.DHold(e);

	}
	//
	//	protected override void DReleased(){
	//		
	//	}
}

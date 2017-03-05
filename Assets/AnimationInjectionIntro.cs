using UnityEngine;
using System.Collections;
using System.IO;

public class AnimationInjectionIntro : AnimationInjectionBase {
	[SerializeField] PuppetControl _puppetControl;
	[SerializeField] GuardIntroHandle m_GuardHandle;
	[SerializeField] PrisonerIntroHandle m_PrisonerHandle;
	private bool isEngaged = false;
	private bool isGuardHandUp = false;
	private bool isGuardReady = false;
	private bool isPrisonerReady = false;
	private bool isAwayTogether = false;

	Timer _endIntroTimer = new Timer(0.5f);
	//private bool isPrisonerDead = false;

	void OnEnable(){
		Events.G.AddListener<EncountEndStateEvent>(OnUpdateEndingSate);
		Events.G.AddListener<PickUpPressedEvent>(PickUpPressed);
		Events.G.AddListener<PickupReleasedEvent>(PickupReleased);
		Events.G.AddListener<CrouchPressedEvent>(CrouchPressed);
		Events.G.AddListener<WalkLeftEvent>(WalkLeft);
		Events.G.AddListener<WalkRightEvent>(WalkRight);
		Events.G.AddListener<APressedEvent>(APressed);
		Events.G.AddListener<AReleasedEvent>(AReleased);
		Events.G.AddListener<SPressedEvent>(SPressed);
		Events.G.AddListener<SReleasedEvent>(SReleased);
		Events.G.AddListener<DPressedEvent>(DPressed);
		Events.G.AddListener<DHoldEvent>(DHold);
		Events.G.AddListener<DReleasedEvent>(DReleased);
	}

	void OnDisable(){
		Events.G.RemoveListener<EncountEndStateEvent>(OnUpdateEndingSate);
		Events.G.RemoveListener<PickUpPressedEvent>(PickUpPressed);
		Events.G.RemoveListener<PickupReleasedEvent>(PickupReleased);
		Events.G.RemoveListener<CrouchPressedEvent>(CrouchPressed);
		Events.G.RemoveListener<WalkLeftEvent>(WalkLeft);
		Events.G.RemoveListener<WalkRightEvent>(WalkRight);
		Events.G.RemoveListener<APressedEvent>(APressed);
		Events.G.RemoveListener<AReleasedEvent>(AReleased);
		Events.G.RemoveListener<SPressedEvent>(SPressed);
		Events.G.RemoveListener<SReleasedEvent>(SReleased);
		Events.G.RemoveListener<DPressedEvent>(DPressed);
		Events.G.RemoveListener<DHoldEvent>(DHold);
		Events.G.RemoveListener<DReleasedEvent>(DReleased);
	}

	void OnUpdateEndingSate(EncountEndStateEvent e){
		if (e.WhoAmI == CharacterIdentity.Guard) {
			isGuardReady = e.IsReady;
		}
		if (e.WhoAmI == CharacterIdentity.Prisoner) {
			isPrisonerReady = e.IsReady;
		}
	}

	public void PEndState(bool istrue){
		isPrisonerReady = istrue;
	}

	public void GEndState(bool istrue){
		isGuardReady = istrue;
	}

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
		if (!isAwayTogether && isGuardReady && isPrisonerReady) {
			isAwayTogether = true;
			_endIntroTimer.Reset ();
		}

		if (isAwayTogether && _endIntroTimer.IsOffCooldown) {
			Events.G.Raise (new LoadTitleCardEvent (0));
			isGuardReady = false;
		}
		if(!isGuardReady || !isPrisonerReady) {
			isAwayTogether = false;
		}
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
	}

	//	protected override void WalkLeft(){
	//
	//	}
	//	protected override void WalkRight(){
	//
	//	}
	protected override void APressed(APressedEvent e){
		base.APressed (e);



	}
	protected override void AReleased(AReleasedEvent e){
		base.AReleased (e);




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
using UnityEngine;
using System.Collections;

public class AnimationInjectionEncounter : AnimationInjectionBase {
	[SerializeField] PuppetControl _puppetControl;
	[SerializeField] GuardEncounterHandle m_GuardHandle;
	[SerializeField] PrisonerEncounterHandle m_PrisonerHandle;
	private bool isEngaged = false;
	private bool isShot = false;
	private bool isGuardHandUp = false;
	private bool isGuardReady = false;
	private bool isPrisonerReady = false;
	private bool isAwayTogether = false;
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
		print ("P: " + isPrisonerReady);
		print ("Away?" + isAwayTogether);
	}

	public void GEndState(bool istrue){
		isGuardReady = istrue;
		print ("G: " + isGuardReady);
		print ("Away?" + isAwayTogether);
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
		print ("P: " + isPrisonerReady + " G: " + isGuardReady);
		if (!isAwayTogether && isGuardReady && isPrisonerReady) {
			Debug.Log ("Away together");
			isAwayTogether = true;
			StartCoroutine (m_GuardHandle.Hug ());
//			m_GuardHandle.Hug ();
//			m_PrisonerHandle.Hug ();
			// raise event
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
		if (!isAwayTogether && !isShot) {
			if (e.WhoAmI == CharacterIdentity.Guard) {
				if (isGuardHandUp) {
					Debug.Log ("Shoot");
					m_GuardHandle.Shoot ();
					isShot = true;
				} 

			}
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
		if (!isAwayTogether && e.WhoAmI == CharacterIdentity.Guard) {
			Debug.Log ("Hold Gun");
			m_GuardHandle.HandUp ();
			isGuardHandUp = true;
			isGuardReady = false;
			// enable collider box 
		}
			

	}
	protected override void AReleased(AReleasedEvent e){
		base.AReleased (e);
		if (!isAwayTogether && e.WhoAmI == CharacterIdentity.Guard) {
			Debug.Log ("Release Gun");
			m_GuardHandle.HandDown ();
			isGuardHandUp = false;
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

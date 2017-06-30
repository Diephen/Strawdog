using UnityEngine;
using System.Collections;

public class AnimationInjection : MonoBehaviour {
	[SerializeField] PuppetControl _puppetControl;
	[SerializeField] bool[] _animInject = new bool[11];
	[SerializeField] GuardHandle m_guard;
	[SerializeField] PrisonerHandle m_prisoner;
	bool[] _tempStateHandling = new bool[11];
	int state = 0;

	Timer _intervalTimer;
	bool _IsResist = false;

	bool _isGuardTorturing = false;       // if the guard is doing the torture action 

	// Use this for initialization
	void Start () {
		_intervalTimer = new Timer (0.5f);
	}

	void Update(){
		//Debug.Log ("Torturing?? " + _isGuardTorturing);	
		CheckResist();
	}

	void OnEnable ()
	{
		Events.G.AddListener<GuardEnteringCellEvent>(OnGuardEnterCell);
		Events.G.AddListener<GuardEngaginPrisonerEvent>(OnGuardEngagePrisoner);

		// all the events for controls
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

	void OnDisable ()
	{
		Events.G.RemoveListener<GuardEnteringCellEvent>(OnGuardEnterCell);
		Events.G.RemoveListener<GuardEngaginPrisonerEvent>(OnGuardEngagePrisoner);

		// for all the controls 
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

	// resist: checking if press two buttons 

	void CheckResist(){
		if (_IsResist && _intervalTimer.IsOffCooldown) {
			_IsResist = false;
			m_prisoner.ReleaseResist ();
		}

	}

	void OnGuardEnterCell (GuardEnteringCellEvent e)
	{
		state = 0;
	}

	void OnGuardEngagePrisoner (GuardEngaginPrisonerEvent e)
	{
		if (e.Engaged) {
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

	//Have public scripts that will be called in place of the original function
	// each of these scripts are containers for different scripts that will be
	// swapped based on the state of the interaction

	void PickUpPressed(PickUpPressedEvent e){
	}
	void PickupReleased(PickupReleasedEvent e){
	}
	void CrouchPressed(CrouchPressedEvent e){
	}
	void WalkLeft(WalkLeftEvent e){
		//Jung-Ho: Let me know if you need to use this
	}
	void WalkRight(WalkRightEvent e){
		//Jung-Ho: Let me know if you need to use this
	}
	void APressed(APressedEvent e){
		if (e.WhoAmI == CharacterIdentity.Guard) {
			// guard torture
			Debug.Log("Call Torture");
			m_guard.Torture();
		} else {
			//if continue tapping 
			m_prisoner.ButtonPressed(0);
			if (!_IsResist && _intervalTimer.IsOffCooldown) {
				_IsResist = true;
				_intervalTimer.Reset ();
				m_prisoner.Resist ();
			} else if (_IsResist && !_intervalTimer.IsOffCooldown) {
				_intervalTimer.Reset ();
			} 
		}


	}
	void AReleased(AReleasedEvent e){
		if (e.WhoAmI == CharacterIdentity.Guard) {
			m_guard.ReleaseTorture ();
		} else {
			//m_prisoner.ReleaseResist ();
		}
	}
	void SPressed(SPressedEvent e){
	}
	void SReleased(SReleasedEvent e){
	}
	void DPressed(DPressedEvent e){
		// choose to leave 

		if (e.WhoAmI != CharacterIdentity.Guard) {
			m_prisoner.ButtonPressed(1);
			if (!_IsResist && _intervalTimer.IsOffCooldown) {
				_IsResist = true;
				_intervalTimer.Reset ();
				m_prisoner.Resist ();
			} else if (_IsResist && !_intervalTimer.IsOffCooldown) {
				_intervalTimer.Reset ();
			} 
		} else {
			
		}
	}

	void DHold(DHoldEvent e){
		// choose to leave 
		if (e.WhoAmI == CharacterIdentity.Guard) {
			// guard torture
			m_guard.Leave();
			//_puppetControl.MoveRight ();
		}
	}

	void DReleased(DReleasedEvent e){
		if (e.WhoAmI == CharacterIdentity.Guard) {
			// guard torture
			// m_guard.Torture();
		} else {
			//m_prisoner.ReleaseResist ();
		}
	}
}
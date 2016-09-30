using UnityEngine;
using System.Collections;

public class AnimationInjection : MonoBehaviour {
	[SerializeField] bool _isGuard;
	[SerializeField] PuppetControl _puppetControl;
	[SerializeField] bool[] _animInject = new bool[11];
	[SerializeField] GuardHandle m_guard;
	[SerializeField] PrisonerHandle m_prisoner;
	bool[] _tempStateHandling = new bool[11];
	int state = 0;

	bool _isGuardTorturing = false;       // if the guard is doing the torture action 

	// Use this for initialization
	void Start () {
	}

	void Update(){
		Debug.Log ("Torturing?? " + _isGuardTorturing);	
	}

	void OnEnable ()
	{
		Events.G.AddListener<GuardEnteringCellEvent>(OnGuardEnterCell);
		Events.G.AddListener<GuardEngaginPrisonerEvent>(OnGuardEngagePrisoner);
	}

	void OnDisable ()
	{
		Events.G.RemoveListener<GuardEnteringCellEvent>(OnGuardEnterCell);
		Events.G.RemoveListener<GuardEngaginPrisonerEvent>(OnGuardEngagePrisoner);
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

	public void PickUpPressed(){
		if (_isGuard) {

		} else {

		}
	}
	public void PickupReleased(){
	}
	public void CrouchPressed(){
	}
	public void WalkLeft(){

	}
	public void WalkRight(){
		if (_isGuard) {

		} else {

		}
	}
	public void APressed(){
		if (_isGuard) {
			// guard torture
			m_guard.Torture();
		} else {
			m_prisoner.Resist ();
		}

	}
	public void AReleased(){
		if (_isGuard) {
			m_guard.ReleaseTorture ();
		} else {
			m_prisoner.ReleaseResist ();
		}
	}
	public void SPressed(){
	}
	public void SReleased(){
	}
	public void DPressed(){
		// choose to leave 
		if (!_isGuard)  {
			m_prisoner.Resist ();
		}
	}

	public void DHold(){
		// choose to leave 
		if (_isGuard) {
			// guard torture
			m_guard.Leave();
			_puppetControl.MoveRight ();
		}
	}

	public void DReleased(){
		if (_isGuard) {
			// guard torture
			// m_guard.Torture();
		} else {
			m_prisoner.ReleaseResist ();
		}
	}
}
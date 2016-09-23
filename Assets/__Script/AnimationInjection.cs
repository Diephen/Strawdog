using UnityEngine;
using System.Collections;

public class AnimationInjection : MonoBehaviour {
	[SerializeField] PuppetControl _puppetControl;
	[SerializeField] bool[] _animInject = new bool[11];
	int state = 0;

	// Use this for initialization
	void Start () {
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
				_puppetControl._stateHandling [i] = _animInject [i];
			}
		} else {
			state = 0;
			for (int i = 0; i < _animInject.Length; i++) {
				_puppetControl._stateHandling [i] = true;
			}
		}
	}
		
		//Have public scripts that will be called in place of the original function
		// each of these scripts are containers for different scripts that will be
		// swapped based on the state of the interaction

	public void PickUpPressed(){
	}
	public void PickupReleased(){
	}
	public void CrouchPressed(){
	}
	public void WalkLeft(){
	}
	public void WalkRight(){
	}
	public void APressed(){
	}
	public void AReleased(){
	}
	public void SPressed(){
	}
	public void SReleased(){
	}
	public void DPressed(){
	}
	public void DReleased(){
	}
}

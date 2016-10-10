using UnityEngine;
using System.Collections;

public class AnimationInjectionTutorial : MonoBehaviour {
	[SerializeField] PuppetControl _puppetControl;
	[SerializeField] bool[] _animInject = new bool[11];
	[SerializeField] GuardTutorialHandle m_guard;
	bool[] _tempStateHandling = new bool[11];
	int state = 0;


	// Use this for initialization
	void Start () {
	}

	void Update(){
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

	public void DHold(){
		// choose to leave 
	
	}

	public void DReleased(){
		
	}
}

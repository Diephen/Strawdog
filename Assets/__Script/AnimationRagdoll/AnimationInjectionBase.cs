using UnityEngine;
using System.Collections;

public class AnimationInjectionBase : MonoBehaviour {
	//[SerializeField] PuppetControl _puppetControl;
	[SerializeField] protected bool[] _animInject = new bool[11];
	protected bool[] _tempStateHandling = new bool[11];
	protected int state = 0;


	void OnEnable ()
	{
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
		

	//Have public scripts that will be called in place of the original function
	// each of these scripts are containers for different scripts that will be
	// swapped based on the state of the interaction

	protected virtual void PickUpPressed(PickUpPressedEvent e){
	}
	protected virtual void PickupReleased(PickupReleasedEvent e){
	}
	protected virtual void CrouchPressed(CrouchPressedEvent e){
	}
	protected virtual void WalkLeft(WalkLeftEvent e){
		//Jung-Ho: Let me know if you need to use this
	}
	protected virtual void WalkRight(WalkRightEvent e){
		//Jung-Ho: Let me know if you need to use this
	}
	protected virtual void APressed(APressedEvent e){
		//Debug.Log ("[Base] A Press");
	}
	protected virtual void AReleased(AReleasedEvent e){
	}
	protected virtual void SPressed(SPressedEvent e){
	}
	protected virtual void SReleased(SReleasedEvent e){
	}
	protected virtual void DPressed(DPressedEvent e){
	}

	protected virtual void DHold(DHoldEvent e){
	}

	protected virtual void DReleased(DReleasedEvent e){
	}
	// Use this for initialization
	protected virtual void Start () {
	}
	
	// Update is called once per frame
	protected virtual void Update () {
	
	}
}

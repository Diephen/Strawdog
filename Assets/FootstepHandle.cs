using UnityEngine;
using System.Collections;

public class FootstepHandle : MonoBehaviour {
	[SerializeField] AudioClip _guardfootStep;
	[SerializeField] PuppetControl _guardPuppetControl;

	[SerializeField] AudioClip _prisonerfootStep;
	[SerializeField] PuppetControl _prisonerPuppetControl;

	void OnTriggerEnter2D(Collider2D other){
		//Debug.Log ("Footstep "+ other.tag);
		if (other.tag == "Guard") {
			_guardPuppetControl.SwitchWalk (_guardfootStep);
		}
		else if (other.tag == "Prisoner") {
			_prisonerPuppetControl.SwitchWalk (_prisonerfootStep);
		}
	}
}

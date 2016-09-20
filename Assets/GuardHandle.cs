using UnityEngine;
using System.Collections;

public class GuardHandle : MonoBehaviour {

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
		Debug.Log("Guard Entered");
	}

	void OnGuardEngagePrisoner (GuardEngaginPrisonerEvent e)
	{
		if (e.Engaged) {
			Debug.Log ("Prisoner Engaged");
		} else {
			Debug.Log ("Prisoner Disengaged");
		}
	}
}

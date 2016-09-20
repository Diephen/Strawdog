using UnityEngine;
using System.Collections;

public class GuardTrigger : MonoBehaviour {
	enum guardState {BeforeEntering, EnteredCell, EngagedPrisoner};
	guardState _guardState = guardState.BeforeEntering;


	void OnTriggerEnter2D(Collider2D other) {
		if (_guardState == guardState.BeforeEntering && other.name == "EnterCell") {
			_guardState = guardState.EnteredCell;
			Events.G.Raise(new GuardEnteringCellEvent());
		}
		if (other.name == "EngagePrisoner") {
			_guardState = guardState.EngagedPrisoner;
			Events.G.Raise(new GuardEngaginPrisonerEvent(true));
		}
	}


	void OnTriggerExit2D(Collider2D other) {
		if (other.name == "EngagePrisoner") {
			_guardState = guardState.EnteredCell;
			Events.G.Raise(new GuardEngaginPrisonerEvent(false));
		}
	}
}

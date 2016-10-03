using UnityEngine;
using System.Collections;

//public  class KeyGivenToPersonEvent : GameEvent {
//	public PersonId PersonId { get; private set; }
//	public KeyGivenToPersonEvent (PersonId personId){
//		PersonId = personId;
//	}
//}

public class GuardEnteringCellEvent : GameEvent {
}

public class GuardLeavingCellEvent : GameEvent {
}

public class Act1EndedEvent : GameEvent {
}

public class GuardEngaginPrisonerEvent : GameEvent {
	public bool Engaged { get; private set; }
	public GuardEngaginPrisonerEvent(bool engaged){
		Engaged = engaged;
	}
}

public class EventManager : MonoBehaviour {
}

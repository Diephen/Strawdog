using UnityEngine;
using System.Collections;

//public  class KeyGivenToPersonEvent : GameEvent {
//	public PersonId PersonId { get; private set; }
//	public KeyGivenToPersonEvent (PersonId personId){
//		PersonId = personId;
//	}
//}
public class Act0EndedEvent : GameEvent {
}


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

/* Act 2 */
public class LockCellEvent : GameEvent {
}

public class LeftCellUnlockedEvent : GameEvent {
}

public class SleepInCellEvent : GameEvent {
}

public class PrisonerFoundBombEvent : GameEvent {
}

public class GuardFoundBombEvent : GameEvent {
}

public class CaughtSneakingEvent : GameEvent {
}

//public class Act2EndedEvent : GameEvent {
//}

/* Act 3 */
public class TriggerExecutionEvent : GameEvent {
}

public class TriggerTakenAwayEvent : GameEvent {
}

public class TriggerPlantBombEvent : GameEvent {
}

/* Act 4-1 Execution*/

public class AboutToStrayOutOfLineEvent : GameEvent {
	public bool Straying { get; private set; }
	public AboutToStrayOutOfLineEvent(bool straying){
		Straying = straying;
	}
}

public class StrayOutOfLineEvent : GameEvent {
}

public class DidNotShootEvent : GameEvent {
}

public class ShootEvent : GameEvent {
}

public class PrisonerShotEvent : GameEvent {
}

/* Act 4-2 Taken Away */
public class Taken_EnterFoodStorageEvent : GameEvent {
}


/* Act 4-3 Plant Bomb */

public class Plant_EnterFoodStorageEvent : GameEvent {
}


public class Guard_EncounterEvent : GameEvent {
}

public class Prisoner_EncounterEvent: GameEvent {
}

/* Act 5 */
public class EventManager : MonoBehaviour {
}
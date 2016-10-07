﻿using UnityEngine;
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

public class StrayOutOfLineEvent : GameEvent {
}

public class DidNotShootEvent : GameEvent {
}

public class ShootEvent : GameEvent {
}

public class PrisonerShotEvent : GameEvent {
}

/* Act 4-2 Taken Away */


/* Act 4-3 Plant Bomb */

/* Act 5 */
public class EventManager : MonoBehaviour {
}
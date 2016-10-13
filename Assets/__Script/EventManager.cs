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
	public bool Locked { get; private set; }
	public LockCellEvent(bool locked){
		Locked = locked;
	}
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

public class BrokeFree : GameEvent {
}

/* Act 4-3 Plant Bomb */

public class Plant_EnterFoodStorageEvent : GameEvent {
}


public class Guard_EncounterEvent : GameEvent {
}

public class Prisoner_EncounterEvent: GameEvent {
}

/* Act 5 */




/* Key Events */
public class PickUpPressedEvent: GameEvent {
	public CharacterIdentity WhoAmI { get; private set; }
	public PickUpPressedEvent(CharacterIdentity whoAmI){
		WhoAmI = whoAmI;
	}
}
public class PickupReleasedEvent: GameEvent {
	public CharacterIdentity WhoAmI { get; private set; }
	public PickupReleasedEvent(CharacterIdentity whoAmI){
		WhoAmI = whoAmI;
	}
}
public class CrouchPressedEvent: GameEvent {
	public CharacterIdentity WhoAmI { get; private set; }
	public CrouchPressedEvent(CharacterIdentity whoAmI){
		WhoAmI = whoAmI;
	}
}
public class WalkLeftEvent: GameEvent {
	public CharacterIdentity WhoAmI { get; private set; }
	public WalkLeftEvent(CharacterIdentity whoAmI){
		WhoAmI = whoAmI;
	}
}
public class WalkRightEvent: GameEvent {
	public CharacterIdentity WhoAmI { get; private set; }
	public WalkRightEvent(CharacterIdentity whoAmI){
		WhoAmI = whoAmI;
	}
}
public class APressedEvent: GameEvent {
	public CharacterIdentity WhoAmI { get; private set; }
	public APressedEvent(CharacterIdentity whoAmI){
		WhoAmI = whoAmI;
	}
}
public class AReleasedEvent: GameEvent {
	public CharacterIdentity WhoAmI { get; private set; }
	public AReleasedEvent(CharacterIdentity whoAmI){
		WhoAmI = whoAmI;
	}
}
public class SPressedEvent: GameEvent {
	public CharacterIdentity WhoAmI { get; private set; }
	public SPressedEvent(CharacterIdentity whoAmI){
		WhoAmI = whoAmI;
	}
}
public class SReleasedEvent: GameEvent {
	public CharacterIdentity WhoAmI { get; private set; }
	public SReleasedEvent(CharacterIdentity whoAmI){
		WhoAmI = whoAmI;
	}
}
public class DPressedEvent: GameEvent {
	public CharacterIdentity WhoAmI { get; private set; }
	public DPressedEvent(CharacterIdentity whoAmI){
		WhoAmI = whoAmI;
	}
}
public class DHoldEvent: GameEvent {
	public CharacterIdentity WhoAmI { get; private set; }
	public DHoldEvent(CharacterIdentity whoAmI){
		WhoAmI = whoAmI;
	}
}
public class DReleasedEvent: GameEvent {
	public CharacterIdentity WhoAmI { get; private set; }
	public DReleasedEvent(CharacterIdentity whoAmI){
		WhoAmI = whoAmI;
	}
}

public class EnableMoveEvent: GameEvent {
}





public class EventManager : MonoBehaviour {
}


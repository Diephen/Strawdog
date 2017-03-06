using UnityEngine;
using System.Collections;

//public  class KeyGivenToPersonEvent : GameEvent {
//	public PersonId PersonId { get; private set; }
//	public KeyGivenToPersonEvent (PersonId personId){
//		PersonId = personId;
//	}
//}

public class LoadVeryBeginningEvent: GameEvent{
}

public class LoadMainMenuEvent: GameEvent{
}

public class LoadTutorialEvent: GameEvent{
}

public class LoadTitleCardEvent: GameEvent{
	public int TitleIndex { get; private set; }
	public LoadTitleCardEvent(int titleIndex){
		TitleIndex = titleIndex;
	}
}

public class Load1_1Event: GameEvent{
}

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

public class TitleEndedEvent : GameEvent {
}

/* Act 2 */
public class LockCellEvent : GameEvent {
	public bool Locked { get; private set; }
	public LockCellEvent(bool locked){
		Locked = locked;
	}
}

public class OpenOfficeEvent : GameEvent {
	public bool Opened { get; private set; }
	public OpenOfficeEvent(bool opened){
		Opened = opened;
	}
}

public class LeftCellUnlockedEvent : GameEvent {
}

public class SleepInCellEvent : GameEvent {
}

public class PrisonerSleepEvent:GameEvent{
	
}
public class DragPrisonerInJail:GameEvent{
	
}

public class CallGuardInCell:GameEvent{
	
}

public class PrisonerFoundBombEvent : GameEvent {
}

public class PrisonerFoundBombAndLeave:GameEvent{
	
}

public class PrisonerWentBack : GameEvent {
}

public class GuardFoundBombEvent : GameEvent {
}

public class GuardSleepEvent : GameEvent {
}

public class CaughtSneakingEvent : GameEvent {
}

public class GuardStairsStartEvent : GameEvent {
}

public class PrisonerLeftFenceEvent : GameEvent {
}

public class PrisonerEncounterSoldierExplore:GameEvent{
	
}

public class PrisonerStairsStartEvent : GameEvent {
}

public class Act2_GuardWalkedUpStairsEvent : GameEvent {
}

public class Act2_GuardWalkedDownStairsEvent : GameEvent {
}

public class Act2_PrisonerWalkedUpStairsEvent : GameEvent {
}

public class Act2_PrisonerWalkedDownStairsEvent : GameEvent {
}

/* soldier drag prisoner in explore */
public class Act2_SoldierAppear:GameEvent{
	
}

public class Act2_SoldierDragPrisonerExplore:GameEvent{
	
}

public class Act2_PrisonerGetBomb:GameEvent{
	
}

public class Act2_PrisonerWalkFenceInteraction:GameEvent{
	public bool IsBombFound { get; private set; }
	public Act2_PrisonerWalkFenceInteraction(bool isbombfound){
		IsBombFound = isbombfound;
	}
}


// 

public class LightCaughtEvent : GameEvent {
	public float Volume { get; private set; }
	public int Id { get; private set; }
	public LightCaughtEvent(float volume, int id){
		Volume = volume;
		Id = id;
	}
}
public class InterrogationQuestioningEvent : GameEvent {
	public bool Engaged { get; private set; }
	public InterrogationQuestioningEvent(bool engaged){
		Engaged = engaged;
	}
}

public class LightOffEvent : GameEvent {
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

public class ExecutionEncounter:GameEvent{
	public ExecutionType ExeType { get; private set; }
	public bool IsStart { get; private set;}
	public ShotDeathPrisonerHandle SP{ get; private set; }
	public ExecutionEncounter(ExecutionType exeType, ShotDeathPrisonerHandle sp, bool isStart){
		ExeType = exeType;
		IsStart = isStart;
		SP = sp;
	}
}

public class ExecutionBreakFree: GameEvent{
}


public class RestartExecution:GameEvent{
	
}

public class SoldierExecuteBoth:GameEvent{
	
}

public class GuardExecutePrisoner:GameEvent{
	
}
	

/* Act 4-2 Taken Away */
public class Taken_EnterFoodStorageEvent : GameEvent {
	public bool BrokeFree { get; private set; }
	public Taken_EnterFoodStorageEvent(bool brokeFree){
		BrokeFree = brokeFree;
	}
}

public class BrokeFree : GameEvent {
}


public class LeaveDitchEvent: GameEvent{
}

public class LineControlEvent:GameEvent{
	public bool IsStop { get; private set; }
	public LineControlEvent(bool isstop){
		IsStop = isstop;
	}
}

public class CutPrisonerBrforeOthers:GameEvent{
	
}


/* Act 4-3 Plant Bomb */

public class Plant_UpStairsEvent : GameEvent {
}

public class Plant_DownStairsEvent : GameEvent {
}


public class Plant_EnterFoodStorageEvent : GameEvent {
}
public class Plant_LeaveFoodStorageEvent : GameEvent {
}


public class Guard_EncounterEvent : GameEvent {
}

public class Prisoner_EncounterEvent: GameEvent {
}
	

/* Act 5 */
public class CallSecretDoorEvent: GameEvent{
}
public class TransitionSecretDoorEvent: GameEvent{
	public bool SecretOn { get; private set; }
	public TransitionSecretDoorEvent(bool secretOn){
		SecretOn = secretOn;
	}
}

public class TriggerAmbientWindEvent: GameEvent{
	public bool Triggered { get; private set; }
	public TriggerAmbientWindEvent(bool triggered){
		Triggered = triggered;
	}
}

public class StopSecretExitEvent: GameEvent{
	public bool Stopped { get; private set; }
	public StopSecretExitEvent(bool stopped){
		Stopped = stopped;
	}
}

public class StartAct5Event: GameEvent {
}

public class StartCreditsEvent: GameEvent {
}

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

//Not for animation injection
public class CrouchHideEvent: GameEvent {
	public CharacterIdentity WhoAmI { get; private set; }
	public CrouchHideEvent(CharacterIdentity whoAmI){
		WhoAmI = whoAmI;
	}
}

public class CrouchReleaseHideEvent: GameEvent {
	public CharacterIdentity WhoAmI { get; private set; }
	public CrouchReleaseHideEvent(CharacterIdentity whoAmI){
		WhoAmI = whoAmI;
	}
}

public class PrisonerHideEvent: GameEvent {
	public bool Hidden { get; private set; }
	public PrisonerHideEvent(bool hidden){
		Hidden = hidden;
	}
}

//Call to Enable Movement
public class EnableMoveEvent: GameEvent {
	public CharacterIdentity WhoAmI { get; private set; }
	public EnableMoveEvent(CharacterIdentity whoAmI = CharacterIdentity.Both){
		WhoAmI = whoAmI;
	}
}
public class DisableMoveEvent: GameEvent {
	public CharacterIdentity WhoAmI { get; private set; }
	public DisableMoveEvent(CharacterIdentity whoAmI = CharacterIdentity.Both){
		WhoAmI = whoAmI;
	}
}

public class IsWalkingEvent : GameEvent {
	public CharacterIdentity WhoAmI { get; private set; }
	public bool IsWalking { get; private set; }
	public bool IsLeft { get; private set; }
	public IsWalkingEvent(CharacterIdentity whoAmI, bool isWalking, bool isLeft){
		WhoAmI = whoAmI;
		IsWalking = isWalking;
		IsLeft = isLeft;
	}
}

//Endings

public class RunAloneEndingEvent: GameEvent {
}

public class RunTogetherEndingEvent: GameEvent {
}

public class GuardAloneEndingEvent: GameEvent {
}
	



public class StrayOutOfLineEvent : GameEvent {
}

public class DidNotShootEvent : GameEvent {
}

public class ShootEvent : GameEvent {
}
	
public class ShootSwitchEvent : GameEvent {
}


public class PrisonerShotEvent : GameEvent {
}

//Post Ending
public class RetryEvent : GameEvent {
}



public class StaticCamera: GameEvent {
}

public class EventManager : MonoBehaviour {
}

// Encounter -- when prisoner overlap guard
public class EncounterTouchEvent:GameEvent{
	public bool OnGuard { get; private set; }
	public EncounterTouchEvent(bool onguard){
		OnGuard = onguard;
	}
	
	
}

public class EncountEndStateEvent:GameEvent{
	public CharacterIdentity WhoAmI { get; private set; }
	public bool IsReady { get; private set; }
	public EncountEndStateEvent(CharacterIdentity whoAmI, bool isReady){
		WhoAmI = whoAmI;
		IsReady = isReady;
	}
}

// ** For progress bar ** //
public class UIProgressBar:GameEvent{
	public bool IsDrop { get; private set; }
	public UIProgressBar(bool isdrop){
		IsDrop = isdrop;
	}
}




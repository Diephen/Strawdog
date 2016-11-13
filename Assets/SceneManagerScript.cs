﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Giverspace;

public class SceneManagerScript : MonoBehaviour {

	enum SceneIndex {
		Logo = 0, 
		Act0 = 1, 
		Act1 = 2,
		Title = 3,
		Act2 = 4,
		Act2_PDown = 5,
		Act2_Explore = 6,
		Act2_GDown = 7,
		Act2_Patrol = 8,
		Act3_No = 9,
		Act3_Yes = 10,
		Act3_Plant_Cell = 11,
		Act3_Plant_Cell_Again = 12,
		Act3_Plant = 13,
		Act3_Plant_Bomb = 14,
		Act3_Plant_Done = 15,
		Act4_1 = 16,
		Act4_2 = 17,
		Act4_2_Ditch = 18,
		Act4_3_Encounter = 19,
		TriggerWarning = 20,
		Ending = 21,
		Tutorial = 22
	};

	FrameScript _frameScript;
//	bool _start = false;
	bool _once = false;
	[SerializeField] VoiceOverManager _voManager;

	void Update(){
//		if(_start){
//			if (Input.GetKeyDown (KeyCode.Space)) {
//				_frameScript = GameObject.Find ("Frame").GetComponent<FrameScript> ();
//				_frameScript.OpenFlap ();
//				_start = false;
//				Log.Metrics.Message("Start Time");
//			}
//		}
		if (Input.GetKeyDown (KeyCode.Minus)) {
			SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
		}
		if (Input.GetKeyDown (KeyCode.Backspace)) {
			SceneManager.LoadScene (0);
		}
		if (Input.GetKeyDown (KeyCode.Equals)) {
			SceneManager.LoadScene (4);
		}
	}

	void OnEnable ()
	{
		Events.G.AddListener<TutorialEndEvent>(LoadAct0);
		Events.G.AddListener<Act0EndedEvent>(LoadVertical);

		Events.G.AddListener<Act1EndedEvent>(LoadTitle);
		Events.G.AddListener<GuardLeavingCellEvent>(OnGuardLeaveCell);

		Events.G.AddListener<TitleEndedEvent>(LoadAct2);

		Events.G.AddListener<Act2_PrisonerWalkedUpStairsEvent>(LoadAct2Explore);
		Events.G.AddListener<Act2_PrisonerWalkedDownStairsEvent>(LoadAct2Explore_down);
		Events.G.AddListener<Act2_GuardWalkedUpStairsEvent>(LoadAct2Patrol);
		Events.G.AddListener<Act2_GuardWalkedDownStairsEvent>(LoadAct2Patrol_down);

		Events.G.AddListener<SleepInCellEvent>(LoadAct3_No);
		Events.G.AddListener<PrisonerWentBack>(LoadWentBack);
		Events.G.AddListener<GuardSleepEvent>(LoadGuardSleep);
		Events.G.AddListener<PrisonerFoundBombEvent>(LoadAct3_Yes);
		Events.G.AddListener<GuardFoundBombEvent>(LoadAct3_Plant);
		Events.G.AddListener<CaughtSneakingEvent>(LoadCaught);

		Events.G.AddListener<TriggerExecutionEvent>(LoadExecution);
		Events.G.AddListener<TriggerTakenAwayEvent>(LoadTakenAway);
		Events.G.AddListener<TriggerPlantBombEvent>(LoadPlantBomb);

		Events.G.AddListener<DidNotShootEvent>(LoadAllGunnedDown);
		Events.G.AddListener<PrisonerShotEvent>(LoadPrisonerShotDown);

		Events.G.AddListener<Taken_EnterFoodStorageEvent>(LoadFoodStorage_Prisoner);
		Events.G.AddListener<LeaveDitchEvent>(LoadLeaveDitch);

		Events.G.AddListener<Plant_UpStairsEvent>(LoadPlantUp);
		Events.G.AddListener<Plant_DownStairsEvent>(LoadPlantDown);
		Events.G.AddListener<Plant_EnterFoodStorageEvent>(LoadFoodStorage_Guard);
		Events.G.AddListener<Plant_LeaveFoodStorageEvent>(LoadLeaveFoodStorage);

		Events.G.AddListener<Prisoner_EncounterEvent>(LoadEncounter1);
		Events.G.AddListener<Guard_EncounterEvent>(LoadEncounter2);

		Events.G.AddListener<TriggerWarningEndEvent>(LoadTutorial);

		Events.G.AddListener<RunAloneEndingEvent>(LoadEnd_RunAlone);
		Events.G.AddListener<RunTogetherEndingEvent>(LoadEnd_RunTogether);
		Events.G.AddListener<GuardAloneEndingEvent>(LoadEnd_GuardAlone);

		Events.G.AddListener<RetryEvent> (LoadRetry);
		Events.G.AddListener<RestartExecution> (ReLoadExecution);
		Events.G.AddListener<SoldierExecuteBoth> (LoadExecutionEndBothDie);
		Events.G.AddListener<GuardExecutePrisoner> (LoadExecutionEndPrisonerDie);

		SceneManager.sceneLoaded += OpenScreen;
	}

	void OnDisable ()
	{
		
		Events.G.RemoveListener<TutorialEndEvent>(LoadAct0);
		Events.G.RemoveListener<Act0EndedEvent>(LoadVertical);

		Events.G.RemoveListener<Act1EndedEvent>(LoadTitle);
		Events.G.RemoveListener<GuardLeavingCellEvent>(OnGuardLeaveCell);

		Events.G.RemoveListener<TitleEndedEvent>(LoadAct2);

		Events.G.RemoveListener<Act2_PrisonerWalkedUpStairsEvent>(LoadAct2Explore);
		Events.G.RemoveListener<Act2_PrisonerWalkedDownStairsEvent>(LoadAct2Explore_down);
		Events.G.RemoveListener<Act2_GuardWalkedUpStairsEvent>(LoadAct2Patrol);
		Events.G.RemoveListener<Act2_GuardWalkedDownStairsEvent>(LoadAct2Patrol_down);

		Events.G.RemoveListener<SleepInCellEvent>(LoadAct3_No);
		Events.G.RemoveListener<PrisonerWentBack>(LoadWentBack);
		Events.G.RemoveListener<GuardSleepEvent>(LoadGuardSleep);
		Events.G.RemoveListener<PrisonerFoundBombEvent>(LoadAct3_Yes);
		Events.G.RemoveListener<GuardFoundBombEvent>(LoadAct3_Plant);
		Events.G.RemoveListener<CaughtSneakingEvent>(LoadCaught);

		Events.G.RemoveListener<TriggerExecutionEvent>(LoadExecution);
		Events.G.RemoveListener<TriggerTakenAwayEvent>(LoadTakenAway);
		Events.G.RemoveListener<TriggerPlantBombEvent>(LoadPlantBomb);

		Events.G.RemoveListener<DidNotShootEvent>(LoadAllGunnedDown);
		Events.G.RemoveListener<PrisonerShotEvent>(LoadPrisonerShotDown);

		Events.G.RemoveListener<Taken_EnterFoodStorageEvent>(LoadFoodStorage_Prisoner);
		Events.G.RemoveListener<LeaveDitchEvent>(LoadLeaveDitch);

		Events.G.RemoveListener<Plant_UpStairsEvent>(LoadPlantUp);
		Events.G.RemoveListener<Plant_DownStairsEvent>(LoadPlantDown);
		Events.G.RemoveListener<Plant_EnterFoodStorageEvent>(LoadFoodStorage_Guard);
		Events.G.RemoveListener<Plant_LeaveFoodStorageEvent>(LoadLeaveFoodStorage);

		Events.G.RemoveListener<Prisoner_EncounterEvent>(LoadEncounter1);
		Events.G.RemoveListener<Guard_EncounterEvent>(LoadEncounter2);

		Events.G.RemoveListener<TriggerWarningEndEvent>(LoadTutorial);

		Events.G.RemoveListener<RunAloneEndingEvent>(LoadEnd_RunAlone);
		Events.G.RemoveListener<RunTogetherEndingEvent>(LoadEnd_RunTogether);
		Events.G.RemoveListener<GuardAloneEndingEvent>(LoadEnd_GuardAlone);

		Events.G.RemoveListener<RetryEvent> (LoadRetry);
		Events.G.RemoveListener<RestartExecution> (ReLoadExecution);
		Events.G.RemoveListener<SoldierExecuteBoth> (LoadExecutionEndBothDie);
		Events.G.RemoveListener<GuardExecutePrisoner> (LoadExecutionEndPrisonerDie);

		SceneManager.sceneLoaded -= OpenScreen;
	}


	void LoadAct0(TutorialEndEvent e){
		StartCoroutine(ChangeFade((int)SceneIndex.Act0, 3f));
	}

	void LoadTutorial(TriggerWarningEndEvent e){
		StartCoroutine(ChangeFade((int)SceneIndex.Tutorial, 0.5f));
	}

	void LoadVertical(Act0EndedEvent e){
		Log.Metrics.Message("End Act 0");
		StartCoroutine(ChangeLevel((int)SceneIndex.Act1, 2f));
	}

	void OnGuardLeaveCell (GuardLeavingCellEvent e){
		Log.Metrics.Message("End Act 1");
		Log.Metrics.Message("CHOICE 1: Leave");
		StartCoroutine(ChangeLevel((int)SceneIndex.Title, 1.5f, "VoiceOver/02_StopDrown"));
	}

	void LoadTitle(Act1EndedEvent e){
		Log.Metrics.Message("End Act 1");
		Log.Metrics.Message("CHOICE 1: Drown");
		StartCoroutine(ChangeLevel((int)SceneIndex.Title, 4f, "VoiceOver/03_Drown"));
	}

		
	void LoadAct2(TitleEndedEvent e){
		StartCoroutine(ChangeLevel((int)SceneIndex.Act2, 0.5f));
	}

	void LoadAct2Explore(Act2_PrisonerWalkedUpStairsEvent e){
//		StartCoroutine(ChangeLevel(4, 2f));
		//End PlayTest
		StartCoroutine(ChangeLevel((int)SceneIndex.Act2_Explore, 2f, "VoiceOver/06_Explore"));
	}

	void LoadAct2Explore_down(Act2_PrisonerWalkedDownStairsEvent e){
		StartCoroutine(ChangeLevel((int)SceneIndex.Act2_PDown, 2f));
	}

	void LoadAct2Patrol(Act2_GuardWalkedUpStairsEvent e){
//		StartCoroutine(ChangeLevel(5, 2f));
		//End PlayTest
		StartCoroutine(ChangeLevel((int)SceneIndex.Act2_Patrol, 2f));
	}

	void LoadAct2Patrol_down(Act2_GuardWalkedDownStairsEvent e){
		StartCoroutine(ChangeLevel((int)SceneIndex.Act2_GDown, 2f));
	}

	void LoadAct3_No(SleepInCellEvent e){
		Log.Metrics.Message("CHOICE 3: Bed");
//		StartCoroutine(ChangeLevel(4, 2f));
		//End PlayTest
		StartCoroutine(ChangeLevel((int)SceneIndex.Act3_No, 3f, "VoiceOver/05_Bed"));
	}

	void LoadWentBack(PrisonerWentBack e){
		Log.Metrics.Message("CHOICE: Prisoner Return (No Bomb)");
		StartCoroutine(ChangeLevel((int)SceneIndex.Act3_No, 1f, "VoiceOver/09_GoBack"));
	}

	void LoadGuardSleep(GuardSleepEvent e){
		Log.Metrics.Message("CHOICE: Guard Sleep");
		StartCoroutine(ChangeLevel((int)SceneIndex.Act3_No, 1f, "VoiceOver/07_GuardSleep"));
	}

	void LoadAct3_Yes(PrisonerFoundBombEvent e){
		Log.Metrics.Message("CHOICE: Prisoner Bomb");
		StartCoroutine(ChangeLevel((int)SceneIndex.Act3_Yes, 3f, "VoiceOver/19_JustMaybe"));
	}
	void LoadAct3_Plant(GuardFoundBombEvent e) {
		Log.Metrics.Message("CHOICE: Guard Return (No Bomb)");
		StartCoroutine(ChangeLevel((int)SceneIndex.Act3_Plant_Cell, 3f, "VoiceOver/20_MaybeHim"));
	}
	void LoadCaught(CaughtSneakingEvent e){
	}

	void LoadFoodStorage_Prisoner(Taken_EnterFoodStorageEvent e){
		Log.Metrics.Message("CHOICE: Killed At Ditch");
		if (e.BrokeFree) {
			StartCoroutine (ChangeLevel ((int)SceneIndex.Ending, 1.5f, "VoiceOver/15_Detrimental"));
		}
		else {
			StartCoroutine (ChangeLevel ((int)SceneIndex.Ending, 1.5f, "VoiceOver/14_Terrible"));
		}
	}

	void LoadExecution(TriggerExecutionEvent e){
		Log.Metrics.Message("CHOICE: Execution");
		StartCoroutine(ChangeLevel((int)SceneIndex.Act4_1, 1f, "VoiceOver/10_BombCaught"));
	}

	void ReLoadExecution(RestartExecution e){
		StartCoroutine(ChangeLevel((int)SceneIndex.Act4_1, 1f));
	}

	// When decide not to execute the prisoner
	void LoadExecutionEndBothDie(SoldierExecuteBoth e){
		StartCoroutine(ChangeLevel((int)SceneIndex.Ending, 3f, "VoiceOver/12_NoChoice"));
	}

	void LoadExecutionEndPrisonerDie(GuardExecutePrisoner e){
		StartCoroutine(ChangeLevel((int)SceneIndex.Ending, 4f, "VoiceOver/11_Unfortunate"));
	}

	void LoadTakenAway(TriggerTakenAwayEvent e){
		StartCoroutine(ChangeLevel((int)SceneIndex.Act4_2_Ditch, 1f));
	}

	void LoadLeaveDitch(LeaveDitchEvent e){
		Log.Metrics.Message("CHOICE: Leave Ditch");
		StartCoroutine(ChangeLevel((int)SceneIndex.Act4_2, 1f));
	}

	void LoadPlantBomb(TriggerPlantBombEvent e){
		Log.Metrics.Message("CHOICE: Plant Bomb");
		StartCoroutine(ChangeLevel(8, 1f));
	}


	void LoadAllGunnedDown(DidNotShootEvent e){
	}
	void LoadPrisonerShotDown(PrisonerShotEvent e){
	}

	void LoadPlantUp(Plant_UpStairsEvent e){
		StartCoroutine(ChangeLevel((int)SceneIndex.Act3_Plant, 1f));
	}

	void LoadPlantDown(Plant_DownStairsEvent e){
		StartCoroutine(ChangeLevel((int)SceneIndex.Act3_Plant_Cell_Again, 1f));
	}

	void LoadFoodStorage_Guard(Plant_EnterFoodStorageEvent e) {
		StartCoroutine(ChangeLevel((int)SceneIndex.Act3_Plant_Bomb, 1f));
	}

	void LoadLeaveFoodStorage(Plant_LeaveFoodStorageEvent e){
		StartCoroutine(ChangeLevel((int)SceneIndex.Act3_Plant_Done, 1f, "VoiceOver/22_DeedDone"));
	}

	void LoadEncounter1(Prisoner_EncounterEvent e){
		StartCoroutine(ChangeLevel((int)SceneIndex.Act4_3_Encounter, 3f));
	}

	void LoadEncounter2(Guard_EncounterEvent e){
		StartCoroutine(ChangeLevel((int)SceneIndex.Act4_3_Encounter, 3f));
	}

	//Ending
	void LoadEnd_RunAlone(RunAloneEndingEvent e){
		Log.Metrics.Message("CHOICE: Run Alone");
		StartCoroutine(ChangeLevel((int)SceneIndex.Ending, 1.5f, "VoiceOver/18_LetGo"));
	}

	void LoadEnd_RunTogether(RunTogetherEndingEvent e){
		Log.Metrics.Message("CHOICE: Escape Together");
		StartCoroutine(ChangeLevel((int)SceneIndex.Ending, 3f, "VoiceOver/16_EscapeTogether"));
	}

	void LoadEnd_GuardAlone(GuardAloneEndingEvent e){
		Log.Metrics.Message("CHOICE: Shot Prisoner");
		StartCoroutine(ChangeLevel((int)SceneIndex.Ending, 3f, "VoiceOver/17_JustJob"));
	}

	void LoadRetry(RetryEvent e){
		StartCoroutine(ChangeLevel((int)SceneIndex.Title, 4f));
	}
		


	void OpenScreen(Scene scene, LoadSceneMode mode){
		if (scene.name == "TriggerWarning") {
		}
		//Capture all ending Scenes
		else if (scene.name == "EndingTemplate") {
		}
		else {
			StartCoroutine (WaitBeforeOpen ());
		}
	}

//	void OnLevelWasLoaded() {
//		_frameScript = GameObject.Find ("Frame").GetComponent<FrameScript> ();
//		_frameScript.OpenFlap ();
//	}

	IEnumerator WaitBeforeOpen(){
		yield return new WaitForSeconds(1.0f);
		if (GameObject.Find ("Frame")!= null) {
			_frameScript = GameObject.Find ("Frame").GetComponent<FrameScript> ();
			_frameScript.OpenFlap ();
		}

	}

	IEnumerator ChangeLevel(int index, float duration, string path = null){
		if (!_once) {
			_once = true;
			yield return new WaitForSeconds (duration);
			if (_frameScript != null) {
				_frameScript.CloseFlap ();
			}
			float waitTime = 1.0f;
			if (path != null) {
				yield return new WaitForSeconds (2.0f);
				waitTime = _voManager.PlayVoiceOver (path);
			}
			yield return new WaitForSeconds (waitTime);
			SceneManager.LoadScene (index);
		}
	}


	IEnumerator ChangeFade(int index, float duration){
		if (!_once) {
			_once = true;
			yield return new WaitForSeconds (duration);
//			if (_frameScript != null) {
//				_frameScript.CloseFlap ();
//			}
			GameObject.Find("Fade").GetComponent<Fading>().BeginFade(1);
			yield return new WaitForSeconds (1.0f);
			SceneManager.LoadScene (index);
		}
	}
}

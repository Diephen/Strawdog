using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Giverspace;

public enum SceneIndex {
	A0_1_Logo = 0,
	A0_2_TriggerWarning = 1,
	A0_3_Tutorial = 2,
	A0_4_MainMenu = 3,
	A0_5_Title = 4,
	A0_6_TitleCard = 5,
	A0_7_Ending = 6,
	A0_8_Credits = 7,
	//Act 1
	A1_1_Intro = 8,
	A1_1_Torture = 9,
	//Act 2
	A2_1_Cell = 10,
	A2_2_Explore = 11,
	A2_3_Patrol = 12,
	A2_4_BombPlantArea = 13,
	A2_5_InterrogationRoom = 14,
	//Act 3
	A3_1_Interrogation = 15,
	A3_2_InterrogationBomb = 16,
	A3_3_PrisonerRead = 17,
	A3_4_GuardRead = 18,
	A3_5_FoodReveal = 19,
	//Act 4
	A4_1_Execution = 20,
	A4_2_Ditch = 21,
	A4_3_EncounterLeft = 22,
	A4_4_EncounterRight = 23,
	A4_5_PlantBomb = 24,
	A4_6_PlantBombDone = 25,
	//Act 5
	A5_1_SecretRoom = 26,
	A5_2_Trial = 27,
	A5_3_Ending = 28

	// Logo = 210, 
	// Act0 = 211, 
	// Act1 = 212,
	// Title = 213,
	// Act2 = 214,
	// Act2_PDown = 215,
	// Act2_Explore = 216,
	// Act2_GDown = 217,
	// Act2_Patrol = 218,
	// Act3_No = 9,
	// Act3_Yes = 10,
	// Act3_Plant_Cell = 11,
	// Act3_Plant_Cell_Again = 12,
	// Act3_Plant = 13,
	// Act3_Plant_Bomb = 14,
	// Act3_Plant_Done = 15,
	// Act4_1 = 16,
	// Act4_2 = 17,
	// Act4_2_Ditch = 18,
	// Act4_3_Encounter = 19,
	// TriggerWarning = 20,
	// Ending = 21,
	// Tutorial = 22,
	// Act5 = 23,
	// Credits = 24
};

public class SceneManagerScript : MonoBehaviour {

	// 0 : Killed at ditch
	// 1 : Prisoner Escapes
	// 2 : Execution for Crimes
	// 3 : Stopped Escape
	// 4 : Happy Ending?
	// 5 : Plant Bomb
	// 6 : Final Ending

	FrameScript _frameScript;
//	bool _start = false;
	bool _once = false;
	[SerializeField] VoiceOverManager _voManager;

	void Start() {
		GameStateManager.gameStateManager._currScene = (SceneIndex)SceneManager.GetActiveScene ().buildIndex;
	}

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
		Events.G.AddListener<LoadMainMenuEvent>(LoadMainMenu);
		Events.G.AddListener<LoadTutorialEvent>(LoadTutorial);
		Events.G.AddListener<LoadTitleCardEvent> (LoadTitleCard);
		//Refactor Line

		Events.G.AddListener<LoadVeryBeginningEvent>(LoadVeryBeginning);
		Events.G.AddListener<Load1_1Event>(Load1_1);
		Events.G.AddListener<Act0EndedEvent>(LoadVertical);

		Events.G.AddListener<Act1EndedEvent>(LoadTitle);
		Events.G.AddListener<GuardLeavingCellEvent>(OnGuardLeaveCell);

		Events.G.AddListener<TitleEndedEvent>(LoadAct2);
		Events.G.AddListener<Load2_1Event>(Load2_1);

		Events.G.AddListener<Load2_2Event>(Load2_2);
		Events.G.AddListener<Load2_3Event>(Load2_3);
		Events.G.AddListener<Load2_4Event>(Load2_4);
		Events.G.AddListener<Load3_1Event>(Load3_1);
		Events.G.AddListener<Load3_2Event>(Load3_2);
		Events.G.AddListener<Load3_3Event>(Load3_3);
		Events.G.AddListener<Load3_4Event>(Load3_4);
		Events.G.AddListener<Load3_5Event>(Load3_5);
		Events.G.AddListener<Load4_1Event>(Load4_1);
		Events.G.AddListener<Load4_2Event>(Load4_2);
		Events.G.AddListener<Load4_3Event>(Load4_3);
		Events.G.AddListener<Load4_4Event>(Load4_4);
		Events.G.AddListener<Load4_5Event>(Load4_5);
		Events.G.AddListener<Load4_6Event>(Load4_6);

		Events.G.AddListener<Act2_PrisonerWalkedUpStairsEvent>(LoadAct2Explore);
		Events.G.AddListener<Act2_PrisonerWalkedDownStairsEvent>(LoadAct2Explore_down);
		Events.G.AddListener<Act2_GuardWalkedUpStairsEvent>(LoadAct2Patrol);
		Events.G.AddListener<Act2_GuardWalkedDownStairsEvent>(LoadAct2Patrol_down);

		Events.G.AddListener<PrisonerSleepEvent>(LoadAct3_No);
		Events.G.AddListener<PrisonerWentBack>(LoadWentBack);
		Events.G.AddListener<GuardSleepEvent>(LoadGuardSleep);
		Events.G.AddListener<PrisonerFoundBombAndLeave>(LoadAct3_Yes);
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



		Events.G.AddListener<RunAloneEndingEvent>(LoadEnd_RunAlone);
		Events.G.AddListener<RunTogetherEndingEvent>(LoadEnd_RunTogether);
		Events.G.AddListener<GuardAloneEndingEvent>(LoadEnd_GuardAlone);

		Events.G.AddListener<RetryEvent> (LoadRetry);
		Events.G.AddListener<RestartExecution> (ReLoadExecution);
		Events.G.AddListener<SoldierExecuteBoth> (LoadExecutionEndBothDie);
		Events.G.AddListener<GuardExecutePrisoner> (LoadExecutionEndPrisonerDie);

		Events.G.AddListener<StartAct5Event> (LoadAct5);
		Events.G.AddListener<StartCreditsEvent> (LoadCredits);

		SceneManager.sceneLoaded += OpenScreen;
	}

	void OnDisable ()
	{
		Events.G.RemoveListener<LoadMainMenuEvent>(LoadMainMenu);
		Events.G.RemoveListener<LoadTutorialEvent>(LoadTutorial);
		Events.G.RemoveListener<LoadTitleCardEvent> (LoadTitleCard);
		//Refactor Line

		Events.G.RemoveListener<LoadVeryBeginningEvent>(LoadVeryBeginning);
		Events.G.RemoveListener<Load1_1Event>(Load1_1);
		Events.G.RemoveListener<Act0EndedEvent>(LoadVertical);

		Events.G.RemoveListener<Act1EndedEvent>(LoadTitle);
		Events.G.RemoveListener<GuardLeavingCellEvent>(OnGuardLeaveCell);

		Events.G.RemoveListener<TitleEndedEvent>(LoadAct2);
		Events.G.RemoveListener<Load2_1Event>(Load2_1);

		Events.G.RemoveListener<Load2_2Event>(Load2_2);
		Events.G.RemoveListener<Load2_3Event>(Load2_3);
		Events.G.RemoveListener<Load2_4Event>(Load2_4);
		Events.G.RemoveListener<Load3_1Event>(Load3_1);
		Events.G.RemoveListener<Load3_2Event>(Load3_2);
		Events.G.RemoveListener<Load3_3Event>(Load3_3);
		Events.G.RemoveListener<Load3_4Event>(Load3_4);
		Events.G.RemoveListener<Load3_5Event>(Load3_5);
		Events.G.RemoveListener<Load4_1Event>(Load4_1);
		Events.G.RemoveListener<Load4_2Event>(Load4_2);
		Events.G.RemoveListener<Load4_3Event>(Load4_3);
		Events.G.RemoveListener<Load4_4Event>(Load4_4);
		Events.G.RemoveListener<Load4_5Event>(Load4_5);
		Events.G.RemoveListener<Load4_6Event>(Load4_6);

		Events.G.RemoveListener<Act2_PrisonerWalkedUpStairsEvent>(LoadAct2Explore);
		Events.G.RemoveListener<Act2_PrisonerWalkedDownStairsEvent>(LoadAct2Explore_down);
		Events.G.RemoveListener<Act2_GuardWalkedUpStairsEvent>(LoadAct2Patrol);
		Events.G.RemoveListener<Act2_GuardWalkedDownStairsEvent>(LoadAct2Patrol_down);

		Events.G.RemoveListener<PrisonerSleepEvent>(LoadAct3_No);
		Events.G.RemoveListener<PrisonerWentBack>(LoadWentBack);
		Events.G.RemoveListener<GuardSleepEvent>(LoadGuardSleep);
		Events.G.RemoveListener<PrisonerFoundBombAndLeave>(LoadAct3_Yes);
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



		Events.G.RemoveListener<RunAloneEndingEvent>(LoadEnd_RunAlone);
		Events.G.RemoveListener<RunTogetherEndingEvent>(LoadEnd_RunTogether);
		Events.G.RemoveListener<GuardAloneEndingEvent>(LoadEnd_GuardAlone);

		Events.G.RemoveListener<RetryEvent> (LoadRetry);
		Events.G.RemoveListener<RestartExecution> (ReLoadExecution);
		Events.G.RemoveListener<SoldierExecuteBoth> (LoadExecutionEndBothDie);
		Events.G.RemoveListener<GuardExecutePrisoner> (LoadExecutionEndPrisonerDie);

		Events.G.RemoveListener<StartAct5Event> (LoadAct5);
		Events.G.RemoveListener<StartCreditsEvent> (LoadCredits);

		SceneManager.sceneLoaded -= OpenScreen;
	}

	void LoadVeryBeginning (LoadVeryBeginningEvent e){
		StartCoroutine(ChangeFade((int)SceneIndex.A0_1_Logo, 2f));
	}

	void LoadMainMenu(LoadMainMenuEvent e){
		StartCoroutine (ChangeFade ((int)SceneIndex.A0_4_MainMenu, 2f));
	}

	void LoadTitleCard(LoadTitleCardEvent e){
		GameStateManager.gameStateManager._actTitleIndex = e.TitleIndex;
		StartCoroutine (ChangeFade ((int)SceneIndex.A0_6_TitleCard, 2f));
	}

	void Load1_1(Load1_1Event e){
		StartCoroutine(ChangeLevel((int)SceneIndex.A1_1_Intro, 3f));
	}

	void Load2_1(Load2_1Event e){
		StartCoroutine(ChangeLevel((int)SceneIndex.A2_1_Cell, 3f));
	}

	void Load2_2(Load2_2Event e){
		//StartCoroutine(ChangeLevel((int)SceneIndex.A2_2_Explore, 3f));
		//DemoBuild
		StartCoroutine(ChangeLevel(11, 3f));
	}

	void Load2_3(Load2_3Event e){
		//StartCoroutine(ChangeLevel((int)SceneIndex.A2_3_Patrol, 3f));
		//DemoBuild
		StartCoroutine(ChangeLevel(11, 3f));
	}

	void Load2_4(Load2_4Event e){
		StartCoroutine(ChangeLevel((int)SceneIndex.A2_4_BombPlantArea, 3f));
	}

	void Load3_1(Load3_1Event e){
		StartCoroutine(ChangeLevel((int)SceneIndex.A3_1_Interrogation, 3f));
	}

	void Load3_2(Load3_2Event e){
		StartCoroutine(ChangeLevel((int)SceneIndex.A3_2_InterrogationBomb, 3f));
	}

	void Load3_3(Load3_3Event e){
		StartCoroutine(ChangeLevel((int)SceneIndex.A3_3_PrisonerRead, 1f));
	}

	void Load3_4(Load3_4Event e){
		StartCoroutine(ChangeLevel((int)SceneIndex.A3_4_GuardRead, 3f));
	}

	void Load3_5(Load3_5Event e){
		StartCoroutine(ChangeLevel((int)SceneIndex.A3_5_FoodReveal, 3f));
	}

	void Load4_1(Load4_1Event e){
		StartCoroutine(ChangeLevel((int)SceneIndex.A4_1_Execution, 3f));
	}

	void Load4_2(Load4_2Event e){
		StartCoroutine(ChangeLevel((int)SceneIndex.A4_2_Ditch, 3f));
	}

	void Load4_3(Load4_3Event e){
		StartCoroutine(ChangeLevel((int)SceneIndex.A4_3_EncounterLeft, 3f));
	}

	void Load4_4(Load4_4Event e){
		StartCoroutine(ChangeLevel((int)SceneIndex.A4_4_EncounterRight, 3f));
	}

	void Load4_5(Load4_5Event e){
		StartCoroutine(ChangeLevel((int)SceneIndex.A4_5_PlantBomb, 3f));
	}

	void Load4_6(Load4_6Event e){
		StartCoroutine(ChangeLevel((int)SceneIndex.A4_6_PlantBombDone, 3f));
	}


	void LoadTutorial(LoadTutorialEvent e){
		StartCoroutine(ChangeFade((int)SceneIndex.A0_3_Tutorial, 0.5f));
	}

	void LoadVertical(Act0EndedEvent e){
		StartCoroutine(ChangeLevel((int)SceneIndex.A1_1_Torture, 2f));
	}

	void OnGuardLeaveCell (GuardLeavingCellEvent e){
		StartCoroutine(ChangeLevel((int)SceneIndex.A0_5_Title, 1.5f));
	}

	void LoadTitle(Act1EndedEvent e){
		StartCoroutine(ChangeLevel((int)SceneIndex.A0_5_Title, 4f));
	}
		
	void LoadAct2(TitleEndedEvent e){
		GameStateManager.gameStateManager._actTitleIndex = 1;
		StartCoroutine(ChangeLevel((int)SceneIndex.A0_6_TitleCard, 0.5f));
	}

	void LoadAct2Explore(Act2_PrisonerWalkedUpStairsEvent e){
//		StartCoroutine(ChangeLevel(4, 2f));
		//End PlayTest
		StartCoroutine(ChangeLevel((int)SceneIndex.A2_2_Explore, 2f));
	}

	void LoadAct2Explore_down(Act2_PrisonerWalkedDownStairsEvent e){
		StartCoroutine(ChangeLevel((int)SceneIndex.A2_1_Cell, 2f));
	}

	void LoadAct2Patrol(Act2_GuardWalkedUpStairsEvent e){
//		StartCoroutine(ChangeLevel(5, 2f));
		//End PlayTest
		//StartCoroutine(ChangeLevel((int)SceneIndex.A2_3_Patrol, 2f));
		//demo build
		StartCoroutine(ChangeLevel(11, 2f));
	}

	void LoadAct2Patrol_down(Act2_GuardWalkedDownStairsEvent e){
		StartCoroutine(ChangeLevel((int)SceneIndex.A2_3_Patrol, 2f));
	}

	void LoadAct3_No(PrisonerSleepEvent e){
//		StartCoroutine(ChangeLevel(4, 2f));
		//End PlayTest
		GameStateManager.gameStateManager._actTitleIndex = 4;
		//StartCoroutine(ChangeLevel((int)SceneIndex.A0_6_TitleCard, 0.5f));
		//Demo Build
		StartCoroutine(ChangeLevel(11, 0.5f));
		//StartCoroutine(ChangeLevel((int)SceneIndex.A3_1_Interrogation, 2f, "VoiceOver/05_Bed"));
	}

	void LoadWentBack(PrisonerWentBack e){
		GameStateManager.gameStateManager._actTitleIndex = 2;
		StartCoroutine(ChangeLevel((int)SceneIndex.A0_6_TitleCard, 0.5f));
		//StartCoroutine(ChangeLevel((int)SceneIndex.A3_1_Interrogation, 1f, "VoiceOver/09_GoBack"));
	}

	void LoadGuardSleep(GuardSleepEvent e){
		GameStateManager.gameStateManager._actTitleIndex = 5;
		StartCoroutine(ChangeLevel((int)SceneIndex.A0_6_TitleCard, 0.5f));
		//StartCoroutine(ChangeLevel((int)SceneIndex.A3_1_Interrogation, 1f, "VoiceOver/07_GuardSleep"));
	}

	void LoadAct3_Yes(PrisonerFoundBombAndLeave e){
		GameStateManager.gameStateManager._actTitleIndex = 3;
		StartCoroutine(ChangeLevel((int)SceneIndex.A0_6_TitleCard, 0.5f));
		//StartCoroutine(ChangeLevel((int)SceneIndex.A3_2_InterrogationBomb, 1f, "VoiceOver/19_JustMaybe"));
	}
	void LoadAct3_Plant(GuardFoundBombEvent e) {
		GameStateManager.gameStateManager._actTitleIndex = 6;
		StartCoroutine(ChangeLevel((int)SceneIndex.A0_6_TitleCard, 0.5f));
		//StartCoroutine(ChangeLevel((int)SceneIndex.A4_5_PlantBomb, 3f, "VoiceOver/20_MaybeHim"));
	}
	void LoadCaught(CaughtSneakingEvent e){
	}

	void LoadFoodStorage_Prisoner(Taken_EnterFoodStorageEvent e){
		GameStateManager._acquiredStates [0] = true;
		if (e.BrokeFree) {
			StartCoroutine (ChangeLevel ((int)SceneIndex.A0_7_Ending, 1.5f, "VoiceOver/15_Detrimental"));
		}
		else {
			StartCoroutine (ChangeLevel ((int)SceneIndex.A0_7_Ending, 1.5f, "VoiceOver/14_Terrible"));
		}
	}

	void LoadExecution(TriggerExecutionEvent e){
		if(e._ExecutionAsGuard){
			GameStateManager.gameStateManager._executionAsGuard = false;
		} else {
			GameStateManager.gameStateManager._executionAsGuard = true;
		}
		GameStateManager._acquiredStates [2] = true;
		GameStateManager.gameStateManager._actTitleIndex = 7;
		StartCoroutine(ChangeLevel((int)SceneIndex.A0_6_TitleCard, 0.5f));
		//StartCoroutine(ChangeLevel((int)SceneIndex.A4_1_Execution, 1f, "VoiceOver/10_BombCaught"));
	}

	void ReLoadExecution(RestartExecution e){
		StartCoroutine(ChangeLevel((int)SceneIndex.A4_1_Execution, 1f));
	}

	// When decide not to execute the prisoner
	void LoadExecutionEndBothDie(SoldierExecuteBoth e){
		StartCoroutine(ChangeLevel((int)SceneIndex.A0_7_Ending, 3f, "VoiceOver/12_NoChoice"));
	}

	void LoadExecutionEndPrisonerDie(GuardExecutePrisoner e){
		StartCoroutine(ChangeLevel((int)SceneIndex.A0_7_Ending, 4f, "VoiceOver/11_Unfortunate"));
	}

	void LoadTakenAway(TriggerTakenAwayEvent e){
		GameStateManager.gameStateManager._actTitleIndex = 8;
		StartCoroutine(ChangeLevel((int)SceneIndex.A0_6_TitleCard, 0.5f));
		//StartCoroutine(ChangeLevel((int)SceneIndex.A4_2_Ditch, 1f));
	}

	void LoadLeaveDitch(LeaveDitchEvent e){
		StartCoroutine(ChangeLevel((int)SceneIndex.A4_2_Ditch, 1f));
	}

	void LoadPlantBomb(TriggerPlantBombEvent e){
		StartCoroutine(ChangeLevel(8, 1f));
	}


	void LoadAllGunnedDown(DidNotShootEvent e){
	}
	void LoadPrisonerShotDown(PrisonerShotEvent e){
	}

	void LoadPlantUp(Plant_UpStairsEvent e){
		StartCoroutine(ChangeLevel((int)SceneIndex.A4_5_PlantBomb, 1f));
	}

	void LoadPlantDown(Plant_DownStairsEvent e){
		StartCoroutine(ChangeLevel((int)SceneIndex.A4_5_PlantBomb, 1f));
	}

	void LoadFoodStorage_Guard(Plant_EnterFoodStorageEvent e) {
		GameStateManager._acquiredStates [5] = true;
		StartCoroutine(ChangeLevel((int)SceneIndex.A4_5_PlantBomb, 1f));
	}

	void LoadLeaveFoodStorage(Plant_LeaveFoodStorageEvent e){
		GameStateManager.gameStateManager._actTitleIndex = 9;
		StartCoroutine(ChangeLevel((int)SceneIndex.A0_6_TitleCard, 0.5f));
		//StartCoroutine(ChangeLevel((int)SceneIndex.A4_6_PlantBombDone, 1f, "VoiceOver/22_DeedDone"));
	}

	void LoadEncounter1(Prisoner_EncounterEvent e){
		StartCoroutine(ChangeLevel((int)SceneIndex.A4_3_EncounterLeft, 3f));
	}

	void LoadEncounter2(Guard_EncounterEvent e){
		StartCoroutine(ChangeLevel((int)SceneIndex.A4_3_EncounterLeft, 3f));
	}

	//Ending
	void LoadEnd_RunAlone(RunAloneEndingEvent e){
		GameStateManager._acquiredStates [1] = true;
		StartCoroutine(ChangeLevel((int)SceneIndex.A0_7_Ending, 1.5f, "VoiceOver/18_LetGo"));
	}

	void LoadEnd_RunTogether(RunTogetherEndingEvent e){
		GameStateManager._acquiredStates [4] = true;
		StartCoroutine(ChangeLevel((int)SceneIndex.A0_7_Ending, 3f, "VoiceOver/16_EscapeTogether"));
	}

	void LoadEnd_GuardAlone(GuardAloneEndingEvent e){
		GameStateManager._acquiredStates [3] = true;
		StartCoroutine(ChangeLevel((int)SceneIndex.A0_7_Ending, 3f, "VoiceOver/17_JustJob"));
	}

	void LoadRetry(RetryEvent e){
		StartCoroutine(ChangeLevel((int)SceneIndex.A0_5_Title, 4f));
	}
		
	void LoadAct5(StartAct5Event e){
		StartCoroutine(ChangeFade((int)SceneIndex.A5_3_Ending, 5f));
	}

	void LoadCredits(StartCreditsEvent e){
		StartCoroutine(ChangeFade((int)SceneIndex.A0_8_Credits, 1f));
	}

	void OpenScreen(Scene scene, LoadSceneMode mode){
		_once = false;
		GameStateManager.gameStateManager._currScene = (SceneIndex)scene.buildIndex;
		if (scene.buildIndex == (int)SceneIndex.A0_2_TriggerWarning) {
		}
		//Capture all ending Scenes
		else if (scene.buildIndex == (int)SceneIndex.A0_7_Ending) {
		}
		else if (scene.name == "DemoEnding") {
		}
		else if (scene.name == "0-6_TitleCard") {
		}
		else if (scene.name == "MainMenu") {
		}
		else if (scene.buildIndex == (int)SceneIndex.A5_1_SecretRoom) {
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
			Debug.Log (index);
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

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Giverspace;
using UnityEditor;

public class SceneManagerScript : MonoBehaviour {

	enum SceneIndex {
		TriggerWarning = 0, 
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
		Act4_3_Encounter = 19
	};

	FrameScript _frameScript;
	bool _start = false;
	bool _once = false;
	[SerializeField] VoiceOverManager _voManager;

	void Update(){
		if(_start){
			if (Input.GetKeyDown (KeyCode.Space)) {
				_frameScript = GameObject.Find ("Frame").GetComponent<FrameScript> ();
				_frameScript.OpenFlap ();
				_start = false;
				Log.Metrics.Message("Start Time");
			}
		}
		if (Input.GetKeyDown (KeyCode.Minus)) {
			SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
		}
		if (Input.GetKeyDown (KeyCode.Equals)) {
			SceneManager.LoadScene (0);
		}
	}

	void OnEnable ()
	{
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

		Events.G.AddListener<Plant_UpStairsEvent>(LoadPlantUp);
		Events.G.AddListener<Plant_DownStairsEvent>(LoadPlantDown);
		Events.G.AddListener<Plant_EnterFoodStorageEvent>(LoadFoodStorage_Guard);
		Events.G.AddListener<Plant_LeaveFoodStorageEvent>(LoadLeaveFoodStorage);

		Events.G.AddListener<Prisoner_EncounterEvent>(LoadEncounter1);
		Events.G.AddListener<Guard_EncounterEvent>(LoadEncounter2);

		Events.G.AddListener<TriggerWarningEndEvent>(LoadAct0);

		SceneManager.sceneLoaded += OpenScreen;
	}

	void OnDisable ()
	{
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

		Events.G.RemoveListener<Plant_UpStairsEvent>(LoadPlantUp);
		Events.G.RemoveListener<Plant_DownStairsEvent>(LoadPlantDown);
		Events.G.RemoveListener<Plant_EnterFoodStorageEvent>(LoadFoodStorage_Guard);
		Events.G.RemoveListener<Plant_LeaveFoodStorageEvent>(LoadLeaveFoodStorage);

		Events.G.RemoveListener<Prisoner_EncounterEvent>(LoadEncounter1);
		Events.G.RemoveListener<Guard_EncounterEvent>(LoadEncounter2);

		Events.G.RemoveListener<TriggerWarningEndEvent>(LoadAct0);



		SceneManager.sceneLoaded -= OpenScreen;
	}


	void LoadAct0(TriggerWarningEndEvent e){
		StartCoroutine(ChangeFade((int)SceneIndex.Act0, 3f));
	}

	void LoadVertical(Act0EndedEvent e){
		Log.Metrics.Message("End Act 0");
		StartCoroutine(ChangeLevel((int)SceneIndex.Act1, 2f));
	}

	void OnGuardLeaveCell (GuardLeavingCellEvent e){
		Log.Metrics.Message("End Act 1");
		Log.Metrics.Message("CHOICE 1: Leave");
		StartCoroutine(ChangeLevel((int)SceneIndex.Title, 4.5f, "VoiceOver/01_NoDrown"));
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
		StartCoroutine(ChangeLevel((int)SceneIndex.Act2_Explore, 2f));
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
		StartCoroutine(ChangeLevel((int)SceneIndex.Act3_No, 1f));
	}

	void LoadWentBack(PrisonerWentBack e){
		StartCoroutine(ChangeLevel((int)SceneIndex.Act3_No, 1f));
	}

	void LoadGuardSleep(GuardSleepEvent e){
		StartCoroutine(ChangeLevel((int)SceneIndex.Act3_No, 1f));
	}

	void LoadAct3_Yes(PrisonerFoundBombEvent e){
		StartCoroutine(ChangeLevel((int)SceneIndex.Act3_Yes, 3f));
	}
	void LoadAct3_Plant(GuardFoundBombEvent e) {
		StartCoroutine(ChangeLevel((int)SceneIndex.Act3_Plant_Cell, 3f));
	}
	void LoadCaught(CaughtSneakingEvent e){
	}


	void LoadExecution(TriggerExecutionEvent e){
		StartCoroutine(ChangeLevel((int)SceneIndex.Act4_1, 1f));
	}
	void LoadTakenAway(TriggerTakenAwayEvent e){
		StartCoroutine(ChangeLevel((int)SceneIndex.Act4_2, 1f));
	}
	void LoadPlantBomb(TriggerPlantBombEvent e){
		StartCoroutine(ChangeLevel(8, 1f));
	}


	void LoadAllGunnedDown(DidNotShootEvent e){
	}
	void LoadPrisonerShotDown(PrisonerShotEvent e){
	}

	void LoadFoodStorage_Prisoner(Taken_EnterFoodStorageEvent e){
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
		StartCoroutine(ChangeLevel((int)SceneIndex.Act3_Plant_Done, 1f));
	}

	void LoadEncounter1(Prisoner_EncounterEvent e){
		StartCoroutine(ChangeLevel((int)SceneIndex.Act4_3_Encounter, 3f));
	}

	void LoadEncounter2(Guard_EncounterEvent e){
		StartCoroutine(ChangeLevel((int)SceneIndex.Act4_3_Encounter, 3f));
	}


	void OpenScreen(Scene scene, LoadSceneMode mode){
		if (scene.name == "TriggerWarning") {
		}
		else if (scene.name == "Act0") {
			_start = true;
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
		if (GameObject.Find ("Frame").GetComponent<FrameScript> ()!= null) {
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
				yield return new WaitForSeconds (1.0f);
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

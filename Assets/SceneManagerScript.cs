using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Giverspace;

public class SceneManagerScript : MonoBehaviour {

	FrameScript _frameScript;
	bool _start = false;
	bool _once = false;

	void Update(){
		if(_start){
			if (Input.GetKeyDown (KeyCode.Space)) {
				_frameScript = GameObject.Find ("Frame").GetComponent<FrameScript> ();
				_frameScript.OpenFlap ();
				_start = false;
				Log.Metrics.Message("Start Time");
			}
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

		Events.G.AddListener<SleepInCellEvent>(LoadAct3_No);
		Events.G.AddListener<PrisonerFoundBombEvent>(LoadAct3_Yes);
		Events.G.AddListener<GuardFoundBombEvent>(LoadAct3_Plant);
		Events.G.AddListener<CaughtSneakingEvent>(LoadCaught);

		Events.G.AddListener<TriggerExecutionEvent>(LoadExecution);
		Events.G.AddListener<TriggerTakenAwayEvent>(LoadTakenAway);
		Events.G.AddListener<TriggerPlantBombEvent>(LoadPlantBomb);

		Events.G.AddListener<DidNotShootEvent>(LoadAllGunnedDown);
		Events.G.AddListener<PrisonerShotEvent>(LoadPrisonerShotDown);

		Events.G.AddListener<Taken_EnterFoodStorageEvent>(LoadFoodStorage_Prisoner);

		Events.G.AddListener<Plant_EnterFoodStorageEvent>(LoadFoodStorage_Guard);
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

		Events.G.RemoveListener<SleepInCellEvent>(LoadAct3_No);
		Events.G.RemoveListener<PrisonerFoundBombEvent>(LoadAct3_Yes);
		Events.G.RemoveListener<GuardFoundBombEvent>(LoadAct3_Plant);
		Events.G.RemoveListener<CaughtSneakingEvent>(LoadCaught);

		Events.G.RemoveListener<TriggerExecutionEvent>(LoadExecution);
		Events.G.RemoveListener<TriggerTakenAwayEvent>(LoadTakenAway);
		Events.G.RemoveListener<TriggerPlantBombEvent>(LoadPlantBomb);

		Events.G.RemoveListener<DidNotShootEvent>(LoadAllGunnedDown);
		Events.G.RemoveListener<PrisonerShotEvent>(LoadPrisonerShotDown);

		Events.G.RemoveListener<Taken_EnterFoodStorageEvent>(LoadFoodStorage_Prisoner);

		Events.G.RemoveListener<Plant_EnterFoodStorageEvent>(LoadFoodStorage_Guard);
		Events.G.RemoveListener<TriggerWarningEndEvent>(LoadAct0);

		SceneManager.sceneLoaded -= OpenScreen;
	}


	void LoadAct0(TriggerWarningEndEvent e){
		print ("Load Act 0");
		StartCoroutine(ChangeFade(1, 2f));
	}

	void LoadVertical(Act0EndedEvent e){
		Log.Metrics.Message("End Act 0");
		StartCoroutine(ChangeLevel(2, 1.7f));
	}

	void OnGuardLeaveCell (GuardLeavingCellEvent e){
		Log.Metrics.Message("End Act 1");
		Log.Metrics.Message("CHOICE 1: Leave");
		StartCoroutine(ChangeLevel(4, 1.5f));
	}

	void LoadTitle(Act1EndedEvent e){
		Log.Metrics.Message("End Act 1");
		Log.Metrics.Message("CHOICE 1: Drown");
		StartCoroutine(ChangeLevel(4, 4f));
	}

		
	void LoadAct2(TitleEndedEvent e){
		StartCoroutine(ChangeLevel(4, 0.5f));
	}

	void LoadAct2Explore(Act2_PrisonerWalkedUpStairsEvent e){
//		StartCoroutine(ChangeLevel(4, 2f));
		//End PlayTest
		StartCoroutine(ChangeLevel(3, 2f));
	}

	void LoadAct2Explore_down(Act2_PrisonerWalkedDownStairsEvent e){
		StartCoroutine(ChangeLevel(4, 2f));
	}

	void LoadAct2Patrol(Act2_GuardWalkedUpStairsEvent e){
//		StartCoroutine(ChangeLevel(5, 2f));
		//End PlayTest
		StartCoroutine(ChangeLevel(3, 2f));
	}

	void LoadAct3_No(SleepInCellEvent e){
		Log.Metrics.Message("CHOICE 3: Bed");
//		StartCoroutine(ChangeLevel(4, 2f));
		//End PlayTest
		StartCoroutine(ChangeLevel(3, 1f));
	}
	void LoadAct3_Yes(PrisonerFoundBombEvent e){
		StartCoroutine(ChangeLevel(7, 3f));
	}
	void LoadAct3_Plant(GuardFoundBombEvent e) {
		StartCoroutine(ChangeLevel(6, 3f));
	}
	void LoadCaught(CaughtSneakingEvent e){
	}


	void LoadExecution(TriggerExecutionEvent e){
		StartCoroutine(ChangeLevel(8, 1f));
	}
	void LoadTakenAway(TriggerTakenAwayEvent e){
		StartCoroutine(ChangeLevel(8, 1f));
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

	void LoadFoodStorage_Guard(Plant_EnterFoodStorageEvent e) {
		StartCoroutine(ChangeLevel(8, 1f));
	}



	void OpenScreen(Scene scene, LoadSceneMode mode){
		if (scene.name == "TriggerWarning") {
		}
		else if (scene.name == "Level0") {
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

	IEnumerator ChangeLevel(int index, float duration){
		if (!_once) {
			_once = true;
			yield return new WaitForSeconds (duration);
			if (_frameScript != null) {
				_frameScript.CloseFlap ();
			}
			yield return new WaitForSeconds (1.0f);
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

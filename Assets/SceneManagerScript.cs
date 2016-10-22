﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void OnEnable ()
	{
		Events.G.AddListener<Act0EndedEvent>(LoadVertical);

		Events.G.AddListener<Act1EndedEvent>(LoadTitle);
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

	}

	void OnDisable ()
	{
		Events.G.RemoveListener<Act0EndedEvent>(LoadVertical);

		Events.G.RemoveListener<Act1EndedEvent>(LoadTitle);
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
	}

	void LoadVertical(Act0EndedEvent e){
		StartCoroutine(ChangeLevel(1, 1.7f));
	}

	void LoadTitle(Act1EndedEvent e){
		StartCoroutine(ChangeLevel(2, 4f));
	}
		
	void LoadAct2(TitleEndedEvent e){
		StartCoroutine(ChangeLevel(3, 0.5f));
	}

	void LoadAct2Explore(Act2_PrisonerWalkedUpStairsEvent e){
		StartCoroutine(ChangeLevel(4, 2f));
	}

	void LoadAct2Explore_down(Act2_PrisonerWalkedDownStairsEvent e){
		StartCoroutine(ChangeLevel(3, 2f));
	}

	void LoadAct2Patrol(Act2_GuardWalkedUpStairsEvent e){
		StartCoroutine(ChangeLevel(5, 2f));
	}

	void LoadAct3_No(SleepInCellEvent e){
		StartCoroutine(ChangeLevel(4, 3f));
	}
	void LoadAct3_Yes(PrisonerFoundBombEvent e){
		StartCoroutine(ChangeLevel(6, 3f));
	}
	void LoadAct3_Plant(GuardFoundBombEvent e) {
		StartCoroutine(ChangeLevel(5, 3f));
	}
	void LoadCaught(CaughtSneakingEvent e){
	}


	void LoadExecution(TriggerExecutionEvent e){
		StartCoroutine(ChangeLevel(7, 1f));
	}
	void LoadTakenAway(TriggerTakenAwayEvent e){
		StartCoroutine(ChangeLevel(7, 1f));
	}
	void LoadPlantBomb(TriggerPlantBombEvent e){
		StartCoroutine(ChangeLevel(7, 1f));
	}


	void LoadAllGunnedDown(DidNotShootEvent e){
	}
	void LoadPrisonerShotDown(PrisonerShotEvent e){
	}

	void LoadFoodStorage_Prisoner(Taken_EnterFoodStorageEvent e){
	}

	void LoadFoodStorage_Guard(Plant_EnterFoodStorageEvent e) {
		StartCoroutine(ChangeLevel(7, 1f));
	}



	IEnumerator ChangeLevel(int index, float duration){
		yield return new WaitForSeconds(duration);
		float fadeTime = GameObject.Find ("Fade").GetComponent<Fading> ().BeginFade (1);
		yield return new WaitForSeconds(fadeTime);
		SceneManager.LoadScene (index);
	}
}

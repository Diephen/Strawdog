using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void OnEnable ()
	{
		Events.G.AddListener<Act1EndedEvent>(LoadAct2);
		Events.G.AddListener<SleepInCellEvent>(LoadAct3_No);
		Events.G.AddListener<PrisonerFoundBombEvent>(LoadAct3_Yes);
		Events.G.AddListener<GuardFoundBombEvent>(LoadAct3_Plant);
		Events.G.AddListener<CaughtSneakingEvent>(LoadCaught);
	}

	void OnDisable ()
	{
		Events.G.RemoveListener<Act1EndedEvent>(LoadAct2);
		Events.G.RemoveListener<SleepInCellEvent>(LoadAct3_No);
		Events.G.RemoveListener<PrisonerFoundBombEvent>(LoadAct3_Yes);
		Events.G.RemoveListener<GuardFoundBombEvent>(LoadAct3_Plant);
		Events.G.RemoveListener<CaughtSneakingEvent>(LoadCaught);
	}
		
	void LoadAct2(Act1EndedEvent e){
	}

	void LoadAct3_No(SleepInCellEvent e){
	}

	void LoadAct3_Yes(PrisonerFoundBombEvent e){
	}

	void LoadAct3_Plant(GuardFoundBombEvent e) {
	}

	void LoadCaught(CaughtSneakingEvent e){
	}

}

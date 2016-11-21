using UnityEngine;
using System.Collections;

public class SoldierDragHandle : MonoBehaviour {
	Animator m_Anim;


	void OnEnable(){
		Events.G.AddListener<CallGuardInCell> (OnPrisonerSleepInCell);
	}

	void OnDisable(){
		Events.G.RemoveListener<CallGuardInCell> (OnPrisonerSleepInCell);
	}

	//

	// Use this for initialization
	void Start () {
		m_Anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnPrisonerSleepInCell(CallGuardInCell e){
		print ("Soldier Walks in");
		m_Anim.Play ("soldier-jc-drag");
	}

	void DragPrisoner(){
		Events.G.Raise (new DragPrisonerInJail ());
	}

	void OnEndJailSequence(){
		// fire end scene event
		Events.G.Raise(new PrisonerSleepEvent());
	}
}

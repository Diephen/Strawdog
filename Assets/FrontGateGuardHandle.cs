using UnityEngine;
using System.Collections;

public class FrontGateGuardHandle : MonoBehaviour {
	private enum greetState{
		idle, salute, talk, endsalute
	} 
	private greetState m_SoldierState = greetState.idle;
	[SerializeField] Animator m_Anim;
	private bool isGateOpen = false;
	[SerializeField] GameObject m_GateBlock;

	// Use this for initialization
	void Start () {
		if (m_Anim == null && GetComponent<Animator> ()) {
			m_Anim = GetComponent<Animator> ();
		} else {
			Debug.Log ("[Error] No animator");
		}

	}
	
	// Update is called once per frame
	void Update () {
		if (m_SoldierState == greetState.salute) {
			if (Input.GetKeyDown (KeyCode.K)) {
				m_SoldierState = greetState.talk;
				CheckState (m_SoldierState);
			}
		}
	
	}

	void CheckState(greetState stat){
		if (m_Anim != null) {
			switch (stat) {
			case greetState.idle:
				break;
			case greetState.salute:
				m_Anim.SetBool ("IsSalute", true);
				Debug.Log ("");
				break;
			case greetState.talk:
				if (!isGateOpen) {
					isGateOpen = true;
					m_Anim.SetTrigger ("TriggerTalk");
					OpenGate ();
				}
				break;
			case greetState.endsalute:
				m_Anim.SetBool ("IsSalute", false);
				break;
			}
		}
	
	}
	void OnTriggerEnter2D(Collider2D other) {
		//Debug.Log ("Attention");
		if(m_SoldierState == greetState.idle && other.name == "GuardStructure"){
			m_SoldierState = greetState.salute;
			CheckState (m_SoldierState);
			Debug.Log ("Attention");
		}
			
	}
		

	void OnTriggerExit2D(Collider2D other) {
		//Debug.Log ("Guard Pass");
		if(other.name == "GuardStructure"){
			m_SoldierState = greetState.endsalute;
			Debug.Log ("Guard Pass");
			CheckState (m_SoldierState);
		}

	}
		

	void OpenGate(){
		Debug.Log ("Open Gate Request");
		m_GateBlock.SetActive (false);
	}
}

using UnityEngine;
using System.Collections;

public class DitchSoldierAI : MonoBehaviour {
	CutDeathPrisoner m_CurrentCP;
	CutDeathPrisoner m_NextCP;
	Animator m_SoldierAnim;
	[SerializeField] DitchPrisonerHandle m_PrisonerHandle;

	bool m_IsPrisoner = false;
	bool m_IsCutting = false;
	// Use this for initialization
	void Awake(){
		m_CurrentCP = m_NextCP = null;
		m_SoldierAnim = GetComponent<Animator> (); 
	}


	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Prisoner") {
			// Cut prisoner
			m_IsPrisoner = true;
			StartExecution ();

		}else if(other.tag == "CutPrisoner"){
			// Cut shoot prisoner 
			m_CurrentCP = other.gameObject.GetComponentInChildren<CutDeathPrisoner>();
			CutCPInLine();
		}

	}

	void OnTriggerStay2D(Collider2D other){
	}


	void OnTriggerExit2D(Collider2D other) {
	}

	void StartExecution(){
		m_IsCutting = true;
		if (m_IsPrisoner) {
			// rearrange cutting order: Prisoner first 
			if (m_CurrentCP != null) {
				m_NextCP = m_CurrentCP;
				m_CurrentCP = null;
			}
			CutPrisoner ();

		} else {
			
		

		}

	}

	void EndExecution(){
		m_IsCutting = false;

	}

	void CutPrisoner(){
		m_IsCutting = false;
		m_SoldierAnim.Play ("soldier-ditch-Prepare");
		m_PrisonerHandle.PrisonerStop ();
	}

	void CutCPInLine(){
		m_CurrentCP.Death ();
	}

	void Kill(){
		m_PrisonerHandle.Death ();
	}

	void Kick(){
		m_PrisonerHandle.Kick ();
	}


}

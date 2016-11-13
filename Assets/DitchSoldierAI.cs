using UnityEngine;
using System.Collections;

public class DitchSoldierAI : MonoBehaviour {
	CutDeathPrisoner m_CurrentCP;
	CutDeathPrisoner m_NextCP;
	Animator m_SoldierAnim;
	[SerializeField] DitchPrisonerHandle m_PrisonerHandle;
	[SerializeField] PuppetControl m_PC;


	bool m_IsPrisoner = false;
	bool m_IsCutting = false;
	bool m_IsLineArrived = false;       // if the line arrives the end 
	bool m_IsPrisonerFree = false;

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

	void OnEnable(){
		Events.G.AddListener<BrokeFree> (OnPrisonerBreakFree);
	}

	void OnDisable(){
		Events.G.RemoveListener<BrokeFree> (OnPrisonerBreakFree);
	}

	void OnPrisonerBreakFree(BrokeFree e){
		m_IsPrisonerFree = true;
		
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (m_IsLineArrived) {
			Events.G.Raise (new LineControlEvent (true));
		}
		if (other.tag == "Prisoner") {
			// Cut prisoner
			m_IsPrisoner = true;
			Events.G.Raise (new CutPrisonerBrforeOthers ());
			StartExecution ();

		}else if(other.tag == "CutPrisoner"){
			// Cut shoot prisoner 

			m_CurrentCP = other.gameObject.GetComponentInChildren<CutDeathPrisoner>();
			StartExecution ();
		}

		if(!m_IsLineArrived){
			m_IsLineArrived = true;
		}

	

	}

	void OnTriggerStay2D(Collider2D other){
	}


	void OnTriggerExit2D(Collider2D other) {
	}

	void StartExecution(){
		m_IsCutting = true;
		if (m_IsPrisoner) {
			m_PC.DisableKeyInput ();
			// rearrange cutting order: Prisoner first 
			if (m_CurrentCP != null) {
				
				// pause this prisoner
				m_NextCP = m_CurrentCP;
				m_CurrentCP = null;
			}
			CutPrisoner ();
			m_IsPrisoner = false;
		} else {
			CutCPInLine();
		

		}

	}

	void EndExecution(){
		m_IsCutting = false;
		Events.G.Raise (new LineControlEvent (false));
//		if (m_NextCP != null) {
//			print ("CP in line");
//			//m_CurrentCP = m_NextCP;
//			//m_NextCP = null;
//			//Events.G.Raise (new LineControlEvent (true));
//			//CutCPInLine ();
//		} else {
//			Events.G.Raise (new LineControlEvent (false));
//		}
	}

	void CutPrisoner(){
		//m_IsCutting = false;
		m_SoldierAnim.Play ("soldier-ditch-Prepare");
		m_PrisonerHandle.PrisonerStop ();
	}

	void CutCPInLine(){
		m_SoldierAnim.Play ("soldier-ditch-CutCP");

		//m_IsCutting = false;

	}

	void KillCP(){
		m_CurrentCP.Death ();
		m_CurrentCP = null;
	}

	void Kill(){
		m_PrisonerHandle.Death ();
	}

	void Kick(){
		m_PrisonerHandle.Kick ();
	}

	void EndScene(){
		Events.G.Raise (new Taken_EnterFoodStorageEvent (m_IsPrisonerFree));
	}


}

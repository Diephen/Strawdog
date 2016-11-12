using UnityEngine;
using System.Collections;

public enum ExecutionType{
	Prisoner,
	ShootPrisoner
}

public class ExecutionGuardHandle : MonoBehaviour {
	Animator m_Anim;
	AnimationControl m_AnimCtrl;
	AnimationInjectionExecution m_AnimInjection;
	[SerializeField] InteractionSound m_ItrSound;
	[SerializeField] ExecutionPrisonerHandle m_PrisonerHandle;
	ShotDeathPrisonerHandle m_CurrentSP;
	ShotDeathPrisonerHandle m_NextInLine = null;
	[SerializeField] Transform[] m_GuardStartPos;
	[SerializeField] GameObject[] m_TeachUI;

	// TODO: UI Shooting toturial 



	bool _execute = false;
	bool m_IsPrisoner = false;
	int _shootCnt = 0;
	int _shootSwitchCnt = 2;

	bool m_IsHandUp = false;
	bool m_IsPrisonerStandUp = false;
	bool m_IsTeaching = true;




	// Use this for initialization
	void Start () {
		m_Anim = GetComponent<Animator> ();
		m_AnimCtrl = GetComponent<AnimationControl> ();
		m_AnimInjection = GetComponent<AnimationInjectionExecution> ();
		//Events.G.Raise (new DisableMoveEvent (CharacterIdentity.Guard));
		transform.position = m_GuardStartPos[0].position;
		m_TeachUI [0].SetActive (false);

	}

	void OnEnable(){
		Events.G.AddListener<ExecutionEncounter> (OnExecutionEncounter);
		Events.G.AddListener<ExecutionBreakFree> (OnPrisonerBreak);
	}

	void OnDisable(){
		Events.G.RemoveListener<ExecutionEncounter> (OnExecutionEncounter);
		Events.G.RemoveListener<ExecutionBreakFree> (OnPrisonerBreak);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnPrisonerBreak(ExecutionBreakFree e){
		m_IsPrisonerStandUp = true;
	}



	void FixedUpdate(){
		if (_shootCnt == _shootSwitchCnt) {
			Events.G.Raise (new ShootSwitchEvent ());
			//print ("Move Gre;iojvnweioqupnvieuvqonpuieopnutvpq!!!");
			//Events.G.Raise (new EnableMoveEvent (CharacterIdentity.Guard));
			m_IsTeaching = false;
			//not needed but code added to not have called multiple times
			_shootCnt++;
		}
	}

	void OnExecutionEncounter(ExecutionEncounter e){
		if (e.IsStart) {
			print ("GH Start Exe");
			// TODO Prisoner has piority over cubes
			if (e.ExeType == ExecutionType.ShootPrisoner) {
				if (!m_IsPrisoner) {
					_execute = true;
					m_IsPrisoner = false;
					if (m_NextInLine == null) {
						m_CurrentSP = e.SP;
					} else {
						m_CurrentSP = m_NextInLine;
						m_NextInLine = null;
					}
					StartExecution ();
				} else {
					m_NextInLine = e.SP;
					m_NextInLine = null;
				}

			} else if(e.ExeType == ExecutionType.Prisoner){
				m_Anim.SetBool ("IsPStand", m_IsPrisonerStandUp);
				if (!_execute) {
					_execute = true;
				} else {
					m_NextInLine = m_CurrentSP;
				}
				m_CurrentSP = null;
				m_IsPrisoner = true;

				StartExecution ();
				m_PrisonerHandle.EncounterGuard ();
			}
		} else {
			
			if (e.ExeType == ExecutionType.ShootPrisoner) {
				m_CurrentSP = null;

			} else {
				m_CurrentSP = null;
				m_IsPrisonerStandUp = false;
				m_Anim.SetBool ("IsPStand", m_IsPrisonerStandUp);
				//m_IsPrisoner = false;
				//TODO - Add ending Condition
				print("End of Execution Scene!!!!");
			}
		}
	}

	public void StartExecution(){
		m_Anim.Play ("g-ShootIdle");
		m_Anim.SetBool ("IsHandUp", false);
		m_IsHandUp = false;
		m_AnimCtrl.SetAnimation (true);
		m_AnimInjection.SetEngage ();
	}


	public void EndExecution(){
		m_Anim.SetBool ("IsHandUp", false);
		m_Anim.SetBool ("IsPStand", false);
		m_IsHandUp = false;
		m_AnimCtrl.SetAnimation (false);
		m_AnimInjection.SetLeave ();
		if (m_NextInLine != null) {
			m_CurrentSP = m_NextInLine;
			m_NextInLine = null;
			_execute = true;
			StartExecution ();
		}

		if (_shootCnt == 3){
			//print ("Move Gre;iojvnweioqupnvieuvqonpuieopnutvpq!!!");
			Events.G.Raise (new EnableMoveEvent (CharacterIdentity.Guard));
		}

	}

	public void HandUp(){
		m_IsHandUp = true;
		//m_Anim.SetBool ("IsPStand", m_IsPrisonerStandUp);
		m_Anim.SetBool ("IsHandUp", true);
		if (m_IsTeaching) {
			m_TeachUI [0].SetActive (true);
			m_TeachUI [1].SetActive (false);
		}
	}

	public void HandDown(){
		m_IsHandUp = false;
		m_Anim.SetBool ("IsHandUp", false);
		if (m_IsTeaching) {
			m_TeachUI [1].SetActive (true);
			m_TeachUI [0].SetActive (false);
		}
	}

	public void Shoot(){
		if (m_IsHandUp && _execute) {
			if (m_IsTeaching) {
				m_TeachUI [0].SetActive (false);
				m_TeachUI [1].SetActive (false);
			}
			if (!m_IsPrisoner) {
				m_Anim.Play ("g-Shoot");
				m_ItrSound.PlayGun ();
				m_IsHandUp = false;
				m_CurrentSP.Executed ();
				_shootCnt++;
				_execute = false;
				m_CurrentSP = null;
				if (_shootCnt == 1) {
					this.transform.position = m_GuardStartPos [1].position;
				} else if (_shootCnt == 2){
					//Events.G.Raise (new EnableMoveEvent (CharacterIdentity.Guard));
				}
			} else {
				print ("Call Prisoner Animation");
				m_PrisonerHandle.Death ();
				m_Anim.Play ("g-Shoot");
				m_ItrSound.PlayGun ();
				m_IsHandUp = false;
				_execute = false;
				_shootCnt++;
				m_IsPrisoner = false;
				m_IsPrisonerStandUp = false;
				m_Anim.SetBool ("IsPStand", false);
			}

		}
	}

	void PlayReloadAfterShoot(){
		m_ItrSound.PlayReload ();
		if (m_IsTeaching) {
			m_TeachUI [0].SetActive (true);
			m_TeachUI [1].SetActive (true);
		}
	}

	public void Death(){
		print ("ShotBySoldier");
		m_Anim.Play ("g-exe-ShotBySoldier");
	}


}

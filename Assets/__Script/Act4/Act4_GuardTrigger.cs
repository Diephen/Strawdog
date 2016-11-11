using UnityEngine;
using System.Collections;

public class Act4_GuardTrigger : MonoBehaviour {
	[SerializeField] Animator m_Anim;
	[SerializeField] AnimationControl m_AnimCtrl;
	[SerializeField] GameObject _guard;
	[SerializeField] AnimationInjectionExecution m_AnimInjection;
	[SerializeField] InteractionSound m_ItrSound;
	[SerializeField] ExecutionPrisonerHandle m_PrisonerHandle;
	PuppetControl _guardPuppetController;
	KeyCode[] _guardKeyCodes;
	//GameObject _otherGameObject = null;
	ShotDeathPrisonerHandle m_CurrentSP = null;
	bool _execute = false;
	bool m_IsPrisoner = false;
	int _shootCnt = 0;
	int _shootSwitchCnt = 2;


	bool m_IsHandUp = false;

	// UI testing & input
	



	void Start(){
		_guardPuppetController = _guard.GetComponent<PuppetControl> ();
		_guardKeyCodes = _guardPuppetController.GetKeyCodes ();
	}

	void Update(){
		// test ui input 

//		if (_execute) {
//			if (Input.GetKey (_guardKeyCodes [0])) {
//				if (Input.GetKey (_guardKeyCodes [3])) {
//			
//				}
//			}
//		}
	}

//	void FixedUpdate(){
//		if (_shootCnt == _shootSwitchCnt) {
//			Events.G.Raise (new ShootSwitchEvent ());
//			//not needed but code added to not have called multiple times
//			_shootCnt++;
//		}
//	}


	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Execution") {
			//print ("Exe!!!");
			_guardPuppetController.StopWalkAudio();
			Events.G.Raise (new ExecutionEncounter (ExecutionType.ShootPrisoner, other.gameObject.GetComponent<ShotDeathPrisonerHandle> (), true));
//			StartExecution ();
//			_execute = true;
//			//_otherGameObject = other.gameObject;
//			m_CurrentSP = other.gameObject.GetComponent<ShotDeathPrisonerHandle> ();
			//m_AnimInjection.SetEngage ();

		} else if (other.tag == "Prisoner") {
			_guardPuppetController.StopWalkAudio();
			Events.G.Raise (new ExecutionEncounter (ExecutionType.Prisoner, null, true));
//			StartExecution ();
//			_execute = true;
//			m_IsPrisoner = true;
			//m_AnimInjection.SetEngage ();
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.tag == "Execution") {
			Events.G.Raise (new ExecutionEncounter (ExecutionType.ShootPrisoner, null, false));
			//_execute = false;
			//_otherGameObject = null;
			//m_CurrentSP = null;
		} else if (other.tag == "Prisoner") {
			Events.G.Raise (new ExecutionEncounter (ExecutionType.Prisoner,null,  false));

		}
	}

	void OnEnable()
	{
//		Events.G.AddListener<Prisoner_EncounterEvent>(PrisonerEncounter);
	}

	void OnDisable ()
	{
//		Events.G.RemoveListener<Prisoner_EncounterEvent>(PrisonerEncounter);
	}

//	public void StartExecution(){
//		m_Anim.Play ("g-ShootIdle");
//		m_Anim.SetBool ("IsHandUp", false);
//		m_IsHandUp = false;
//		m_AnimCtrl.SetAnimation (true);
//		m_AnimInjection.SetEngage ();
//	}
//
//
//	public void EndExecution(){
//		m_Anim.SetBool ("IsHandUp", false);
//		m_IsHandUp = false;
//		m_AnimCtrl.SetAnimation (false);
//		m_AnimInjection.SetLeave ();
//
//	}

//	public void HandUp(){
//		m_IsHandUp = true;
//		m_Anim.SetBool ("IsHandUp", true);
//		//m_ItrSound.PlayReload ();
//	}
//
//	public void HandDown(){
//		m_IsHandUp = false;
//		m_Anim.SetBool ("IsHandUp", false);
//	}
//
//
//	public void Shoot(){
//		if (m_IsHandUp && _execute) {
//			if (!m_IsPrisoner) {
//				m_Anim.Play ("g-Shoot");
//				m_ItrSound.PlayGun ();
//				m_IsHandUp = false;
//				//_otherGameObject.gameObject.SetActive (false);
//				m_CurrentSP.Executed ();
//				//Events.G.Raise (new EnableMoveEvent ());
//				_shootCnt++;
//				_execute = false;
//				m_CurrentSP = null;
//			} else {
//				print ("Call Prisoner Animation");
//
//			}
//		
//		}
//	}
		

}

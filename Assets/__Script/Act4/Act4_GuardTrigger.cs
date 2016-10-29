using UnityEngine;
using System.Collections;

public class Act4_GuardTrigger : MonoBehaviour {

	[SerializeField] GameObject _guard;
	PuppetControl _guardPuppetController;
	KeyCode[] _guardKeyCodes;
	//GameObject _otherGameObject = null;
	ShotDeathPrisonerHandle m_CurrentSP = null;
	bool _execute = false;
	int _shootCnt = 0;
	int _shootSwitchCnt = 2;

	void Start(){
		_guardPuppetController = _guard.GetComponent<PuppetControl> ();
		_guardKeyCodes = _guardPuppetController.GetKeyCodes ();
	}

	void Update(){
		if (_execute) {
			if (Input.GetKey (_guardKeyCodes [0])) {
				if (Input.GetKey (_guardKeyCodes [3])) {
					//_otherGameObject.gameObject.SetActive (false);
					m_CurrentSP.Executed();
					Events.G.Raise (new EnableMoveEvent ());
					_shootCnt++;
					_execute = false;
					m_CurrentSP = null;
				}
			}
		}
	}

	void FixedUpdate(){
		if (_shootCnt == _shootSwitchCnt) {
			Events.G.Raise (new ShootSwitchEvent ());
			//not needed but code added to not have called multiple times
			_shootCnt++;
		}
	}


	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Execution") {
			print ("Exe!!!");
			_execute = true;
			//_otherGameObject = other.gameObject;
			m_CurrentSP = other.gameObject.GetComponent<ShotDeathPrisonerHandle>();
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.tag == "Execution") {
			_execute = false;
			//_otherGameObject = null;
			//m_CurrentSP = null;
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
}

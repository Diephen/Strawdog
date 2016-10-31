using UnityEngine;
using System.Collections;

public class Interrogation : MonoBehaviour {
	enum InterrogationType{
		prisoner_yes,
		prisoner_no,
		guard
	}
	//[SerializeField] bool _bombFound = false;
	[SerializeField] InterrogationType m_SceneType;
	[SerializeField] float _duration;
	[SerializeField] InterrogationPrisonerHandler m_IP;
	[SerializeField] InterrogationGuardHandle m_IG;
	[SerializeField] GameObject m_ClockHand;
	Timer _interrogationTimer;
	// Use this for initialization
	void Awake () {
		_interrogationTimer = new Timer (_duration);
		_interrogationTimer.Reset ();
	}

	void Start(){
		InitPuppetState ();
	}

	void InitPuppetState(){
		if (m_SceneType == InterrogationType.prisoner_yes) {
			// no note reading
			m_IP.SetBombState(true);
			_duration = 20f;
		}

		if (m_SceneType == InterrogationType.prisoner_no) {
			// read note
			m_IP.SetBombState(false);
		}

		if (m_SceneType == InterrogationType.guard) {
			// no note reading 
			// no scene transition
			// still minimum control
			m_IP.SetBombState(false);
		}

	}
	
	// Update is called once per frame
	void Update () {
		//print (_interrogationTimer.TimeLeft);
		if(m_SceneType != InterrogationType.guard){
			if (_interrogationTimer.IsOffCooldown) {
				// call last animation 
				m_IG.EndInterrogation();

			} else {
				ClockTick ();
			} 
		}

	}

	void ClockTick(){
		float rotateSpeed = 360 / _duration;
		Vector3 eularAngle = m_ClockHand.transform.eulerAngles;
		eularAngle.z -= rotateSpeed * Time.deltaTime;
		m_ClockHand.transform.rotation= Quaternion.Euler(eularAngle);
	}

	public void NextScene(){
		if (m_SceneType == InterrogationType.prisoner_yes) {
			print ("Go to execution");
			Events.G.Raise (new TriggerExecutionEvent ());

		} else if(m_SceneType == InterrogationType.prisoner_no){
			print ("Go to Ditch");
			Events.G.Raise (new TriggerTakenAwayEvent ());
		}
	}

}

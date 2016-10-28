using UnityEngine;
using System.Collections;

public class Interrogation : MonoBehaviour {
	[SerializeField] bool _bombFound = false;
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
	
	// Update is called once per frame
	void Update () {
		//print (_interrogationTimer.TimeLeft);
		if (_interrogationTimer.IsOffCooldown) {
			if (_bombFound) {
				print ("Go to execution");
				Events.G.Raise (new TriggerExecutionEvent ());
			
			} else {
				print ("Go to Ditch");
				Events.G.Raise (new TriggerTakenAwayEvent ());
			}
		} else {
			ClockTick ();
		} 
	}

	void ClockTick(){
		float rotateSpeed = 360 / _duration;
		Vector3 eularAngle = m_ClockHand.transform.eulerAngles;
		eularAngle.z -= rotateSpeed * Time.deltaTime;
		m_ClockHand.transform.rotation= Quaternion.Euler(eularAngle);
	}

}

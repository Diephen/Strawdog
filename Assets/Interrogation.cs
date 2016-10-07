using UnityEngine;
using System.Collections;

public class Interrogation : MonoBehaviour {
	[SerializeField] bool _bombFound = false;
	[SerializeField] float _duration = 3.0f;
	Timer _interrogationTimer;
	// Use this for initialization
	void Start () {
		_interrogationTimer = new Timer (_duration);
	}
	
	// Update is called once per frame
	void Update () {
		if (_interrogationTimer.IsOffCooldown) {
			if (_bombFound) {
				Events.G.Raise (new TriggerExecutionEvent ());
			} else {
				Events.G.Raise (new TriggerTakenAwayEvent ());
			}
		}
	}
}

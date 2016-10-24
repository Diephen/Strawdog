using UnityEngine;
using System.Collections;

public class TriggerWarning : MonoBehaviour {
	[SerializeField] float duration;
	float _startTime;
	bool isEnd = false;
	// Use this for initialization
	void Start () {
		_startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time - _startTime >= duration && !isEnd) {
			isEnd = true;
			print ("End of trigger warning");
			Events.G.Raise (new TriggerWarningEndEvent ());
		}
	}
}

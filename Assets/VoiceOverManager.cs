using UnityEngine;
using System.Collections;

public class VoiceOverManager : MonoBehaviour {

	AudioSource _voiceOverSource;
	// Use this for initialization
	void Start () {
		_voiceOverSource = gameObject.GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnEnable()
	{
//		Events.G.AddListener<LightOffEvent> (StopCaught1);
	}

	void OnDisable ()
	{
//		Events.G.RemoveListener<GuardEnteringCellEvent>(OnGuardEnterCell);
	}
}

using UnityEngine;
using System.Collections;

public class VoiceOverManager : MonoBehaviour {

	AudioSource _voiceOverSource;
	// Use this for initialization
	void Start () {
		_voiceOverSource = gameObject.GetComponent<AudioSource> ();
	}


//	void OnEnable()
//	{
//		Events.G.AddListener<Act1EndedEvent> (PlayDrown);
//		Events.G.AddListener<GuardLeavingCellEvent> (PlayLeft);
//	}
//
//	void OnDisable ()
//	{
//		Events.G.RemoveListener<Act1EndedEvent>(PlayDrown);
//		Events.G.RemoveListener<GuardLeavingCellEvent>(PlayLeft);
//	}
//
//	void PlayDrown(Act1EndedEvent e){
//		_voiceOverSource.clip = Resources.Load<AudioClip>("VoiceOver/03_Drown");
//		_voiceOverSource.Play ();
//	}
//
//	void PlayLeft(GuardLeavingCellEvent e){
//		_voiceOverSource.clip = Resources.Load<AudioClip>("VoiceOver/01_NoDrown");
//		_voiceOverSource.Play ();
//	}

	public float PlayVoiceOver(string path){
		_voiceOverSource.clip = Resources.Load<AudioClip>(path);
		_voiceOverSource.Play ();
		return _voiceOverSource.clip.length;
	}

	void LeftUnlockPlay(LeftCellUnlockedEvent e){
		//_voiceOverSource.clip = Resources.Load<AudioClip>("VoiceOver/04_Unlock");
		//_voiceOverSource.Play ();
	}

	void OnEnable(){
		Events.G.AddListener<LeftCellUnlockedEvent>(LeftUnlockPlay);
	}

	void OnDisable(){
		Events.G.RemoveListener<LeftCellUnlockedEvent>(LeftUnlockPlay);
	}
}

using UnityEngine;
using System.Collections;

public class AudioController : MonoBehaviour {
	AudioSource _audioSource;
	[SerializeField] float[] volume = new float[] {0.1f, 0.3f, 0.7f};
	// Use this for initialization
	void Start () {
		_audioSource = gameObject.GetComponent <AudioSource> ();
		_audioSource.volume = volume [0];

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void SmoothVolume(AudioSource audioSource, float goal){
		if (goal > audioSource.volume) {
			while (goal >= audioSource.volume) {
				audioSource.volume = audioSource.volume + 0.0001f;
			}
		} else {
			while (goal <= audioSource.volume) {
				audioSource.volume = audioSource.volume - 0.0001f;
			}
		}
	}

	void OnEnable ()
	{
		Events.G.AddListener<GuardEnteringCellEvent>(OnGuardEnterCell);
		Events.G.AddListener<GuardEngaginPrisonerEvent>(OnGuardEngagePrisoner);
	}

	void OnDisable ()
	{
		Events.G.RemoveListener<GuardEnteringCellEvent>(OnGuardEnterCell);
		Events.G.RemoveListener<GuardEngaginPrisonerEvent>(OnGuardEngagePrisoner);
	}

	void OnGuardEnterCell (GuardEnteringCellEvent e)
	{
//		_audioSource.volume = volume [1];
		SmoothVolume (_audioSource, volume[1]);

	}

	void OnGuardEngagePrisoner (GuardEngaginPrisonerEvent e)
	{
		if (e.Engaged) {
//			_audioSource.volume = volume [2];
			SmoothVolume (_audioSource, volume[2]);
		} else {
//			_audioSource.volume = volume [1];
			SmoothVolume (_audioSource, volume[1]);
		}
	}
}

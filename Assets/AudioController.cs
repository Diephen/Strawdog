using UnityEngine;
using System.Collections;

public class AudioController : MonoBehaviour {
	[SerializeField] float[] volume = new float[] {0.1f, 0.3f, 0.7f};
	[SerializeField] AudioSource _audioSource;
	[SerializeField] AudioSource _soundSource1;
	AudioSource _tempAudioSource;
	float _goalVolume;

	void Start () {
		_audioSource.volume = volume [0];
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (_tempAudioSource != null && !Mathf.Approximately (_tempAudioSource.volume, _goalVolume)) 
		{
			if (_tempAudioSource.volume > _goalVolume) 
			{
				_tempAudioSource.volume = _tempAudioSource.volume - 0.01f;
			} 
			else 
			{
				_tempAudioSource.volume = _tempAudioSource.volume + 0.01f;
			}
		}
	}

	void OnEnable ()
	{
		Events.G.AddListener<GuardEnteringCellEvent>(OnGuardEnterCell);
		Events.G.AddListener<GuardEngaginPrisonerEvent>(OnGuardEngagePrisoner);

		//Act2
		Events.G.AddListener<LockCellEvent>(OnGuardEngagePrisoner);
		Events.G.AddListener<PrisonerFoundBombEvent>(OnGuardEngagePrisoner);
		Events.G.AddListener<GuardFoundBombEvent>(OnGuardEngagePrisoner);
		Events.G.AddListener<Act2_GuardWalkedUpStairsEvent>(OnGuardEngagePrisoner);
		Events.G.AddListener<Act2_PrisonerWalkedUpStairsEvent>(OnGuardEngagePrisoner);
		Events.G.AddListener<Act2_PrisonerWalkedDownStairsEvent>(OnGuardEngagePrisoner);
	}

	void OnDisable ()
	{
		Events.G.RemoveListener<GuardEnteringCellEvent>(OnGuardEnterCell);
		Events.G.RemoveListener<GuardEngaginPrisonerEvent>(OnGuardEngagePrisoner);

		//Act2
		Events.G.RemoveListener<LockCellEvent>(OnGuardEngagePrisoner);
		Events.G.RemoveListener<PrisonerFoundBombEvent>(OnGuardEngagePrisoner);
		Events.G.RemoveListener<GuardFoundBombEvent>(OnGuardEngagePrisoner);
		Events.G.RemoveListener<Act2_GuardWalkedUpStairsEvent>(OnGuardEngagePrisoner);
		Events.G.RemoveListener<Act2_PrisonerWalkedUpStairsEvent>(OnGuardEngagePrisoner);
		Events.G.RemoveListener<Act2_PrisonerWalkedDownStairsEvent>(OnGuardEngagePrisoner);
	}

	void OnGuardEnterCell (GuardEnteringCellEvent e)
	{
		_tempAudioSource = _audioSource;
		_goalVolume = volume [1];
	}

	void OnGuardEngagePrisoner (GuardEngaginPrisonerEvent e)
	{
		if (e.Engaged) 
		{
			_tempAudioSource = _audioSource;
			_goalVolume = volume [2];
		} 
		else 
		{
			_tempAudioSource = _audioSource;
			_goalVolume = volume [1];
		}
	}
}
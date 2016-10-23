using UnityEngine;
using System.Collections;

public class AudioController : MonoBehaviour {
	[SerializeField] float[] volume = new float[] {0.1f, 0.3f, 0.7f};
	[SerializeField] AudioSource _audioSource;
	[SerializeField] AudioSource _soundSource1;
	[SerializeField] AudioSource _soundSource2;
	AudioSource _tempAudioSource;
	float _goalVolume;
	[SerializeField] AudioClip _lockCell;
	[SerializeField] AudioClip _openCell;
	[SerializeField] AudioClip _pickUpBomb;
	[SerializeField] AudioClip _guardStairs;
	[SerializeField] AudioClip _prisonerStairs;

	bool _soundOff = true;
	Timer _soundOffTimer = new Timer(0.5f);

	static AudioController _instance = null;

	void Awake() {
		if (_instance) {
			Destroy (gameObject);
		}
		else {
			_instance = this;
			DontDestroyOnLoad (gameObject);
		}
	}

	void Start () {
		_audioSource.volume = volume [0];
	}
	
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

		if (_soundOff) {
			if (Mathf.Approximately (_soundSource2.volume, 0.0f)) {
				_soundSource2.Stop ();
				_soundOff = false;
			} else {
				_soundSource2.volume = MathHelpers.LinMapFrom01 (_soundSource2.volume, 0.0f, _soundOffTimer.PercentTimePassed);
			}
		}
	}

	void PlayLock(LockCellEvent e){
		if (e.Locked) {
			_soundSource1.clip = _lockCell;
			_soundSource1.Play ();
		}
		else {
			_soundSource1.clip = _openCell;
			_soundSource1.Play ();
		}
	}
	void PlayPickUpBomb(PrisonerFoundBombEvent e){
		_soundSource1.clip = _pickUpBomb;
		_soundSource1.Play ();
	}
	void PlayPickUpBomb1(GuardFoundBombEvent e){
		_soundSource1.clip = _pickUpBomb;
		_soundSource1.Play ();
	}
	void GuardStairsStart(GuardStairsStartEvent e){
		_soundSource1.clip = _guardStairs;
		_soundSource1.Play ();
	}
	void PrisonerStairsStartEvent(PrisonerStairsStartEvent e){
		_soundSource1.clip = _prisonerStairs;
		_soundSource1.Play ();
	}

	void PlayLightCaught(LightCaughtEvent e){
		if (!_soundSource2.isPlaying) {
			_soundSource2.Play ();
		}
		_soundSource2.volume = e.Volume;
	}

	void StopCaught(PrisonerHideEvent e){
		if (e.Hidden) {
//			_soundSource2.Stop ();
			_soundOffTimer.Reset();
			_soundOff = true;
		}
	}

	void StopCaught1(LightOffEvent e){
//		_soundSource2.Stop ();
		_soundOffTimer.Reset ();
		_soundOff = true;
	}


	void OnEnable ()
	{
		Events.G.AddListener<GuardEnteringCellEvent>(OnGuardEnterCell);
		Events.G.AddListener<GuardEngaginPrisonerEvent>(OnGuardEngagePrisoner);

		//Act2
		Events.G.AddListener<LockCellEvent>(PlayLock);
		Events.G.AddListener<PrisonerFoundBombEvent>(PlayPickUpBomb);
		Events.G.AddListener<GuardFoundBombEvent>(PlayPickUpBomb1);
		Events.G.AddListener<GuardStairsStartEvent>(GuardStairsStart);
		Events.G.AddListener<PrisonerStairsStartEvent>(PrisonerStairsStartEvent);
		Events.G.AddListener<LightCaughtEvent>(PlayLightCaught);
		Events.G.AddListener<PrisonerHideEvent>(StopCaught);
		Events.G.AddListener<LightOffEvent> (StopCaught1);
	}

	void OnDisable ()
	{
		Events.G.RemoveListener<GuardEnteringCellEvent>(OnGuardEnterCell);
		Events.G.RemoveListener<GuardEngaginPrisonerEvent>(OnGuardEngagePrisoner);

		//Act2
		Events.G.RemoveListener<LockCellEvent>(PlayLock);
		Events.G.RemoveListener<PrisonerFoundBombEvent>(PlayPickUpBomb);
		Events.G.RemoveListener<GuardFoundBombEvent>(PlayPickUpBomb1);
		Events.G.RemoveListener<GuardStairsStartEvent>(GuardStairsStart);
		Events.G.RemoveListener<PrisonerStairsStartEvent>(PrisonerStairsStartEvent);
		Events.G.RemoveListener<LightCaughtEvent>(PlayLightCaught);
		Events.G.RemoveListener<PrisonerHideEvent>(StopCaught);
		Events.G.RemoveListener<LightOffEvent> (StopCaught1);
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
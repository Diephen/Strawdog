using UnityEngine;
using System.Collections;
using Giverspace;
using UnityEngine.SceneManagement;

public class AudioController : MonoBehaviour {
	[SerializeField] float[] volume = new float[] {0.1f, 0.3f, 0.7f};
	[SerializeField] AudioSource _musicSource1;
	[SerializeField] AudioSource _musicSource2;
	[SerializeField] AudioSource _soundSource1;
	[SerializeField] AudioSource _soundSource2_Light;
	[SerializeField] AudioSource _soundSource3;
	[SerializeField] AudioSource _soundSource4;
	AudioSource _tempAudioSource;
	float _goalVolume;
	[SerializeField] AudioClip _lockCell;
	[SerializeField] AudioClip _openCell;
	[SerializeField] AudioClip _pickUpBomb;
	[SerializeField] AudioClip _guardStairs;
	[SerializeField] AudioClip _prisonerStairs;
	[SerializeField] AudioClip _guardWalkOut;

	bool _soundOff = true;
	Timer _soundOffTimer = new Timer(0.5f);

	static AudioController _instance = null;
	int _currentSceneIndex = 0;

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
		_musicSource1.volume = volume [0];
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
			if (Mathf.Approximately (_soundSource2_Light.volume, 0.0f)) {
				_soundSource2_Light.Stop ();
				_soundOff = false;
			} else {
				_soundSource2_Light.volume = MathHelpers.LinMapFrom01 (_soundSource2_Light.volume, 0.0f, _soundOffTimer.PercentTimePassed);
			}
		}

		if (_currentSceneIndex == 0) {
			Debug.Log ("0 Loop");

		}
//		else if (_currentSceneIndex == 20) {
//			if(Input.GetKeyDown(KeyCode.Space)){
//
//			}
//		}
	}

	void PlayLock(LockCellEvent e){
		if (e.Locked) {
			_soundSource1.clip = _lockCell;
			_soundSource1.Play ();
			Log.Metrics.Message("*Lock");
		}
		else {
			_soundSource1.clip = _openCell;
			_soundSource1.Play ();
			Log.Metrics.Message("*[Un]lock");
		}
	}
	void LeftCell(LeftCellUnlockedEvent e){
		_soundSource2_Light.clip = _guardWalkOut;
		_soundSource2_Light.volume = 1.0f;
		_soundSource2_Light.Play ();
		Log.Metrics.Message("CHOICE 2: Unlock");
	}
	void LeaveCell(GuardLeavingCellEvent e){
		_soundSource2_Light.clip = _guardWalkOut;
		_soundSource2_Light.volume = 1.0f;
		_soundSource2_Light.Play ();
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
		Log.Metrics.Message("CHOICE 2: Guard Stairs (Lock)");
	}
	void PrisonerStairsStartEvent(PrisonerStairsStartEvent e){
		_soundSource1.clip = _prisonerStairs;
		_soundSource1.Play ();
		Log.Metrics.Message("CHOICE 3: Prisoner Stairs");
	}

	void PlayLightCaught(LightCaughtEvent e){
		if (!_soundSource2_Light.isPlaying) {
			_soundSource2_Light.Play ();
		}
		_soundSource2_Light.volume = e.Volume;
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
		SceneManager.activeSceneChanged += NewScene;

		Events.G.AddListener<GuardEnteringCellEvent>(OnGuardEnterCell);
		Events.G.AddListener<GuardEngaginPrisonerEvent>(OnGuardEngagePrisoner);
		Events.G.AddListener<LeftCellUnlockedEvent> (LeftCell);
		Events.G.AddListener<GuardLeavingCellEvent> (LeaveCell);

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
		SceneManager.activeSceneChanged -= NewScene;

		Events.G.RemoveListener<GuardEnteringCellEvent>(OnGuardEnterCell);
		Events.G.RemoveListener<GuardEngaginPrisonerEvent>(OnGuardEngagePrisoner);
		Events.G.RemoveListener<LeftCellUnlockedEvent> (LeftCell);
		Events.G.RemoveListener<GuardLeavingCellEvent> (LeaveCell);

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
		_tempAudioSource = _musicSource1;
		_goalVolume = volume [1];
	}

	void OnGuardEngagePrisoner (GuardEngaginPrisonerEvent e)
	{
		if (e.Engaged) 
		{
			_tempAudioSource = _musicSource1;
			_goalVolume = volume [2];
		} 
		else 
		{
			_tempAudioSource = _musicSource1;
			_goalVolume = volume [1];
		}
	}

	void NewScene(Scene previousScene, Scene newScene){
		_currentSceneIndex = SceneManager.GetActiveScene ().buildIndex;

		if (_currentSceneIndex == 0) {
			_soundSource4.clip = Resources.Load<AudioClip>("Sounds/Opening/OpeningSound");
			_soundSource3.clip = Resources.Load<AudioClip>("Sounds/Opening/vinylSound");
			_soundSource3.loop = true;
			_soundSource4.Play ();
			_soundSource3.Play ();
		}
	}

}
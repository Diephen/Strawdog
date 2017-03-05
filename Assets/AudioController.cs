using UnityEngine;
using System.Collections;
using Giverspace;
using UnityEngine.SceneManagement;
using System.Runtime.ConstrainedExecution;

public class AudioController : MonoBehaviour {
	float[] volume = new float[] {1.0f, 0.5f, 0.0f};
	[SerializeField] AudioSource _musicSource1;
	[SerializeField] AudioSource _musicSource2;
	[SerializeField] AudioSource _musicSource3;
	[SerializeField] AudioSource _soundSource1;
	[SerializeField] AudioSource _soundSource2_Light;
	[SerializeField] AudioSource _soundSource3;
	[SerializeField] AudioSource _soundSource4;
	AudioSource _tempAudioSource;
	float _goalVolume = 9999.9f;
	[SerializeField] AudioClip _lockCell;
	[SerializeField] AudioClip _openCell;
	[SerializeField] AudioClip _pickUpBomb;
	[SerializeField] AudioClip _guardStairs;
	[SerializeField] AudioClip _prisonerStairs;
	[SerializeField] AudioClip _guardWalkOut;

	bool _musicOff1 = false;
	bool _musicOff2 = false;
	bool _musicOff3 = false;
	bool _musicOn1 = false;
	bool _musicOn2 = false;
	bool _musicOn3 = false;
	Timer _musicOffTimer = new Timer(4f);
	Timer _musicOnTimer = new Timer(4f);
	[SerializeField] AnimationCurve _musicOffCurve;
	[SerializeField] AnimationCurve _musicOnCurve;

	bool _soundOff1 = false;
	bool _soundOff2 = false;
	bool _soundOff3 = false;
	bool _soundOff4 = false;
	Timer _soundOffTimer = new Timer(1f);

	float _musicVolume = 1.0f;

	static AudioController _instance = null;
	int _currentSceneIndex = 0;

	bool _preventAmbientWindTwice = false;

	AudioClip _loopClip;
	AudioClip _satrioClip;

	AudioClip _windHowl;

	void Awake() {
		if (_instance) {
			Destroy (gameObject);
		}
		else {
			_instance = this;
			DontDestroyOnLoad (gameObject);
			_loopClip = Resources.Load<AudioClip> ("Music/Loop No.1 v.2");
			_satrioClip = Resources.Load<AudioClip> ("Music/Ambient cue 1");
			_windHowl = Resources.Load<AudioClip> ("Sounds/windhowl_ambient");
		}
	}

	void FixedUpdate () {

		if (_tempAudioSource != null &&_goalVolume != 9999.9f && !Mathf.Approximately (_tempAudioSource.volume, _goalVolume)) 
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

		MusicFadeControl ();
		SoundFadeControl ();




		if (_currentSceneIndex == (int)SceneIndex.A1_1_Intro) {
			if (!_musicSource1.isPlaying && !_musicSource2.isPlaying) {
				_musicSource2.loop = true;
				_musicSource2.Play ();
			}
		}
		else if (_currentSceneIndex == (int)SceneIndex.A0_2_TriggerWarning) {
			if(Input.GetKeyDown(KeyCode.Space) && !_soundSource1.isPlaying){
				_soundOffTimer.Reset ();
				_soundOff3 = true;
				_soundSource1.Play ();
			}
		}
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
		_soundSource3.clip = _guardWalkOut;
		_soundSource3.loop = false;
		_soundSource3.volume = 0.8f;
		_soundSource3.Play ();
		Log.Metrics.Message("CHOICE 2: Unlock");
	}
	void LeaveCell(GuardLeavingCellEvent e){
		_soundSource3.clip = _guardWalkOut;
		_soundSource3.volume = 1.0f;
		_soundSource3.Play ();
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

	void LoadTutorialEnd(LoadTitleCardEvent e){
		if (!_soundSource3.isPlaying) {
			if (e.TitleIndex == 0) {
				_soundSource3.volume = 1.0f;
				_soundSource3.loop = false;
				_soundSource3.clip = Resources.Load<AudioClip> ("Sounds/PianoPluck");
				_soundSource3.Play ();
			}
		}
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
			_soundOff2 = true;
		}
	}

	void StopCaught1(LightOffEvent e){
//		_soundSource2.Stop ();
		_soundOffTimer.Reset ();
		_soundOff2 = true;
	}

	void LoadTransitionAct1(Act0EndedEvent e){
		_musicSource3.volume = 1.0f;
		_musicSource3.Play ();
		_musicOffTimer.Reset ();
		_musicOff1 = true;
		_musicOff2 = true;
	}

	void PlayAmbientWind(TriggerAmbientWindEvent e){
		if (e.Triggered != _preventAmbientWindTwice) {
			_preventAmbientWindTwice = e.Triggered;

			if (e.Triggered) {
				_musicSource1.clip = _windHowl;
				_musicOn2 = false;
				_musicOff2 = true;
				_musicSource1.volume = 0.0f;
				_musicSource1.loop = true;
				_musicOff1 = false;
				_musicOn1 = true;
			}
			else {
				_musicSource2.clip = Resources.Load<AudioClip> ("Music/Loop 2");
				_musicOn1 = false;
				_musicOff1 = true;
				_musicSource2.volume = 0.0f;
				_musicSource2.loop = true;
				_musicOff2 = false;
				_musicOn2 = true;
			}
		_musicOff3 = true;
		_musicOnTimer.Reset ();
		_musicOffTimer.Reset ();
		}
	}

	void OnEnable ()
	{
		SceneManager.activeSceneChanged += NewScene;
		Events.G.AddListener<LoadTitleCardEvent>(LoadTutorialEnd);

		Events.G.AddListener<Act0EndedEvent>(LoadTransitionAct1);

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

		//Act3
		Events.G.AddListener<InterrogationQuestioningEvent> (InterroSound);
		Events.G.AddListener<TriggerAmbientWindEvent> (PlayAmbientWind);
	}

	void OnDisable ()
	{
		SceneManager.activeSceneChanged -= NewScene;
		Events.G.RemoveListener<LoadTitleCardEvent>(LoadTutorialEnd);

		Events.G.RemoveListener<Act0EndedEvent>(LoadTransitionAct1);

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

		//Act3
		Events.G.RemoveListener<InterrogationQuestioningEvent> (InterroSound);
		Events.G.RemoveListener<TriggerAmbientWindEvent> (PlayAmbientWind);
	}

	void InterroSound (InterrogationQuestioningEvent e){
		if (e.Engaged) 
		{
			_goalVolume = volume [2];
		} 
		else 
		{
			_goalVolume = volume [0];
		}
	}

	void OnGuardEnterCell (GuardEnteringCellEvent e)
	{
		_goalVolume = volume [1];
	}

	void OnGuardEngagePrisoner (GuardEngaginPrisonerEvent e)
	{
		if (e.Engaged) 
		{
			_goalVolume = volume [2];
		} 
		else 
		{
			_goalVolume = volume [1];
		}
	}

	void NewScene(Scene previousScene, Scene newScene){
		_currentSceneIndex = newScene.buildIndex;
		if (_currentSceneIndex == (int)SceneIndex.A0_1_Logo) {
			_musicSource1.Stop ();
			_musicSource2.Stop ();
			_musicSource3.Stop ();
			_soundSource1.Stop ();
			_soundSource2_Light.Stop ();
			_soundSource3.Stop ();
			_soundSource4.Stop ();

			_soundSource4.clip = Resources.Load<AudioClip> ("Sounds/Opening/OpeningSteps");
			_soundSource3.clip = Resources.Load<AudioClip> ("Sounds/Opening/vinylSound");
			_soundSource3.loop = true;
			_soundSource3.volume = 1.0f;
			_soundSource4.volume = 1.0f;
			_soundSource4.Play ();
			_soundSource3.Play ();
		}
		else if (_currentSceneIndex == (int)SceneIndex.A0_2_TriggerWarning) {
			_soundSource1.clip = Resources.Load<AudioClip> ("Sounds/Opening/PickUpNeedle");
			_soundSource1.loop = false;
		}
		else if (_currentSceneIndex == (int)SceneIndex.A0_3_Tutorial) {
			_musicSource2.volume = 0.6f;
			_musicSource2.clip = _windHowl;
			_musicSource2.loop = true;
			_musicSource2.Play ();
		}
		else if (_currentSceneIndex == (int)SceneIndex.A1_1_Intro) {
			_soundSource2_Light.Stop ();
			_musicSource1.clip = Resources.Load<AudioClip> ("Music/Theme 1 v.2");
			_musicSource2.clip = _loopClip;
			_musicSource3.clip = Resources.Load<AudioClip> ("Music/Transition1");
			_musicSource1.loop = false;
			_musicVolume = 1.0f;
			_musicSource2.volume = 1.0f;
			_musicSource1.Play ();
		}
		else if (_currentSceneIndex == (int)SceneIndex.A1_1_Intro) {
			_musicSource3.clip = Resources.Load<AudioClip> ("Music/Transition1");
			_musicOnTimer.Reset ();
			_musicOffTimer.Reset ();
			_musicVolume = 0.5f;
			if (!_musicSource1.isPlaying) {
				_musicSource1.clip = _satrioClip;
				_musicSource1.volume = 0.0f;
				_musicSource1.loop = true;
				_musicOn1 = true;
				_tempAudioSource = _musicSource1;
			}
			else if (!_musicSource2.isPlaying) {
				_musicSource2.clip = _satrioClip;
				_musicSource2.volume = 0.0f;
				_musicSource2.loop = true;
				_musicOn2 = true;
				_tempAudioSource = _musicSource2;
			}
			_musicOff3 = true;
		}
//		else if (_currentSceneIndex == 3) {
//			//Reinitializing volume change variables
//			_musicOnTimer.Reset ();
//			_musicOffTimer.Reset ();
//			_tempAudioSource = null;
//			_goalVolume = 9999.9f;
//			_soundSource1.Stop ();
//			_soundSource2_Light.Stop ();
//			_soundSource3.Stop ();
//			_soundSource4.Stop ();
//
//			_musicOff2 = true;
//			_musicOff1 = true;
//			_musicSource3.clip = Resources.Load<AudioClip> ("Music/Chimes");
//			_musicOn3 = true;
//		}
		else if (_currentSceneIndex == 4) {
			_musicOnTimer.Reset ();
			_musicSource2.clip = _loopClip;
			_musicSource2.loop = true;
			_musicOn2 = true;
		}
		else if (_currentSceneIndex == 6) {
			_soundSource2_Light.clip = Resources.Load<AudioClip> ("Sounds/Light-Ring-1");
			_soundSource2_Light.loop = true;
		}
		else if (_currentSceneIndex == 9) {
			_musicOnTimer.Reset ();
			_musicOffTimer.Reset ();
			_soundOff1 = true;
			_soundOff3 = true;
			_soundOff4 = true;
			_musicOff2 = true;
			_musicSource1.clip = _satrioClip;
			_musicSource1.volume = 1.0f;
			_musicSource1.loop = true;
			_musicOn1 = true;
			_tempAudioSource = _musicSource1;
			// _soundSource2_Light.clip = Resources.Load<AudioClip> ("Sounds/ClockTick");
			// _soundSource2_Light.volume = 1.0f;
			// _soundSource2_Light.loop = true;
			// _soundSource2_Light.Play ();
		}
		else if (_currentSceneIndex == 10) {
			_musicOnTimer.Reset ();
			_musicOffTimer.Reset ();
			_soundOff1 = true;
			_soundOff3 = true;
			_soundOff4 = true;
			_musicOff2 = true;
			_musicSource1.clip = _satrioClip;
			_musicSource1.volume = 1.0f;
			_musicSource1.loop = true;
			_musicOn1 = true;
			_tempAudioSource = _musicSource1;
			_soundSource2_Light.clip = Resources.Load<AudioClip> ("Sounds/ClockTick");
			_soundSource2_Light.volume = 1.0f;
			_soundSource2_Light.loop = true;
			_soundSource2_Light.Play ();
		}
		else if (_currentSceneIndex == 15) {
			_musicOnTimer.Reset ();
			_musicOffTimer.Reset ();
			_soundOff2 = true;
			_musicOff1 = true;
			_musicSource2.clip = _loopClip;
			_musicSource2.loop = true;
			_musicOn2 = true;
		}
		else if (_currentSceneIndex == 16) {
			_musicOnTimer.Reset ();
			_musicOffTimer.Reset ();
			_soundOff2 = true;
			_musicOff1 = true;
			_musicSource2.clip = _loopClip;
			_musicSource2.loop = true;
			_musicOn2 = true;
		}
		else if (_currentSceneIndex == 18) {
			_musicOnTimer.Reset ();
			_musicOffTimer.Reset ();
			_soundOff2 = true;
			_musicOff1 = true;
			_musicSource2.clip = _loopClip;
			_musicSource2.loop = true;
			_musicOn2 = true;
		}
		else if (_currentSceneIndex == 24) {
			_musicOffTimer.Reset ();
			_soundOff1 = true;
			_soundOff2 = true;
			_soundOff3 = true;
			_soundOff4 = true;
			_musicOff1 = true;
			_musicOff2 = true;
			_musicOff3 = true;
		}
	}


	void MusicFadeControl(){
		if (_musicOff1) {
			if (Mathf.Approximately (_musicSource1.volume, 0.0f)) {
				_musicSource1.Stop ();
				_musicOff1 = false;
			}
			_musicSource1.volume = MathHelpers.LinMapFrom01(0.0f, _musicSource1.volume, _musicOffCurve.Evaluate (_musicOffTimer.PercentTimePassed));
		}
		else if (_musicOn1) {
			_musicSource1.volume = Mathf.Clamp (_musicOnCurve.Evaluate (_musicOnTimer.PercentTimePassed), 0.0f, _musicVolume);

			if (!_musicSource1.isPlaying) {
				_musicSource1.Play ();
			}
			else if (_musicSource1.volume > _musicVolume-0.02f) {
				_musicOn1 = false;
			}
		}
		if (_musicOff2) {
			if (Mathf.Approximately (_musicSource2.volume, 0.0f)) {
				_musicSource2.Stop ();
				_musicOff2 = false;
			} 
			_musicSource2.volume = MathHelpers.LinMapFrom01(0.0f, _musicSource2.volume, _musicOffCurve.Evaluate (_musicOffTimer.PercentTimePassed));
		}
		else if (_musicOn2) {
			
			_musicSource2.volume = MathHelpers.LinMapFrom01(0.0f, _musicVolume, _musicOnCurve.Evaluate (_musicOnTimer.PercentTimePassed));

			if (!_musicSource2.isPlaying) {
				_musicSource2.Play ();
			}
			else if (_musicSource2.volume > _musicVolume-0.02f) {
				_musicOn2 = false;
			}
		}
		if (_musicOff3) {
			if (Mathf.Approximately (_musicSource3.volume, 0.0f)) {
				_musicSource3.Stop ();
				_musicOff3 = false;
			}
			_musicSource3.volume =  MathHelpers.LinMapFrom01(0.0f, _musicSource3.volume, _musicOffCurve.Evaluate (_musicOffTimer.PercentTimePassed));
		}
		else if (_musicOn3) {
			_musicSource3.volume = Mathf.Clamp (_musicOnCurve.Evaluate (_musicOnTimer.PercentTimePassed), 0.0f, _musicVolume);

			if (!_musicSource3.isPlaying) {
				_musicSource3.Play ();
			}
			else if (_musicSource3.volume > _musicVolume-0.02f) {
				_musicOn3 = false;
			}
		}
	}

	void SoundFadeControl(){
		if (_soundOff1) {
			if (Mathf.Approximately (_soundSource1.volume, 0.0f)) {
				_soundSource1.Stop ();
				_soundOff1 = false;
			}
			_soundSource1.volume = MathHelpers.LinMapFrom01 (_soundSource1.volume, 0.0f, _soundOffTimer.PercentTimePassed);
		}
		if (_soundOff2) {
			if (Mathf.Approximately (_soundSource2_Light.volume, 0.0f)) {
				_soundSource2_Light.Stop ();
				_soundOff2 = false;
			}
			_soundSource2_Light.volume = MathHelpers.LinMapFrom01 (_soundSource2_Light.volume, 0.0f, _soundOffTimer.PercentTimePassed);
		}
		if (_soundOff3) {
			if (Mathf.Approximately (_soundSource3.volume, 0.0f)) {
				_soundSource3.Stop ();
				_soundOff3 = false;
			}
			_soundSource3.volume = MathHelpers.LinMapFrom01 (_soundSource3.volume, 0.0f, _soundOffTimer.PercentTimePassed);
		}
		if (_soundOff4) {
			if (Mathf.Approximately (_soundSource4.volume, 0.0f)) {
				_soundSource4.Stop ();
				_soundOff4 = false;
			}
			_soundSource4.volume = MathHelpers.LinMapFrom01 (_soundSource4.volume, 0.0f, _soundOffTimer.PercentTimePassed);
		}
	}

	public void LoopSound(AudioClip _loopClip){
		_soundSource2_Light.clip = _loopClip;
		_soundSource2_Light.volume = 1.0f;
		_soundSource2_Light.loop = true;
		_soundSource2_Light.Play ();
	}

	public void SingleSound(AudioClip _singleClip){
		_soundSource4.clip = _singleClip;
		_soundSource4.volume = 1.0f;
		_soundSource4.Play ();
	}
}
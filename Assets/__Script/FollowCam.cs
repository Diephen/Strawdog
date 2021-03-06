﻿using UnityEngine;
using System.Collections;

public class FollowCam : MonoBehaviour {
	enum cameraPos {Left, Center, Right, Static};

	[SerializeField] bool _startGuard = true;
	[SerializeField] Transform _followGuard;
	[SerializeField] Transform _followPrisoner;
	Transform _followObj;
	Vector3 CameraPos;
	[SerializeField] bool[] XYZ = new bool[3] {false, false, false};

	Vector3 _guardPosDiff;
	Vector3 _prisonerPosDiff;

	[SerializeField] cameraPos _cameraToggle = cameraPos.Right;

	float timer = 0f;

	[SerializeField] float _leftCameraPos = 2f;
	[SerializeField] float _rightCameraPos = -2f;

	[SerializeField] int ActNumber = 1;
	enum followWho {Guard, Prisoner, None};
	followWho _followWho = followWho.None;

	cameraPos _prevTansition;

	void Start () {
		CameraPos = gameObject.transform.position;
		if (_startGuard) {
			_followObj = _followGuard;
			_followWho = followWho.Guard;
		} else {
			_followObj = _followPrisoner;
			_followWho = followWho.Prisoner;
		}
		_guardPosDiff = transform.position - _followGuard.position;
		_prisonerPosDiff = transform.position - _followPrisoner.position;

	}
	
	// Update is called once per frame
	void Update () {

//		if (Input.GetKeyDown (KeyCode.T)) {
//			timer = 0f;
//			cameraToggle = !cameraToggle;
//		}
			

		if (XYZ [0]) {
			if (_cameraToggle == cameraPos.Left) {
				timer += Time.deltaTime/3f;
				CameraPos.x = Mathf.Lerp (CameraPos.x, _followObj.position.x + _leftCameraPos, timer);
			} else if (_cameraToggle == cameraPos.Right) {
				timer += Time.deltaTime;
				CameraPos.x = Mathf.Lerp (CameraPos.x, _followObj.position.x + _rightCameraPos, timer);
			} else if(_cameraToggle == cameraPos.Center){
				timer += Time.deltaTime;
				CameraPos.x = Mathf.Lerp (CameraPos.x, _followObj.position.x, timer);
			}

//			CameraPos.x = _followObj.position.x + _rightCameraPos;
			//			CameraPos.x = _followObj.position.x + _posDiff.x;
		} 
		if (XYZ [1]) {
			if (_followWho == followWho.Guard) {
				CameraPos.y = _followObj.position.y + _guardPosDiff.y;
			} else if (_followWho == followWho.Prisoner) {
				CameraPos.y = _followObj.position.y + _prisonerPosDiff.y;
			} else {
				CameraPos.y = _followObj.position.y;
			}

		}
		if (XYZ [2]) {
//			CameraPos.z = _followObj.position.z + _posDiff.z;
		}
		gameObject.transform.position = CameraPos;
	}


	void OnEnable ()
	{
		Events.G.AddListener<StaticCamera>(StaticCam);

		Events.G.AddListener<GuardEngaginPrisonerEvent>(OnGuardEngagePrisoner);
		Events.G.AddListener<LeftCellUnlockedEvent>(LeftCellUnlocked);
		Events.G.AddListener<Guard_EncounterEvent>(GuardEncounter);
		Events.G.AddListener<Prisoner_EncounterEvent>(PrisonerEncounter);
		Events.G.AddListener<BrokeFree>(BreakFree);
		Events.G.AddListener<LockCellEvent>(CloseDoor);
		Events.G.AddListener<TransitionSecretDoorEvent>(TransitionSecret);
		Events.G.AddListener<ShootSwitchEvent>(ShootSwitch);
		Events.G.AddListener<PrisonerShotEvent>(PrisonerShot);
		Events.G.AddListener<Act2_SoldierAppear>(PrisonerHitFence);
		Events.G.AddListener<PrisonerLeftFenceEvent>(PrisonerLeftFence);

		Events.G.AddListener<CameraFollowTransformEvent>(FollowTransform);
		Events.G.AddListener<DogReturnToPrisonerEvent>(DogReturnToPrisoner);
		Events.G.AddListener<DogReturnToGuardEvent>(DogReturnToGuard);
	}

	void OnDisable ()
	{
		Events.G.RemoveListener<StaticCamera>(StaticCam);

		Events.G.RemoveListener<GuardEngaginPrisonerEvent>(OnGuardEngagePrisoner);
		Events.G.RemoveListener<LeftCellUnlockedEvent>(LeftCellUnlocked);
		Events.G.RemoveListener<Guard_EncounterEvent>(GuardEncounter);
		Events.G.RemoveListener<Prisoner_EncounterEvent>(PrisonerEncounter);
		Events.G.RemoveListener<BrokeFree>(BreakFree);
		Events.G.RemoveListener<LockCellEvent>(CloseDoor);
		Events.G.RemoveListener<TransitionSecretDoorEvent>(TransitionSecret);
		Events.G.RemoveListener<ShootSwitchEvent>(ShootSwitch);
		Events.G.RemoveListener<PrisonerShotEvent>(PrisonerShot);
		Events.G.RemoveListener<Act2_SoldierAppear>(PrisonerHitFence);
		Events.G.RemoveListener<PrisonerLeftFenceEvent>(PrisonerLeftFence);

		Events.G.RemoveListener<CameraFollowTransformEvent>(FollowTransform);
		Events.G.RemoveListener<DogReturnToPrisonerEvent>(DogReturnToPrisoner);
		Events.G.RemoveListener<DogReturnToGuardEvent>(DogReturnToGuard);
	}

	void StaticCam(StaticCamera e){
		_cameraToggle = cameraPos.Static;
	}

	void FollowTransform(CameraFollowTransformEvent e){
		if(_followObj != e._Transform.transform){
			timer = 0f;
			_followObj = e._Transform.transform;
			_cameraToggle = cameraPos.Center;
		}
	}

	void DogReturnToPrisoner(DogReturnToPrisonerEvent e){
		timer = 0f;
		_followObj = _followPrisoner;
		_cameraToggle = cameraPos.Left;
	}

	void DogReturnToGuard(DogReturnToGuardEvent e){
		timer = 0f;
		_followObj = _followGuard;
		_cameraToggle = cameraPos.Left;
	}

	void TransitionSecret(TransitionSecretDoorEvent e){
		if (e.SecretOn) {
			timer = 0f;
			_prevTansition = _cameraToggle;
			_cameraToggle = cameraPos.Center;
		}
		else {
			timer = 0f;
			_cameraToggle = _prevTansition;
		}
	}
		

	void OnGuardEngagePrisoner (GuardEngaginPrisonerEvent e)
	{
		if (e.Engaged) {
			timer = 0f;
			_followObj = _followPrisoner;
			_followWho = followWho.Prisoner;
			_cameraToggle = cameraPos.Left;
		}
	}

	void LeftCellUnlocked(LeftCellUnlockedEvent e){
		timer = 0f;
		_followObj = _followPrisoner;
		_followWho = followWho.Prisoner;
		_cameraToggle = cameraPos.Left;
	}

	void GuardEncounter(Guard_EncounterEvent e) {
		timer = 0f;
		_followObj = _followPrisoner;
		_cameraToggle = cameraPos.Left;
	}

	void BreakFree(BrokeFree e) {
		timer = 0f;
		_cameraToggle = cameraPos.Center;
	}

	void PrisonerEncounter(Prisoner_EncounterEvent e) {
		timer = 0f;
		_followObj = _followGuard;
		_cameraToggle = cameraPos.Right;
	}

	void CloseDoor(LockCellEvent e){
		if (!e.Locked) {
			timer = 0f;
			_followObj = _followPrisoner;
			_cameraToggle = cameraPos.Left;
		} else {
			timer = 0f;
			_followObj = _followGuard;
			_cameraToggle = cameraPos.Center;
		}
	}

	void ShootSwitch(ShootSwitchEvent e){
		timer = 0f;
		_followObj = _followGuard;
		_cameraToggle = cameraPos.Right;
	}

	void PrisonerShot(PrisonerShotEvent e){
		timer = 0f;
		_followObj = _followGuard;
		_cameraToggle = cameraPos.Right;
	}

	void PrisonerHitFence(Act2_SoldierAppear e){
		timer = 0f;
		_followObj = _followPrisoner;
		_cameraToggle = cameraPos.Left;
	}

	void PrisonerLeftFence(PrisonerLeftFenceEvent e){
		timer = 0f;
		_followObj = _followPrisoner;
		_cameraToggle = cameraPos.Right;
	}
}

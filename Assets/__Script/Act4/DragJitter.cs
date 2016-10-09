﻿using UnityEngine;
using System.Collections;

public class DragJitter : MonoBehaviour {
	[SerializeField] AnimationCurve _joltCurve;
	[SerializeField] float _pushOffset = 4f;

	PuppetControl _prisonerPuppetControl;
	KeyCode[] _prisonerKeyCodes;
	Timer _pushTimer;

	[SerializeField] bool _isThePrisoner;
	int _inputCount = 0;
	int _freedomCount = 20;

	// Use this for initialization
	void Start () {
		_pushTimer = new Timer (3.0f);
		_pushTimer.CooldownTime = _pushOffset;

		if (_isThePrisoner) {
			_prisonerPuppetControl = gameObject.GetComponent <PuppetControl> ();
			_prisonerKeyCodes = _prisonerPuppetControl.GetKeyCodes ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		gameObject.transform.Translate (Vector2.left * Time.deltaTime * _joltCurve.Evaluate (_pushTimer.PercentTimePassed) * 5.0f);
		if (_pushTimer.IsOffCooldown) {
			_pushTimer.Reset ();
		}

		if (_isThePrisoner) {
			if (Input.GetKeyDown (_prisonerKeyCodes [0]) ||
			   Input.GetKeyDown (_prisonerKeyCodes [1]) ||
			   Input.GetKeyDown (_prisonerKeyCodes [2]) ||
			   Input.GetKeyDown (_prisonerKeyCodes [3])) {
				_inputCount++;
			}
		}

		if (_inputCount > _freedomCount) {
			_prisonerPuppetControl.EnableContinuousWalk ();
			Events.G.Raise (new BrokeFree());
			this.enabled = false;
		}
	}
}
using UnityEngine;
using System.Collections;
using System.IO;

public class patrol : MonoBehaviour {
	[SerializeField] MinMax _patrolArea = new MinMax (-1.0f, 1.0f);
	float _startPosition;
	[SerializeField] float _speed = 2.0f;
	bool _isLeft = true;
	bool _wait = false;
	bool _turn = true;
	Timer _waitTimer;
	Timer _turnTimer;
	[SerializeField] guardPatrol _guardPatrol;
	[SerializeField] int _id;

	bool _isSleeping = false;

	[SerializeField] Color _sleepColor;
	[SerializeField] Color _hoverColor;
	int _hoverState = 0; // 0: not hover, 1: hover, 2: awake
	Timer _hoverColorTimer;
	SpriteRenderer _patrolGuardSprite;

	void Start () {
		_startPosition = transform.position.x;
		_waitTimer = new Timer (4.0f);
		_turnTimer = new Timer (2.0f);
		_hoverColorTimer = new Timer (1f);

		_patrolGuardSprite = GetComponent<SpriteRenderer> ();

		// Set all the guard asleep for when Patrol
		if (GameStateManager.gameStateManager._currScene == SceneIndex.A2_3_Patrol) {
			_isSleeping = true;
			_guardPatrol.gameObject.SetActive (false);
			_patrolGuardSprite.color = _sleepColor;
		}
	}
	
	void Update () {
		if (!_wait && !_isSleeping) {
			if (_isLeft) {
				transform.Translate (Vector2.left * Time.deltaTime * _speed);
				if (transform.position.x < _startPosition + _patrolArea.Min) {
					_isLeft = false;
					_waitTimer.Reset ();
					_turnTimer.Reset ();
					_wait = true;
				}
			}
			else {
				transform.Translate (Vector2.right * Time.deltaTime * _speed);
				if (transform.position.x > _startPosition + _patrolArea.Max) {
					_isLeft = true;
					_waitTimer.Reset ();
					_turnTimer.Reset ();
					_wait = true;
				}
			}
		}
		else {
			if (_waitTimer.IsOffCooldown) {
				_wait = false;
				_turn = true;

			} else if (_turnTimer.IsOffCooldown && _turn) {
				_guardPatrol.Turn (!_isLeft);
				_turn = false;
			}
		}
	}

	void Wait(LightCaughtEvent e){
		if (e.Id == _id) {
			_waitTimer.Reset ();
			_wait = true;
		}
	}

	public void WakeUp() {
		//Call the animation and set _isSleeping to false
		_isSleeping = false;
		_guardPatrol.gameObject.SetActive (true);
		_hoverColorTimer.Reset ();
		_hoverState = 2;
		StartCoroutine (GlowColor());
	}

	public void HoverColor(bool isHovering){
		if (_hoverState != 2) {
			_hoverColorTimer.Reset ();
			if (isHovering) {
				_hoverState = 1;
			} else {
				_hoverState = 0;
			}
			StartCoroutine (GlowColor());
		}
	}

	IEnumerator GlowColor(){
		while (!_hoverColorTimer.IsOffCooldown) {
			if (_hoverState == 0) {
				_patrolGuardSprite.color = Color.Lerp (_patrolGuardSprite.color, _sleepColor, _hoverColorTimer.PercentTimePassed);
			} else if (_hoverState == 1) {
				_patrolGuardSprite.color = Color.Lerp (_patrolGuardSprite.color, _hoverColor, _hoverColorTimer.PercentTimePassed);
			} else {
				_patrolGuardSprite.color = Color.Lerp (_patrolGuardSprite.color, Color.white, _hoverColorTimer.PercentTimePassed);
			}
			yield return null;
		}

	}

	void OnEnable(){
		Events.G.AddListener<LightCaughtEvent>(Wait);
	}
	void OnDisable(){
		Events.G.RemoveListener<LightCaughtEvent>(Wait);
	}
}

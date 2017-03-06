using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CellAnimation: StateMachineBehaviour{

}

public class CellDoorResponse : MonoBehaviour {

	AudioSource _audioSource;
	KeyCode _gUp;
	KeyCode _gDown;
	KeyCode _pUp;
	KeyCode _pDown;
	Color _originColor;
	Color _glowColor = Color.white;
	SpriteRenderer _spriteRenderer;
	bool _doorAreaEntered = false;
	Timer _doorGlowTimer;
	Animator _anim;

	// Use this for initialization
	void Start () {
		_doorGlowTimer = new Timer(1.0f);
		_spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		_originColor = _spriteRenderer.color;
		_audioSource = gameObject.GetComponent<AudioSource>();
		_gUp = GameStateManager.gameStateManager._gUpKey;
		_gDown = GameStateManager.gameStateManager._gDownKey;
		_pUp = GameStateManager.gameStateManager._pUpKey;
		_pDown = GameStateManager.gameStateManager._pDownKey;
		_anim = gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if(_doorAreaEntered){
			if(Input.GetKeyDown(_gUp)||Input.GetKeyDown(_gDown)||Input.GetKeyDown(_pUp)||Input.GetKeyDown(_pDown)){
				if(!_audioSource.isPlaying){
					_audioSource.Play();
					_anim.SetBool("Response", true);
				}
			}
			_spriteRenderer.color = Color.Lerp(_glowColor, _originColor, _doorGlowTimer.PercentTimeLeft);
		} else {
			_spriteRenderer.color = Color.Lerp(_originColor, _glowColor, _doorGlowTimer.PercentTimeLeft);
		}
	}

	void FixedUpdate(){
		if(!_audioSource.isPlaying){
			_anim.SetBool("Response", false);
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		if(other.tag == "Guard"|other.tag == "Prisoner"){
			_doorAreaEntered = true;
			_doorGlowTimer.Reset();
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if(other.tag == "Guard"|other.tag == "Prisoner"){
			_doorAreaEntered = false;
			_doorGlowTimer.Reset();
		}
	}
}

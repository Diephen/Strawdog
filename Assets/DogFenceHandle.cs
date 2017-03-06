using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DogFenceHandle : MonoBehaviour {
	enum DogState{
		wait,
		idle,
		start,
		beg,
		touched,
		leave,
		follow,
		fence
	}

DogState m_DogState = DogState.wait;
Transform _playerCharacter;
[SerializeField] Transform _dogStone;
[SerializeField] Transform _soldierAppearPos;
Vector3 _tempFlipDog;
Vector3 _tempDogPos;
bool _calledOnce = false;
bool _fenceDogCalledOnce = false;
Vector3 _dogOriginPos;

	void Start(){
		_tempFlipDog = transform.localScale;
		_tempDogPos = transform.position;
		_dogOriginPos = transform.position;
	}

	void FixedUpdate () {
		if(m_DogState == DogState.follow){
			if(Mathf.Abs(_playerCharacter.position.x - transform.position.x) < 2.0f){
				m_DogState = DogState.idle;
			}
			else if(_playerCharacter.position.x > transform.position.x){
         		_tempDogPos.x = Mathf.MoveTowards(transform.position.x, _playerCharacter.position.x, Time.deltaTime * 2.7f);
				transform.position = _tempDogPos;
				// transform.Translate(Vector2.right * Time.deltaTime * 2.5f);
				if(transform.localScale.x > 0.0f){
					_tempFlipDog = transform.localScale;
					_tempFlipDog.x = _tempFlipDog.x * -1.0f;
					transform.localScale = _tempFlipDog;
				}
			} else if(transform.position.x > _dogOriginPos.x){
				_tempDogPos.x = Mathf.MoveTowards(transform.position.x, _playerCharacter.position.x, Time.deltaTime * 2.0f);
				transform.position = _tempDogPos;
				if(transform.localScale.x < 0.0f){
					_tempFlipDog = transform.localScale;
					_tempFlipDog.x = _tempFlipDog.x * -1.0f;
					transform.localScale = _tempFlipDog;
				}
			}
		} else if (m_DogState == DogState.fence){
			if(_dogStone.position.x > transform.position.x){
				transform.Translate(Vector2.right * Time.deltaTime * 4.3f);
			} else {
				if(_calledOnce == false){
					_calledOnce = true;
					StartCoroutine(WaitForBark());
				}
			}
		}
		if(_soldierAppearPos.position.x < transform.position.x){
			m_DogState = DogState.fence;
			if(_fenceDogCalledOnce == false){
				_fenceDogCalledOnce = true;
				Events.G.Raise(new CameraFollowTransformEvent(gameObject));
				Events.G.Raise(new DisableMoveEvent(CharacterIdentity.Both));
			}
		}
		Debug.Log (m_DogState);
	}

	IEnumerator WaitForBark(){
		yield return new WaitForSeconds(2);
		if(SceneManager.GetActiveScene().buildIndex == 11){
			Events.G.Raise(new DogReturnToPrisonerEvent());
			Events.G.Raise(new EnableMoveEvent(CharacterIdentity.Both));
		} else if (SceneManager.GetActiveScene().buildIndex == 12){
			Events.G.Raise(new DogReturnToGuardEvent());
			Events.G.Raise(new EnableMoveEvent(CharacterIdentity.Both));
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		//Debug.Log ("Attention");
		if(other.name == "DogFenceTrigger"){
			m_DogState = DogState.fence;
			other.enabled = false;

		}
		if((other.tag == "Guard" || other.tag == "Prisoner")){
			_playerCharacter = other.transform;
			m_DogState = DogState.idle;
			// StopPlayer ();
			// StartCoroutine (m_ProgressBar.FadeIn(3f));
			// m_AnimInjection.SetEngage ();
			// m_DogState = DogState.start;
			// CheckState (m_DogState);
			// Debug.Log ("wiggle wiggle");
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		//Debug.Log ("Attention");
		if(other.tag == "Guard"|other.tag == "Prisoner"){
			// LeavePlayer ();
			if(m_DogState != DogState.fence){
				m_DogState = DogState.follow;
			}
			// CheckState (m_DogState);
			// //Debug.Log ("bye bye");
		}

	}



}

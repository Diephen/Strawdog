using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogFenceHandle : MonoBehaviour {
	enum DogState{
		idle,
		start,
		beg,
		touched,
		leave,
		follow,
		fence
	}

DogState m_DogState = DogState.idle;
Transform _playerCharacter;
[SerializeField] Transform _dogStone;
[SerializeField] Transform _soldierAppearPos;

	void Update () {
		if(m_DogState == DogState.follow){
			if(_playerCharacter.position.x > transform.position.x){
				transform.Translate(Vector2.right * Time.deltaTime * 2.3f);
			} else {
				transform.Translate(Vector2.left * Time.deltaTime * 2.0f);
			}
		} else if (m_DogState == DogState.fence){
			if(_dogStone.position.x > transform.position.x){
				transform.Translate(Vector2.right * Time.deltaTime * 4.0f);
			}
		}
		if(_soldierAppearPos.position.x < transform.position.x){
			m_DogState = DogState.fence;
		}
		Debug.Log (m_DogState);
	}

	void OnTriggerEnter2D(Collider2D other) {
		//Debug.Log ("Attention");
		if(other.name == "DogFenceTrigger"){
			m_DogState = DogState.fence;
			other.enabled = false;
			Debug.Log ("wiggle wiggle");
		}
		if(m_DogState == DogState.idle && (other.tag == "Guard" || other.tag == "Prisoner")){
			_playerCharacter = other.transform;
			m_DogState = DogState.follow;
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
			// m_DogState = DogState.idle;
			// CheckState (m_DogState);
			// //Debug.Log ("bye bye");
		}

	}



}

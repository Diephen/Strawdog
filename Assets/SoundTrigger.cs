using UnityEngine;
using System.Collections;

public class SoundTrigger : MonoBehaviour {
	AudioSource _audioSource;
	// Use this for initialization
	void Start () {
		_audioSource = gameObject.GetComponent<AudioSource> ();
	}
	
	void OnTriggerEnter2D(Collider2D other){
		if(other.tag == "Guard" || other.tag == "Prisoner"){
			if(!_audioSource.isPlaying){
				_audioSource.Play();
			}
		}
	}
}

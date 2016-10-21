using UnityEngine;
using System.Collections;

public class Fence : MonoBehaviour {
	AudioSource _audioSource;

	// Use this for initialization
	void Start () {
		_audioSource = gameObject.GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Guard" || other.tag == "Prisoner") {
			_audioSource.Play ();
		}
	}
}

using UnityEngine;
using System.Collections;

public class Announcement : MonoBehaviour {
	AudioSource _audioSource;
	BoxCollider2D _box2d;
	// Use this for initialization
	void Start () {
		_audioSource = gameObject.GetComponent<AudioSource> ();
		_box2d = gameObject.GetComponent<BoxCollider2D> ();
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Guard") {
			_audioSource.Play ();
			_box2d.enabled = false;
		}
	}
}

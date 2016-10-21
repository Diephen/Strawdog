using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour {
	float _floorPos =  -3.3f;
	bool _floorStopped = false;
	bool _thrownOnce = false;
	Rigidbody2D _bombRigidBody;
	AudioSource _audioSource;
	// Use this for initialization
	void Start () {
		_bombRigidBody = gameObject.GetComponent<Rigidbody2D> ();
		_audioSource = gameObject.GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.y < _floorPos && !_floorStopped) {
			_bombRigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
			_floorStopped = true;
			if (_thrownOnce) {
				_audioSource.Play ();
			}
		}
		else if (_floorStopped && transform.position.y > _floorPos) {
			_floorStopped = false;
		}
	}

	public void ThrowBomb(){
		if (!_thrownOnce) {
			_bombRigidBody.constraints = RigidbodyConstraints2D.None;
			_bombRigidBody.AddForce ((Vector2.up * 2.2f + Vector2.left * 1.2f) * 300.0f);
			_thrownOnce = true;
		}
	}
}

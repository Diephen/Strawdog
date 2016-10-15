using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour {
	float _floorPos =  -3.3f;
	bool _floorStopped = false;
	bool _thrownOnce = false;
	Rigidbody2D _bombRigidBody;
	// Use this for initialization
	void Start () {
		_bombRigidBody = gameObject.GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.y < _floorPos && !_floorStopped) {
			_bombRigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
			_floorStopped = true;
		}
		else if (_floorStopped && transform.position.y > _floorPos) {
			_floorStopped = false;
		}
	}

	public void ThrowBomb(){
		if (!_thrownOnce) {
			_bombRigidBody.constraints = RigidbodyConstraints2D.None;
			_bombRigidBody.AddForce ((Vector2.up * 3f + Vector2.left * 0.5f) * 300.0f);
			_thrownOnce = true;
		}
	}
}

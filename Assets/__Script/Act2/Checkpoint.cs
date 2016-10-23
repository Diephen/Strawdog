using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {
	[SerializeField] float _checkPosition = 0.0f;
	FrameScript _frameScript;
	bool _once = false;
	// Use this for initialization
	void Start () {
		_frameScript = GameObject.Find ("Frame").GetComponent<FrameScript> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Checkpoint") {
			_checkPosition = other.transform.position.x;
		}
	}

	void Caught(CaughtSneakingEvent e){
		if (!_once) {
			StartCoroutine (Respawn ());
			_once = true;
		}
	}

	IEnumerator Respawn(){
		yield return new WaitForSeconds(2.0f);
		_frameScript.CloseFlap ();
		yield return new WaitForSeconds(1.0f);
		Vector3 _tempPos = transform.position;
		_tempPos.x = _checkPosition;
		transform.position = _tempPos;
		yield return new WaitForSeconds(1.0f);
		_frameScript.OpenFlap ();
		_once = false;
	}

	void OnEnable(){
		Events.G.AddListener<CaughtSneakingEvent>(Caught);
	}

	void OnDisable(){
		Events.G.RemoveListener<CaughtSneakingEvent>(Caught);
	}
}

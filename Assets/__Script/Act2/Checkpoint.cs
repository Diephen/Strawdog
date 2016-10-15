using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {
	[SerializeField] float _checkPosition = 0.0f;
	Fading _fadingScript;
	// Use this for initialization
	void Start () {
		_fadingScript = GameObject.Find ("Fade").GetComponent<Fading> ();
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
		StartCoroutine (Respawn ());
	}

	IEnumerator Respawn(){
		yield return new WaitForSeconds(2.0f);
		float fadeTime = _fadingScript.BeginFade (1);
		yield return new WaitForSeconds(fadeTime);
		Vector3 _tempPos = transform.position;
		_tempPos.x = _checkPosition;
		transform.position = _tempPos;
		yield return new WaitForSeconds(1.0f);
		_fadingScript.BeginFade (-1);
	}

	void OnEnable(){
		Events.G.AddListener<CaughtSneakingEvent>(Caught);
	}

	void OnDisable(){
		Events.G.RemoveListener<CaughtSneakingEvent>(Caught);
	}
}

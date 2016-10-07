using UnityEngine;
using System.Collections;

public class EnterInteriorTriggerHandle : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void OnTriggerEnter2D(Collider2D other) {
		//Debug.Log ("Attention");
		if(other.name == "GuardStructure"){
			Events.G.Raise(new Act0EndedEvent());
		}

	}
}

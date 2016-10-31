using UnityEngine;
using System.Collections;

public class EndTrigger_Act2Explore : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Prisoner") {
			Events.G.Raise (new StaticCamera ());
			this.enabled = false;
		}
	}
}

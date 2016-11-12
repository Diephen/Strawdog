using UnityEngine;
using System.Collections;

public class AmbientWindTrigger : MonoBehaviour {
	[SerializeField] bool _ambientWind = false;

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Guard" || other.tag == "Prisoner") {
			Events.G.Raise (new TriggerAmbientWindEvent (_ambientWind));
		}
	}
}

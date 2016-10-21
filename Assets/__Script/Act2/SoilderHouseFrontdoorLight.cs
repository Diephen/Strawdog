using UnityEngine;
using System.Collections;

public class SoilderHouseFrontdoorLight : MonoBehaviour {
	Animator m_anim;

	// Use this for initialization
	void Start () {
		m_anim = GetComponent<Animator> ();
	}


	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Prisoner") {
			m_anim.SetTrigger ("TriggerFlicker");
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.tag == "Prisoner") {
			m_anim.SetTrigger ("TriggerFlicker");
		}
	}
}

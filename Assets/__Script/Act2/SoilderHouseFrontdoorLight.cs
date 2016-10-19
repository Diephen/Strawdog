using UnityEngine;
using System.Collections;

public class SoilderHouseFrontdoorLight : MonoBehaviour {
	Animator m_anim;

	// Use this for initialization
	void Start () {
		m_anim = GetComponent<Animator> ();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.name == "FemaleStructure") {
			m_anim.SetTrigger ("TriggerFlicker");
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.name == "FemaleStructure") {
			m_anim.SetTrigger ("TriggerFlicker");
		}
	}
}

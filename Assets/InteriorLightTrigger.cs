using UnityEngine;
using System.Collections;

public class InteriorLightTrigger : MonoBehaviour {
	Animator m_anim;
	// Use this for initialization
	void Start () {
		if (GetComponent<Animator> ()) {
			m_anim = GetComponent<Animator> ();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void OnTriggerEnter2D(Collider2D other) {
		if(other.name == "GuardStructure"){
			m_anim.SetBool ("IsOn", true);
		}


	}

	void OnTriggerExit2D(Collider2D other) {
		if(other.name == "GuardStructure"){
			m_anim.SetBool ("IsOn", false);
		}


	}

}

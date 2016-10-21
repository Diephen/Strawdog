using UnityEngine;
using System.Collections;

public class EndSoldier : MonoBehaviour {
	[SerializeField] Animator m_anim;
	// Use this for initialization
	void Awake () {
		if (GetComponent<Animator> () != null) {
			m_anim = GetComponent<Animator> ();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other) {
		//Debug.Log ("Attention");
		if(other.name == "GuardStructure"){
			m_anim.SetBool ("IsNofuther", true);
			Debug.Log ("Attention");
		}

	}


	void OnTriggerExit2D(Collider2D other) {
		//Debug.Log ("Guard Pass");
		if(other.name == "GuardStructure"){
			m_anim.SetBool ("IsNofuther", false);
			Debug.Log ("Attention");
		}

	}
}

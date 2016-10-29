using UnityEngine;
using System.Collections;

public class ShotDeathPrisonerHandle : MonoBehaviour {
	
	[SerializeField] Animator m_Anim;
	[SerializeField] BoxCollider2D m_StopCol;
	BoxCollider2D m_TriggerCol;
	bool isExecuted = false;


	// Use this for initialization
	void Start () {
		m_TriggerCol = gameObject.GetComponent<BoxCollider2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	
	}

	public void Executed(){
		if (!isExecuted) {
			isExecuted = true;
			m_Anim.Play ("SP-death");
			m_StopCol.enabled = false;
			m_TriggerCol.enabled = false;
		}
	}
}

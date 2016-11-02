using UnityEngine;
using System.Collections;

public class TutorialTree : MonoBehaviour {
	bool m_IsLeaveFall = false;
	[SerializeField] Animator m_Anim;
	[SerializeField] GameObject m_DropFlower;
	int m_BumpCnt = 0;
	// Use this for initialization
	void Start () {
		if (GetComponent<Animator> ()) {
			m_Anim = GetComponent<Animator> ();
		} else {
			print ("No animator");
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (m_BumpCnt >= 2) {
			m_DropFlower.GetComponent<HingeJoint2D> ().enabled = false;
		}
	}


	void OnTriggerEnter2D(Collider2D other) {
		if (!m_IsLeaveFall && other.tag == "Guard") {
			m_Anim.Play ("TreeFall");
			//m_IsLeaveFall = true;
			m_BumpCnt += 1;
		}

	}
}

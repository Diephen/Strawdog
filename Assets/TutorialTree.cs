using UnityEngine;
using System.Collections;

public class TutorialTree : MonoBehaviour {
	bool m_IsLeaveFall = false;
	[SerializeField] Animator m_Anim;
	[SerializeField] GameObject m_DropFlower;
	int m_BumpCnt = 0;
	bool m_IsFlowerGone = false;
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

		if (m_DropFlower != null && !m_IsFlowerGone) {
			if( m_DropFlower.transform.position.y <= -15){
				//Destroy(m_DropFlower);
				m_DropFlower.GetComponent<BoxCollider2D>().enabled = false;
				m_DropFlower.GetComponent<Rigidbody2D> ().isKinematic = true;
				m_IsFlowerGone = true;
			}

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

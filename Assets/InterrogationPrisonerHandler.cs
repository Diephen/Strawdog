using UnityEngine;
using System.Collections;

public class InterrogationPrisonerHandler : MonoBehaviour {
	[SerializeField] Animator m_Anim;
	[SerializeField] GuardHandle m_GuardHandle;
	[SerializeField] AnimationControl m_AnimCtrl;
	[SerializeField] InteractionSound m_ItrAudio;
	// Use this for initialization

	void Awake() {
		if (m_Anim == null) {
			m_Anim = GetComponent<Animator> ();
		}

		//m_AnimCtrl.SetAnimation (true);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ForceToRead(){
		m_AnimCtrl.SetAnimation (true);
//		if(m_Anim.isActiveAndEnabled){
		m_Anim.Play ("IP-ForceRead");
//		}

	}

	public void BackToIdle(){
		m_Anim.Play ("IP-BackToIdle");
	}

	void DisAbleAnim(){
		m_AnimCtrl.SetAnimation (false);
	}
}

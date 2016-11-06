using UnityEngine;
using System.Collections;

public class ExecutionGuardHandle : MonoBehaviour {
	Animator m_Anim;
	AnimationControl m_AnimCtrl;
	[SerializeField] InteractionSound m_ItrSound;

	bool m_IsHandUp = false;


	// Use this for initialization
	void Start () {
		m_Anim = GetComponent<Animator> ();
		m_AnimCtrl = GetComponent<AnimationControl> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void StartExecution(){
		m_Anim.SetBool ("IsHandUp", false);
		m_IsHandUp = false;
		m_AnimCtrl.SetAnimation (true);
	}


	void EndExecution(){
		m_Anim.SetBool ("IsHandUp", false);
		m_IsHandUp = false;
		m_AnimCtrl.SetAnimation (false);
	}

	public void HandUp(){
		m_IsHandUp = true;
		m_Anim.SetBool ("IsHandUp", true);
	}

	public void HandDown(){
		m_IsHandUp = false;
		m_Anim.SetBool ("IsHandUp", false);
	}

	public void Shoot(){
		if (m_IsHandUp) {
			m_Anim.Play ("g-Shoot");
			m_IsHandUp = false;
		}
	}

	void PlayReloadAfterShoot(){
		m_ItrSound.PlayReload ();
	}


}

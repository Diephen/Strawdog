using UnityEngine;
using System.Collections;

public class PrisonerEncounterHandle : MonoBehaviour {
	
	//[SerializeField] GuardEncounterHandle m_GuardHandle;
	Animator m_Anim;
	AnimationControl m_AnimCtrl;
	AnimationInjectionEncounter m_AnimInjection;

	bool m_IsDead = false;

	// Use this for initialization
	void Awake () {
		m_Anim = GetComponent<Animator> ();
		m_AnimCtrl = GetComponent<AnimationControl> ();
		m_AnimInjection = GetComponent<AnimationInjectionEncounter> ();
	}

	void StartAnimation(){
		m_AnimCtrl.SetAnimation (true);
		m_AnimInjection.SetEngage ();
	}

	void EndAnimation(){
		m_AnimCtrl.SetAnimation (false);
		m_AnimInjection.SetLeave ();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Death(){
		if (!m_IsDead) {
			StartAnimation ();
			m_Anim.Play ("p-enct-Death");
			m_IsDead = true;
		}
	}

}

using UnityEngine;
using System.Collections;

public class GuardEncounterHandle : MonoBehaviour {
	[SerializeField] PrisonerEncounterHandle m_PrisonerHandle;
	[SerializeField] BoxCollider2D m_BlockColl;
	//SpriteRenderer[] m_WhiteBase;
	Animator m_Anim;
	AnimationControl m_AnimCtrl;
	AnimationInjectionEncounter m_AnimInjection;
	bool m_IsHandUp = false;
	bool m_IsShot = false;
	// Use this for initialization
	void Awake () {
		m_Anim = GetComponent<Animator> ();
		m_AnimCtrl = GetComponent<AnimationControl> ();
		m_AnimInjection = GetComponent<AnimationInjectionEncounter> ();
	}

	void Start(){
		StartAnimation ();

	}

	void StartAnimation(){
		m_AnimCtrl.SetAnimation (true);
		m_Anim.Play("g-ecnt-Idle");
		m_BlockColl.enabled = false;
		m_IsHandUp = false;
		m_AnimInjection.SetEngage ();
	}

	void EndAnimation(){
		m_AnimCtrl.SetAnimation (false);
		m_AnimInjection.SetLeave ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void HandUp(){
		if (!m_IsShot) {
			m_Anim.Play ("g-ecnt-HoldGunUp");
			m_BlockColl.enabled = true;
			m_IsHandUp = true;
			m_PrisonerHandle.GuardHoldGun ();
		}
	}

	public void HandDown(){
		if (!m_IsShot) {
			m_Anim.Play("g-ecnt-GunDown");
			m_BlockColl.enabled = false;
			Events.G.Raise (new EnableMoveEvent (CharacterIdentity.Prisoner));
			m_IsHandUp = false;
			m_PrisonerHandle.GuardReleaseGun ();
		}

	}

	public void Shoot(){
		if (m_IsHandUp && !m_IsShot) {
			m_Anim.Play ("g-enct-Shoot");
			m_PrisonerHandle.Death ();
			m_IsShot = true;
		}

	}

	public void Interacte(){
		if (!m_IsHandUp && !m_IsShot) {
			m_Anim.Play ("g-enct-HoldHand");
		}
	}
}

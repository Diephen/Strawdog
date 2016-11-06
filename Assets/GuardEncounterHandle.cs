using UnityEngine;
using System.Collections;

public class GuardEncounterHandle : MonoBehaviour {
	[SerializeField] PrisonerEncounterHandle m_PrisonerHandle;
	//[SerializeField] BoxCollider2D m_BlockColl;
	//SpriteRenderer[] m_WhiteBase;
	Animator m_Anim;
	AnimationControl m_AnimCtrl;
	AnimationInjectionEncounter m_AnimInjection;
	PuppetControl m_PuppetControl;
	bool m_IsHandUp = false;
	bool m_IsShot = false;
	bool m_IsTouch = false;
	bool m_IsEnd = false;
	// Use this for initialization
	void OnEnable(){
		Events.G.AddListener<EncounterTouchEvent>(OnPrisonerTouchGuard);
	}

	void OnDisable(){
		Events.G.RemoveListener<EncounterTouchEvent>(OnPrisonerTouchGuard);
	}

	void OnPrisonerTouchGuard(EncounterTouchEvent e){
		m_IsTouch = e.OnGuard;
	}

	void Awake () {
		m_Anim = GetComponent<Animator> ();
		m_AnimCtrl = GetComponent<AnimationControl> ();
		m_AnimInjection = GetComponent<AnimationInjectionEncounter> ();
		m_PuppetControl = GetComponent<PuppetControl> ();
	}

	void Start(){
		Events.G.Raise (new DisableMoveEvent (CharacterIdentity.Guard));
		StartAnimation ();
	}

	void StartAnimation(){
		m_AnimCtrl.SetAnimation (true);
		m_Anim.Play("g-ecnt-Idle");
		//m_BlockColl.enabled = false;
		m_IsHandUp = false;
		m_AnimInjection.SetEngage ();
	}

	void EndAnimation(){
		m_AnimCtrl.SetAnimation (false);
		m_AnimInjection.SetLeave ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!m_IsEnd && m_IsTouch && !m_IsShot && !m_IsHandUp) {
			if (Input.GetKeyDown (m_PuppetControl.GetKeyCodes()[3])) {
				GiveHand ();
				//m_AnimInjection.GEndState (true);
				Events.G.Raise(new EncountEndStateEvent(CharacterIdentity.Guard, true));
			}

			if (Input.GetKeyUp (m_PuppetControl.GetKeyCodes () [3])) {
				WithdrawHand ();
				//m_AnimInjection.GEndState (false);
				Events.G.Raise(new EncountEndStateEvent(CharacterIdentity.Guard, false));
			}
		}
	
	}

	public void SetTouch(bool touch){
		m_IsTouch = touch;
	}

	public void HandUp(){
		if (!m_IsShot) {
			m_Anim.Play ("g-ecnt-HoldGunUp");
			//m_BlockColl.enabled = true;
			m_PrisonerHandle.OnGunUpIdle();
			m_IsHandUp = true;
			m_PrisonerHandle.GuardHoldGun ();
		}
	}

	public void HandDown(){
		if (!m_IsShot) {
			m_Anim.Play("g-ecnt-GunDown");
			m_PrisonerHandle.OnGunDownFree();
			//m_BlockColl.enabled = false;

			m_IsHandUp = false;
			m_PrisonerHandle.GuardReleaseGun ();
		}

	}

	public void Shoot(){
		if (m_IsHandUp && !m_IsShot) {
			m_Anim.Play ("g-enct-Shoot");
			m_PrisonerHandle.Death ();
			m_IsShot = true;
			Events.G.Raise (new PrisonerShotEvent ());

		}

	}

	void EnableGuardMovement(){
		m_AnimCtrl.SetAnimation (false);
		m_AnimInjection.SetLeave ();
		Events.G.Raise (new EnableMoveEvent (CharacterIdentity.Guard));

	}

	public void GiveHand(){
		m_Anim.Play ("g-enct-GiveHandToPrisoner");
	}

	public void WithdrawHand(){
		m_Anim.Play("g-enct-WithDrawHand");
	}


	public IEnumerator Hug(){
		yield return new WaitForSeconds (1);
		if (!m_IsEnd) {
			m_Anim.Play ("g-enct-End");
			m_IsEnd = true;
			Events.G.Raise (new RunTogetherEndingEvent ());
		}
		m_PrisonerHandle.Hug ();

	}
}

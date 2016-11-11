using UnityEngine;
using System.Collections;

public class GuardIntroHandle : MonoBehaviour {
	[SerializeField] PrisonerEncounterHandle m_PrisonerHandle;
	[SerializeField] BoxCollider2D m_GuardsEncounterTrigger;
	//[SerializeField] BoxCollider2D m_BlockColl;
	//SpriteRenderer[] m_WhiteBase;
	Animator m_Anim;
	AnimationControl m_AnimCtrl;
	AnimationInjectionIntro m_AnimInjection;
	PuppetControl m_PuppetControl;

	bool m_IsTouch = false;

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
		m_AnimInjection = GetComponent<AnimationInjectionIntro> ();
		m_PuppetControl = GetComponent<PuppetControl> ();
	}

	void StartAnimation(){
		m_AnimCtrl.SetAnimation (true);
		m_Anim.Play("g-ecnt-Idle");
		//m_BlockColl.enabled = false;
		m_AnimInjection.SetEngage ();
	}

	void EndAnimation(){
		m_AnimCtrl.SetAnimation (false);
		m_AnimInjection.SetLeave ();
	}

	// Update is called once per frame
	void Update () {
		if (m_IsTouch) {
			if (Input.GetKeyDown (m_PuppetControl.GetKeyCodes()[3])) {
				StartAnimation ();
				GiveHand ();
				//m_AnimInjection.GEndState (true);
				Events.G.Raise(new EncountEndStateEvent(CharacterIdentity.Guard, true));
			}

			if (Input.GetKeyUp (m_PuppetControl.GetKeyCodes () [3])) {
				WithdrawHand ();
				EndAnimation ();
				//m_AnimInjection.GEndState (false);
				Events.G.Raise(new EncountEndStateEvent(CharacterIdentity.Guard, false));
			}
		}

	}

	public void SetTouch(bool touch){
		m_IsTouch = touch;
	}

	public void GiveHand(){
		m_Anim.Play ("g-enct-GiveHandToPrisoner");
	}

	public void WithdrawHand(){
		m_Anim.Play("g-enct-WithDrawHand");
	}
}

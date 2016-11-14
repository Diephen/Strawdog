using UnityEngine;
using System.Collections;

public class ExecutionPrisonerHandle : MonoBehaviour {
	Animator m_Anim;
	AnimationControl m_AnimCtrl;
	AnimationInjectionExecution m_AnimInjection;
	PuppetControl m_PuppetControl;
	BoxCollider2D[] m_PrisonerColl;
	DragJitter m_Jitter;
	SpriteRenderer[] m_Sprites;
	[SerializeField] Color m_EndColor;
	[SerializeField] Color m_StartColor;
	[SerializeField] float m_Duration;
	[SerializeField] InteractionSound m_ItrSound;
	Timer m_ColorTimer;
	//[SerializeField] ExecutionGuardHandle m_GuardHandle;
	bool m_IsFree = false;
	bool m_IsPrisonerDead = false;
	bool m_IsEncounterGuard = false;

	// Use this for initialization
	void Awake () {
		m_Anim = GetComponent<Animator> ();
		m_AnimCtrl = GetComponent<AnimationControl> ();
		m_AnimInjection = GetComponent<AnimationInjectionExecution> ();
		m_PuppetControl = GetComponent<PuppetControl> ();
		m_PrisonerColl = GetComponentsInChildren<BoxCollider2D> ();
		m_Jitter = GetComponent<DragJitter> ();
		m_Sprites = gameObject.GetComponentsInChildren<SpriteRenderer> ();
//		foreach (SpriteRenderer spr in m_Sprites) {
//			spr.material.color = m_StartColor;
//		}
		m_ColorTimer = new Timer (m_Duration);
	
	}

	void Start(){
		StartAnimation ();
	}
	
	// Update is called once per frame
	void Update () {
		if (m_IsPrisonerDead) {
			TurnDark ();
		}
	
	}

	void OnEnable(){
		Events.G.AddListener<ExecutionBreakFree> (OnPrisonerBreak);

	}

	void OnDisable(){
		Events.G.RemoveListener<ExecutionBreakFree> (OnPrisonerBreak);
	}

	void OnPrisonerBreak(ExecutionBreakFree e){
		print ("P Free stop animation");
		m_IsFree = true;
		EndAnimtion ();
	}

	void StartAnimation(){
		m_AnimCtrl.SetAnimation (true);
		m_AnimInjection.SetEngage ();
	}

	void EndAnimtion(){
		m_AnimCtrl.SetAnimation (false);
		m_AnimInjection.SetLeave ();
	}

	public void EncounterGuard(){
		StartAnimation ();
		m_IsEncounterGuard = true;
		m_PuppetControl.DisableKeyInput ();
		m_Jitter.DisableJitter ();
		//m_Jitter.DisableJitter ();
		if (m_IsFree) {
			m_Anim.Play ("p-exe-StandIdle");
		} else {
			m_Anim.Play ("p-exe-Bond");
		}
	}

	public void Struggle(bool isLeft){
		if (!m_IsFree && !m_IsPrisonerDead && !m_IsEncounterGuard) {
			if (isLeft) {
				m_Anim.Play ("p-exe-StruggleLeft");
			}else{
				m_Anim.Play ("p-exe-StruggleRight");
			}
		}

	}

	void TurnDark(){
		foreach (SpriteRenderer spr in m_Sprites) {
			spr.material.color = Color.Lerp (m_StartColor, m_EndColor, m_ColorTimer.PercentTimePassed);
		}
		//lerpedColor = Color.Lerp(Color.white, Color.black, Time.deltaTime);
	}

	public void Death(){
		if (!m_IsPrisonerDead) {
			m_ColorTimer.Reset ();
			StartAnimation ();
			if (m_IsFree) {
				m_Anim.Play ("p-exe-StandDeath");
				print ("P Stand and die");
			} else {
				m_Anim.Play ("p-exe-SitDeath");
				print ("P Sit and die");
			}
			foreach (BoxCollider2D bc in m_PrisonerColl) {
				bc.enabled = false;
			}
			m_IsPrisonerDead = true;
		}

	}

	public void OnGunUp(){
		if (!m_IsPrisonerDead) {
			if (m_IsFree) {
				m_Anim.Play ("p-exe-StandGunUp");
			}
		}

	}

	public void OnGunDown(){
		if (!m_IsPrisonerDead) {
			if (m_IsFree) {
				m_Anim.Play ("p-exe-StandIdle");
			} else {
				m_Anim.Play ("p-exe-Bond");
			}
		}

	}

	void PlayBodyFall(){
		m_ItrSound.PlayPrisonerFall ();
	}

	void LoadEnding(){
		Events.G.Raise (new GuardExecutePrisoner ());
	}


}

using UnityEngine;
using System.Collections;

public class PrisonerEncounterHandle : MonoBehaviour {
	GameObject[] m_PWhiteBase;
	GameObject[] m_GWhiteBase;
	[SerializeField] float m_Speed;
	[SerializeField] Color m_StartColor;
	[SerializeField] Color m_EndColor;
	[SerializeField] BoxCollider2D[] m_Colls;
	bool isLightUp = false;
	//[SerializeField] GuardEncounterHandle m_GuardHandle;
	Animator m_Anim;
	AnimationControl m_AnimCtrl;
	AnimationInjectionEncounter m_AnimInjection;
	PuppetControl m_PuppetControl;
	[SerializeField] Transform m_HugPos;
	// touch 
	bool m_IsTouch = false;
	bool m_IsEnd = false;

	bool m_IsDead = false;

	void OnEnable(){
		Events.G.AddListener<EncounterTouchEvent>(OnPrisonerTouchGuard);
	}

	void OnDisable(){
		Events.G.RemoveListener<EncounterTouchEvent>(OnPrisonerTouchGuard);
	}

	// Use this for initialization
	void Awake () {
		m_Anim = GetComponent<Animator> ();
		m_AnimCtrl = GetComponent<AnimationControl> ();
		m_AnimInjection = GetComponent<AnimationInjectionEncounter> ();
		m_PWhiteBase = GameObject.FindGameObjectsWithTag ("PWhiteBase");
		m_GWhiteBase = GameObject.FindGameObjectsWithTag ("GWhiteBase");
		m_PuppetControl = GetComponent<PuppetControl> ();
		m_Colls = GetComponentsInChildren<BoxCollider2D> ();
	}

	void StartAnimation(){
		m_AnimCtrl.SetAnimation (true);
		//m_Anim.Play ("p-enct-Idle");
		m_AnimInjection.SetEngage ();


	}

	void EndAnimation(){
		m_AnimCtrl.SetAnimation (false);
		m_AnimInjection.SetLeave ();
	}

	void OnPrisonerTouchGuard(EncounterTouchEvent e){
		if (!m_IsEnd) {
			if (e.OnGuard) {
				m_IsTouch = true;
				LightUpPuppet ();
				//StartAnimation ();
			} else {
				m_IsTouch = false;
				DimPuppet ();
				//EndAnimation ();
			}
		}
	}

	void LightUpPuppet(){
		foreach (GameObject spr in m_PWhiteBase) {
			spr.GetComponent<SpriteRenderer> ().color = m_EndColor;

		}
		foreach (GameObject spr in m_GWhiteBase) {
			spr.GetComponent<SpriteRenderer> ().color = m_EndColor;

		}
	}

	void DimPuppet(){
		foreach (GameObject spr in m_PWhiteBase) {
			spr.GetComponent<SpriteRenderer> ().color = m_StartColor;
		}
		foreach (GameObject spr in m_GWhiteBase) {
			spr.GetComponent<SpriteRenderer> ().color = m_StartColor;

		}
	}

	// Use this for initialization
	void Start () {
		DimPuppet ();
	}
	
	// Update is called once per frame
	void Update () {
		#if UNITY_EDITOR
		if(Input.GetKeyDown(KeyCode.B)){
			//Debug.Log ("Toggle Animtion and Rigidbody");
			isLightUp = !isLightUp;

			if (isLightUp) {
				LightUpPuppet ();
			} else {
				DimPuppet();
			}
		}
		#endif

		if (m_IsTouch && !m_IsDead && !m_IsEnd) {
			if (Input.GetKeyDown (m_PuppetControl.GetKeyCodes()[3])) {
				StartAnimation ();
				GiveHand ();
				//m_AnimInjection.PEndState (true);
				Events.G.Raise(new EncountEndStateEvent(CharacterIdentity.Prisoner, true));
			}

			if (Input.GetKeyUp (m_PuppetControl.GetKeyCodes () [3])) {
				EndAnimation ();
				//m_AnimInjection.PEndState (false);
				Events.G.Raise(new EncountEndStateEvent(CharacterIdentity.Prisoner, false));
			}
		}

		if (m_IsEnd) {
			MoveToFinalPos ();
		}
	}

	public void OnGunUpIdle(){
		m_AnimCtrl.SetAnimation (true);
		m_Anim.Play ("p-enct-OnGunIdle");
		m_AnimInjection.SetEngage ();
	}

	public void OnGunDownFree(){
		m_AnimCtrl.SetAnimation (false);
		//m_Anim.Play ("p-enct-Idle");
		m_AnimInjection.SetLeave ();
	}

	public void Death(){
		if (!m_IsDead) {
			StartAnimation ();
			m_Anim.Play ("p-enct-Death");
			m_IsDead = true;
			foreach (BoxCollider2D bc in m_Colls) {
				bc.enabled = false;
			}
		}
	}

	public void GuardHoldGun(){
		if (m_IsTouch) {
			DimPuppet ();
		}
	}

	public void GuardReleaseGun(){
		if (m_IsTouch) {
			LightUpPuppet ();
		}
	}

	public void GiveHand(){
		if (m_IsTouch) {
			m_Anim.Play ("p-enct-GiveHand");
		}
	}

	public void WithdrawHand(){
		if (m_IsTouch) {
			m_Anim.Play ("p-enct-WithDrawHand");
		}
	}

	public void Hug(){
		if (!m_IsEnd) {
			//transform.position = m_HugPos.position;
			LightUpPuppet();
			m_IsEnd = true;
			m_Anim.Play ("p-enct-End");
		}
	}

	void MoveToFinalPos(){
		Vector3 npos = transform.position;
		if (transform.position.x + m_Speed * Time.deltaTime < m_HugPos.position.x) {
			npos.x += m_Speed * Time.deltaTime;
			transform.position = npos;
		} else {
			npos.x = m_HugPos.position.x;
		}

	}


}

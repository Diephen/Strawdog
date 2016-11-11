﻿using UnityEngine;
using System.Collections;

public class PrisonerIntroHandle : MonoBehaviour {
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
	// touch 
	bool m_IsTouch = false;

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

		if (m_IsTouch) {
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
}

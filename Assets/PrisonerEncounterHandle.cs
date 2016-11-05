using UnityEngine;
using System.Collections;

public class PrisonerEncounterHandle : MonoBehaviour {
	GameObject[] m_WhiteBase;
	[SerializeField] Color m_StartColor;
	[SerializeField] Color m_EndColor;
	bool isLightUp = false;
	//[SerializeField] GuardEncounterHandle m_GuardHandle;
	Animator m_Anim;
	AnimationControl m_AnimCtrl;
	AnimationInjectionEncounter m_AnimInjection;

	// touch 
	bool m_IsTouch = false;


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
		m_WhiteBase = GameObject.FindGameObjectsWithTag ("WhiteBase");
	}

	void StartAnimation(){
		m_AnimCtrl.SetAnimation (true);
		m_AnimInjection.SetEngage ();
		DimPuppet ();
	}

	void EndAnimation(){
		m_AnimCtrl.SetAnimation (false);
		m_AnimInjection.SetLeave ();
	}

	void OnPrisonerTouchGuard(EncounterTouchEvent e){
		if (e.OnGuard) {
			m_IsTouch = true;
			LightUpPuppet ();
		} else {
			m_IsTouch = false;
			DimPuppet ();
		}
		
	}

	void LightUpPuppet(){
		foreach (GameObject spr in m_WhiteBase) {
			spr.GetComponent<SpriteRenderer> ().color = m_EndColor;

		}
	}

	void DimPuppet(){
		foreach (GameObject spr in m_WhiteBase) {
			spr.GetComponent<SpriteRenderer> ().color = m_StartColor;
		}
	}

	// Use this for initialization
	void Start () {
	
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
	
	}

	public void Death(){
		if (!m_IsDead) {
			StartAnimation ();
			m_Anim.Play ("p-enct-Death");
			m_IsDead = true;
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


}

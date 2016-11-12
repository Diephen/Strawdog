using UnityEngine;
using System.Collections;

public class DitchPrisonerHandle : MonoBehaviour {
	Animator m_Anim;
	AnimationControl m_AnimCtrl;
	AnimationInjectionDitch m_AnimInjection;
	PuppetControl m_PuppetControl;
	Act4_PrisonerTrigger m_PrisonerTrigger;
	HingeJoint2D[] m_AllJoints;
	[SerializeField] BoxCollider2D[] m_WholeBodyColl;
	[SerializeField] Vector2 m_Force;
	Rigidbody2D[] m_Body;
	SpriteRenderer[] m_Sprites;

	[SerializeField] HingeJoint2D[] m_StringJoints;

	//[SerializeField] ExecutionGuardHandle m_GuardHandle;
	bool m_IsFree = false;

	// Use this for initialization
	void Awake () {
		m_Anim = GetComponent<Animator> ();
		m_AnimCtrl = GetComponent<AnimationControl> ();
		m_AnimInjection = GetComponent<AnimationInjectionDitch> ();
		m_PrisonerTrigger = GetComponentInChildren<Act4_PrisonerTrigger> ();
		m_AllJoints = GetComponentsInChildren<HingeJoint2D> ();
		m_Body = new Rigidbody2D[m_WholeBodyColl.Length];
		for (int i = 0; i < m_WholeBodyColl.Length; i++) {
			m_Body [i] = m_WholeBodyColl [i].gameObject.GetComponent<Rigidbody2D> ();
			m_WholeBodyColl [i].enabled = false;
		}
		//m_WholeBodyColl.enabled = false;
		m_Sprites = GetComponentsInChildren<SpriteRenderer>();

	}

	// Use this for initialization
	void Start () {
		StartAnimation ();

	}
	
	// Update is called once per frame
	void Update () {
		#if UNITY_EDITOR
		if(Input.GetKeyDown(KeyCode.C)){
			Death();
		}

		if(Input.GetKeyDown(KeyCode.Space)){
			foreach(Rigidbody2D rig in m_Body){
				rig.AddForce(m_Force);
			}

		}
		#endif
	
	}

	void OnEnable(){
		Events.G.AddListener<BrokeFree> (OnPrisonerBreakDitchLine);
	}

	void OnDisable(){
		Events.G.RemoveListener<BrokeFree> (OnPrisonerBreakDitchLine);
	}

	void OnPrisonerBreakDitchLine(BrokeFree e){
		print ("P Free stop animation");
		m_IsFree = true;
		EndAnimtion ();
		m_PrisonerTrigger.UpdatePrisonerState (m_IsFree);
	}

	void StartAnimation(){
		m_AnimCtrl.SetAnimation (true);
		m_Anim.Play("p-ditch-BondIdle");
		m_AnimInjection.SetEngage ();
	}

	void EndAnimtion(){
		m_AnimCtrl.SetAnimation (false);
		m_AnimInjection.SetLeave ();
	}
		

	public void Struggle(bool isLeft){
		if (!m_IsFree) {
			if (isLeft) {
				m_Anim.Play ("p-ditch-StruggleLeft");
			}else{
				m_Anim.Play ("p-ditch-StruggleRight");
			}
		}

	}

	public void BondIdle(){
		if (!m_IsFree) {
			m_Anim.Play ("p-ditch-BondIdle");
		}

	}

	public void PrisonerStop(){
		// play scard animation

		Events.G.Raise (new DisableMoveEvent (CharacterIdentity.Prisoner));
	}

	public void Death(){
		//m_AnimCtrl.SetAnimation (true);
		//m_AnimInjection.SetEngage ();
		print ("P death");
		//m_Anim.Play ("p-ditch-Die");
		foreach(BoxCollider2D bc in m_WholeBodyColl){
			bc.enabled = true;
		}
		foreach (HingeJoint2D hj in m_StringJoints){
			hj.enabled = false;
		}
	}

	public void Kick(){
		foreach(Rigidbody2D rig in m_Body){
			rig.AddForce(m_Force);
		}

		foreach (SpriteRenderer spr in m_Sprites) {
			spr.sortingOrder -= 6;
		}
	}
}

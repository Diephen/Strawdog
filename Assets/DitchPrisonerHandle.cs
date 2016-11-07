using UnityEngine;
using System.Collections;

public class DitchPrisonerHandle : MonoBehaviour {
	Animator m_Anim;
	AnimationControl m_AnimCtrl;
	AnimationInjectionDitch m_AnimInjection;
	PuppetControl m_PuppetControl;
	Act4_PrisonerTrigger m_PrisonerTrigger;
	//[SerializeField] ExecutionGuardHandle m_GuardHandle;
	bool m_IsFree = false;

	// Use this for initialization
	void Awake () {
		m_Anim = GetComponent<Animator> ();
		m_AnimCtrl = GetComponent<AnimationControl> ();
		m_AnimInjection = GetComponent<AnimationInjectionDitch> ();
		m_PrisonerTrigger = GetComponentInChildren<Act4_PrisonerTrigger> ();
	}

	// Use this for initialization
	void Start () {
		StartAnimation ();
	}
	
	// Update is called once per frame
	void Update () {
	
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
}

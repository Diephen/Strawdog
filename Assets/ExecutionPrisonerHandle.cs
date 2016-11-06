using UnityEngine;
using System.Collections;

public class ExecutionPrisonerHandle : MonoBehaviour {
	Animator m_Anim;
	AnimationControl m_AnimCtrl;
	AnimationInjectionExecution m_AnimInjection;
	PuppetControl m_PuppetControl;
	//[SerializeField] ExecutionGuardHandle m_GuardHandle;
	bool m_IsFree = false;

	// Use this for initialization
	void Awake () {
		m_Anim = GetComponent<Animator> ();
		m_AnimCtrl = GetComponent<AnimationControl> ();
		m_AnimInjection = GetComponent<AnimationInjectionExecution> ();
	
	}

	void Start(){
		StartAnimation ();
	}
	
	// Update is called once per frame
	void Update () {
	
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

	public void Struggle(bool isLeft){
		if (!m_IsFree) {
			if (isLeft) {
				m_Anim.Play ("p-exe-StruggleLeft");
			}else{
				m_Anim.Play ("p-exe-StruggleRight");
			}
		}

	}


}

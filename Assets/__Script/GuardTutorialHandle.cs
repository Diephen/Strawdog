using UnityEngine;
using System.Collections;

public class GuardTutorialHandle : MonoBehaviour {
	[SerializeField] Animator m_GuardAnim;
	[SerializeField] DogHandle m_DogHandle;
	[SerializeField] AnimationControl m_AnimCtrl;
	[SerializeField] PuppetControl m_PC;
	[SerializeField] InteractionSound m_ItrAudio;

	// Use this for initialization
	void Start () {
		m_GuardAnim = GetComponent<Animator> () ? GetComponent<Animator> () : null;
		m_PC = GetComponent<PuppetControl> () ? GetComponent<PuppetControl> () : null;
		m_AnimCtrl = GetComponent<AnimationControl> () ? GetComponent<AnimationControl> () : null;
		m_DogHandle = GameObject.FindObjectOfType<DogHandle> ().GetComponent<DogHandle> ();
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void StartDogInteraction(){
		//m_AnimCtrl.SetAnimation (true);
		m_AnimCtrl.SetAnimation (true);
		m_GuardAnim.SetBool ("IsSeeDog", true);
	}

	public void LeaveDog(){
		m_GuardAnim.SetBool ("IsSeeDog", false);
	}

	public void DisableAnim(){
		m_AnimCtrl.SetAnimation (false);
	}
}

using UnityEngine;
using System.Collections;

public class GuardTutorialHandle : MonoBehaviour {
	[SerializeField] Animator m_GuardAnim;
	[SerializeField] DogHandle m_DogHandle;
	[SerializeField] AnimationControl m_AnimCtrl;
	[SerializeField] PuppetControl m_PC;
	[SerializeField] InteractionSound m_ItrAudio;
	[SerializeField] AnimationInjectionTutorial m_AnimInjection;
	bool isEndInteraction=false;
	bool isStartInteraction = false;
	bool isPetting = false;


	// Use this for initialization
	void Start () {
		m_GuardAnim = GetComponent<Animator> () ? GetComponent<Animator> () : null;
		m_PC = GetComponent<PuppetControl> () ? GetComponent<PuppetControl> () : null;
		m_AnimCtrl = GetComponent<AnimationControl> () ? GetComponent<AnimationControl> () : null;
		m_DogHandle = GameObject.FindObjectOfType<DogHandle> ().GetComponent<DogHandle> ();
		m_AnimInjection = GameObject.FindObjectOfType<GuardTutorialHandle> ().GetComponent<AnimationInjectionTutorial> ();
	}

	public void StartDogInteraction(){
		//m_AnimCtrl.SetAnimation (true);
		m_AnimCtrl.SetAnimation (true);
		m_GuardAnim.SetBool ("IsSeeDog", true);
		isStartInteraction = false;
		isEndInteraction = false;
	}

	// guard press D
	public void LeaveDog(){
		if (!isEndInteraction) {
			m_GuardAnim.SetBool ("IsSeeDog", false);
			isEndInteraction = true;
			if (isPetting) {
				ReleasePet ();
				m_ItrAudio.StopPlay ();
			}
			//m_GuardAnim.SetBool ("IsPetting", false);
			m_DogHandle.PersonLeft ();
		}


	}

	public void DisableAnim(){
		m_AnimCtrl.SetAnimation (false);
		m_AnimInjection.SetLeave ();
	}

	public void PetDog(){
		if (isStartInteraction && !isEndInteraction) {
			isPetting = true;
			m_GuardAnim.SetBool ("IsPetting", true);
			m_DogHandle.Pet ();
		}

	}

	public void ReleasePet(){
		if (isStartInteraction && !isEndInteraction) {
			isPetting = false;
			m_GuardAnim.SetBool ("IsPetting", false);
			m_DogHandle.ReleasePet ();
		}
	}

	public void UpdatePetTime(float t){
		m_GuardAnim.SetFloat ("PetTime", t);
	}

	public void DogHappy(){
		m_GuardAnim.SetTrigger ("TriggerDogHappy");
		isEndInteraction = true;
	}

	public void EnablePetting(){
		if (!isStartInteraction) {
			isStartInteraction = true;	
			Debug.Log ("Enable Petting");
		}

	}
}

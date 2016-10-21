﻿using UnityEngine;
using System.Collections;

public class DogHandle : MonoBehaviour {
	enum DogState{
		idle,
		start,
		beg,
		touched,
		leave
	}

	DogState m_DogState;
	Animator m_Anim;
	PuppetControl m_PC;
	GuardTutorialHandle m_GuardHandle;
	AnimationInjectionTutorial m_AnimInjection;
	[SerializeField] Animator m_UIAnim;

	float m_StartPetTime;
	float m_PetTime = 0f;
	bool m_IsPetting = false;
	bool m_IsDogHappy = false;

	// Use this for initialization
	void Start () {
		m_PC = GameObject.FindObjectOfType<GuardTutorialHandle> ().GetComponent<PuppetControl> ();
		m_GuardHandle = GameObject.FindObjectOfType<GuardTutorialHandle> ().GetComponent<GuardTutorialHandle> ();
		m_Anim = GetComponent<Animator> () ? GetComponent<Animator> () : null;
		m_AnimInjection = GameObject.FindObjectOfType<GuardTutorialHandle> ().GetComponent<AnimationInjectionTutorial> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (m_IsPetting) {
			m_PetTime = Time.time - m_StartPetTime;
			m_Anim.SetFloat ("PetTime", m_PetTime);
			m_GuardHandle.UpdatePetTime (m_PetTime);
		} else {
			m_PetTime = 0;
		}

		if (m_PetTime >= 5.0f) {
			if (!m_IsDogHappy) {
				m_IsDogHappy = true;
				DogHappy ();
			}


		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		//Debug.Log ("Attention");
		if(m_DogState == DogState.idle && other.name == "GuardStructure"){
			StopPlayer ();
			m_AnimInjection.SetEngage ();
			m_DogState = DogState.start;
			CheckState (m_DogState);
			Debug.Log ("wiggle wiggle");
		}

	}

	void OnTriggerExit2D(Collider2D other) {
		//Debug.Log ("Attention");
		if(other.name == "GuardStructure"){
			LeavePlayer ();
			m_DogState = DogState.idle;
			CheckState (m_DogState);
			//Debug.Log ("bye bye");
		}

	}

	void CheckState(DogState dgs)
	{
		switch (dgs) {
		case DogState.idle:
			break;
		case DogState.start:
			m_GuardHandle.StartDogInteraction ();
			WalkToPlayer ();
			break;
		case DogState.beg:
			
			break;
		case DogState.touched:
			
			break;
		case DogState.leave:
			break;

		}
		
	}

	public void WalkToPlayer(){
		if (m_Anim != null) {
			m_Anim.SetBool ("IsStartWalk", true);
		}

	}

	void StopPlayer(){
		if (m_PC._stateHandling [3]) {
			m_PC._stateHandling [3] = false;
		}

	}

	void LeavePlayer(){
		if (!m_PC._stateHandling [3]) {
			m_PC._stateHandling [3] = true;
		}
	}

	public void Pet(){
		//m_DogState = DogState.touched;
		//CheckState (m_DogState);
		m_Anim.SetBool ("IsPetting", true);	
		m_IsPetting = true;
		m_StartPetTime = Time.time;

		//get pet 
	}

	public void ReleasePet(){
		m_Anim.SetBool ("IsPetting", false);	
		m_IsPetting = false;
		m_Anim.SetFloat ("PetTime", 0f);
		m_GuardHandle.UpdatePetTime (0f);
		//m_StartPetTime = Time.time;

		// ask for pet 
	}

	public void PersonLeft(){
		m_Anim.SetBool ("IsPetting", false);
		m_Anim.SetBool ("IsStartWalk", false);
		m_IsPetting = false;
		//LeavePlayer ();

		// stop begging 
	}

	public void DogHappy(){
		
		gameObject.GetComponent<BoxCollider2D> ().enabled = false;
		m_Anim.SetTrigger ("TriggerDogHappy");
		m_GuardHandle.DogHappy ();

	}

	public void EndInteraction(){
		LeavePlayer ();
		m_Anim.SetBool ("IsPetting", false);
		m_Anim.SetBool ("IsStartWalk",false);
	}

	public void UIShowPet(){
		m_UIAnim.SetBool ("IsBeg", true);
		//m_UIAnim.Play("UI-Petme");
		print ("Bark Bark");
	}

	public void UIHidePet(){
		m_UIAnim.SetBool ("IsBeg", false);
		m_UIAnim.Play("UI-Idle");
	}



}

using UnityEngine;
using System.Collections;

public class DogHandle : MonoBehaviour {
	enum DogState{
		idle,
		start,
		beg,
		touched,
		leave
	}

	[SerializeField] DogState m_DogState;
	[SerializeField] Animator m_Anim;
	[SerializeField] PuppetControl m_PC;
	[SerializeField] GuardTutorialHandle m_GuardHandle;


	// Use this for initialization
	void Start () {
		m_PC = GameObject.FindObjectOfType<GuardTutorialHandle> ().GetComponent<PuppetControl> ();
		m_GuardHandle = GameObject.FindObjectOfType<GuardTutorialHandle> ().GetComponent<GuardTutorialHandle> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other) {
		//Debug.Log ("Attention");
		if(m_DogState == DogState.idle && other.name == "GuardStructure"){
			StopPlayer ();
			m_DogState = DogState.start;
			CheckState (m_DogState);
			Debug.Log ("waggle waggle");
		}

	}

	void OnTriggerExit2D(Collider2D other) {
		//Debug.Log ("Attention");
		if(other.name == "GuardStructure"){
			LeavePlayer ();
			m_DogState = DogState.idle;
			CheckState (m_DogState);
			Debug.Log ("bye bye");
		}

	}

	void CheckState(DogState dgs)
	{
		switch (dgs) {
		case DogState.idle:
			
			break;
		case DogState.start:
			m_GuardHandle.StartDogInteraction ();
			break;
		case DogState.beg:
			break;
		case DogState.touched:
			break;
		case DogState.leave:
			break;

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



}

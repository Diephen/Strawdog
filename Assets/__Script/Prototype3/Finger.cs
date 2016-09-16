using UnityEngine;
using System.Collections;

public class Finger : MonoBehaviour {
	[SerializeField] Transform m_FingerH;         // the height of the finger 
	[SerializeField] KeyCode m_ListenKey;
	[SerializeField] float m_MoveDist;
	[SerializeField] bool m_IsActive = false;
	[SerializeField] AudioClip m_sound;
	[SerializeField] AudioSource m_AS;
	[SerializeField] bool m_Mouth = false;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (m_Mouth) {
			if (Input.GetKeyDown (m_ListenKey)) {
				
				FingerDown ();
				if (!m_AS.isPlaying) {
					m_AS.Play ();
					if (Input.GetKey (KeyCode.A) && Input.GetKey (KeyCode.S) && Input.GetKey (KeyCode.D)) {
						m_AS.Stop ();
					}
				}
			}

			if (Input.GetKeyUp (m_ListenKey)) {
				FingerUp ();
			}
		} else {
			if (m_IsActive) {
				if (Input.GetKeyDown (m_ListenKey)) {
					Debug.Log ("Finger Up");
					FingerUp ();
					if (!m_AS.isPlaying) {
						m_AS.Play ();
					}
				}

				if (Input.GetKeyUp (m_ListenKey)) {
					Debug.Log ("Finger Down");
					FingerDown ();

				}
			}

		}
	}

	public void SetActive(bool active){
		m_IsActive = active;
	}

	void FingerDown(){
		Vector3 pos = m_FingerH.position;
		pos.y -= m_MoveDist;
		m_FingerH.position = pos;
	}

	void FingerUp(){
		Vector3 pos = m_FingerH.position;
		pos.y += m_MoveDist;
		m_FingerH.position = pos;
	}
}

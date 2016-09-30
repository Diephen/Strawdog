using UnityEngine;
using System.Collections;

public class InteractionSound : MonoBehaviour {
	// Sound Clip reference
	// 0 -- dunk in 
	// 1 -- dunk out
	//
	[SerializeField] AudioClip[] m_sounds;
	[SerializeField] AudioSource m_audio;

	// Use this for initialization
	void Start () {
		m_audio = gameObject.GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void PlayDunkIn(){
		m_audio.clip = m_sounds [0];
		if (!m_audio.isPlaying) {
			m_audio.Play ();
		}

	}

	public void PlayDunkOut(){
		m_audio.clip = m_sounds [1];
		if (!m_audio.isPlaying) {
			m_audio.Play ();
		}

	}
}

﻿using UnityEngine;
using System.Collections;

public class InteractionSound : MonoBehaviour {
	// Sound Clip reference
	// 0 -- dunk in 
	// 1 -- dunk out
	// 2 -- drown
	//
	//
	// 5 --
	// 6 -- Faint Sound

	//Tutorial
	// 7 -- Happy Dog
	// 8 -- Panting Dog
	// 9 -- Begging Dog
	// 10 -- gun shot execution
	// 11 -- gun reload

	[SerializeField] AudioClip[] m_sounds;
	[SerializeField] AudioSource m_audio;
	[SerializeField] AudioSource m_audioDrown;

	// Use this for initialization
	void Start () {
		m_audio = gameObject.GetComponent<AudioSource> ();
		m_audioDrown = gameObject.AddComponent<AudioSource> ();
		m_audioDrown.loop = true;
		m_audioDrown.playOnAwake = false;
		m_audioDrown.clip = m_sounds [2];
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

	public void PlayDrown(){
		if (!m_audioDrown.isPlaying) {
			m_audioDrown.Play();
		}
	}

	public void StopDrown(){
		if (m_audioDrown.isPlaying) {
			m_audioDrown.Stop();
		}
	}

	public void PlayResist(){
		m_audio.clip = m_sounds [3];
		if (!m_audio.isPlaying) {
			m_audio.Play ();
		}
	}

	public void PlayBreakOut(){
		m_audio.clip = m_sounds [4];
		if (!m_audio.isPlaying) {
			m_audio.Play ();
		}
	}

	public void PlayChain(){
		m_audio.clip = m_sounds [5];
		if (!m_audio.isPlaying) {
			m_audio.Play ();
		}
	}

	public void PlayFaint(){
		m_audioDrown.Stop ();
		m_audio.clip = m_sounds [6];
		m_audio.Play ();
	}

	// Tutorial
	public void PlayHappy(){
		m_audio.clip = m_sounds [7];
		if (!m_audio.isPlaying) {
			m_audio.Play ();
		}
	}

	public void PlayPant(){
		m_audio.clip = m_sounds [8];
		m_audio.Play ();
		if (!m_audio.isPlaying) {
			m_audio.Play ();
		}
	}

	public void PlayBeg(){
		m_audio.clip = m_sounds [9];
		if (!m_audio.isPlaying) {
			m_audio.Play ();
		}
	}

	public void PlayGun(){
		m_audio.clip = m_sounds [10];
		if (!m_audio.isPlaying) {
			m_audio.Play ();
		}
	}

	public void PlayReload(){
		m_audio.clip = m_sounds [11];
		if (!m_audio.isPlaying) {
			m_audio.Play ();
		}
	}

	public void StopPlay(){
		if(m_audio.isPlaying){
			m_audio.Stop();
		}
	}


}

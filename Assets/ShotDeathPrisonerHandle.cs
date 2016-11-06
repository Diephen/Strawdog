using UnityEngine;
using System.Collections;

public class ShotDeathPrisonerHandle : MonoBehaviour {
	
	[SerializeField] Animator m_Anim;
	[SerializeField] BoxCollider2D m_StopCol;
	[SerializeField] SpriteRenderer[] m_Sprites;
	[SerializeField] Color m_EndColor;
	[SerializeField] Color m_StartColor;
	[SerializeField] float m_Duration;
	AudioSource m_Audio;
	Timer m_ColorTimer;
	BoxCollider2D m_TriggerCol;
	bool isExecuted = false;


	// Use this for initialization
	void Start () {
		m_TriggerCol = gameObject.GetComponent<BoxCollider2D> ();
		m_Sprites = gameObject.GetComponentsInChildren<SpriteRenderer> ();
		foreach (SpriteRenderer spr in m_Sprites) {
			spr.material.color = m_StartColor;
		}
		m_ColorTimer = new Timer (m_Duration);
		m_Audio = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (isExecuted) {
			TurnDark ();
		}
	
	}

	public void Executed(){
		if (!isExecuted) {
			isExecuted = true;
			m_Anim.Play ("SP-death");
			m_StopCol.enabled = false;
			m_TriggerCol.enabled = false;
			m_ColorTimer.Reset ();
		}
	}

	void TurnDark(){
		foreach (SpriteRenderer spr in m_Sprites) {
			spr.material.color = Color.Lerp (m_StartColor, m_EndColor, m_ColorTimer.PercentTimePassed);
		}
		//lerpedColor = Color.Lerp(Color.white, Color.black, Time.deltaTime);
	}

	public void PlayBodyFall(){
		if (!m_Audio.isPlaying) {
			m_Audio.Play ();
		}
	}
}

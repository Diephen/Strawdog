using UnityEngine;
using System.Collections;

public class ShotDeathPrisonerHandle : MonoBehaviour {
	[SerializeField] bool m_IsStartDead = false;
	[SerializeField] Animator m_Anim;
	[SerializeField] SpriteRenderer[] m_Sprites;
	[SerializeField] Color m_EndColor;
	[SerializeField] Color m_StartColor;
	[SerializeField] float m_Duration;
	Timer m_AnimTimer;
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
		m_AnimTimer = new Timer (m_Duration);
		m_AnimTimer.Reset ();
		RandomIdleAnimation ();

		if (m_IsStartDead) {
			m_Anim.Play ("SP-StartAsDead");
			m_TriggerCol.enabled = false;
			foreach (SpriteRenderer spr in m_Sprites) {
				spr.material.color = m_EndColor;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (isExecuted) {
			TurnDark ();
		} else {
			if (m_AnimTimer.IsOffCooldown) {
				RandomIdleAnimation ();
				RandomTimer ();
			}
		}

	
	}

	public void Executed(){
		if (!isExecuted) {
			isExecuted = true;
			m_Anim.Play ("SP-death");
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

	void RandomIdleAnimation(){
		int AnimIdx = Random.Range (0, 2);
		m_Anim.SetInteger ("IdleIdx", AnimIdx);
	}

	void RandomTimer(){
		m_AnimTimer.CooldownTime = Random.Range (0, 3);
		m_AnimTimer.Reset ();
	}
}

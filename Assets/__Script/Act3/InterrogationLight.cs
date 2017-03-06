using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterrogationLight : MonoBehaviour {
	bool m_IsOn;
	SpriteRenderer m_SpriteRenderer;
	Sprite m_OnSprite;
	Sprite m_OffSprite;


	void Awake(){
		m_OnSprite = Resources.Load<Sprite> ("Images/stub-g");
		m_OffSprite = Resources.Load<Sprite> ("Images/stub-r");
	}


	// Use this for initialization
	void Start () {
		m_SpriteRenderer = GetComponent<SpriteRenderer> ();
		m_SpriteRenderer.sprite = m_OffSprite;
		m_IsOn = false;

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void TurnLightOn(){
		if (!m_IsOn) {
			m_IsOn = true;
			m_SpriteRenderer.sprite = m_OnSprite;
		}
	}

	public void TurnLightOff(){
		if (m_IsOn) {
			m_IsOn = false;
			m_SpriteRenderer.sprite = m_OffSprite;
		}
	}
}

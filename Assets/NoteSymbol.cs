using UnityEngine;
using System.Collections;

public class NoteSymbol : MonoBehaviour {
	bool m_IsUnclock = false;
	[SerializeField] Color m_StartColor;
	[SerializeField] Color m_EndColor;
	SpriteRenderer m_Sprite;
	[SerializeField] Animator m_Anim;


	// Use this for initialization
	void Awake () {
		//gameObject.SetActive (false);
		m_Sprite = GetComponent<SpriteRenderer>();
		m_Sprite.material.color=m_StartColor;
		if (GetComponent<Animator> ()) {
			m_Anim = GetComponent<Animator> ();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
		

	public void ShowWithAlpha(Color m_AlphaColor){
		//gameObject.SetActive (true);
		//m_Sprite.color = m_StartColor;
		if(!m_IsUnclock){
			m_IsUnclock = true;
		}
		print ("Call showing animation");
		m_Sprite.material.color = m_AlphaColor;
		// move symbol to the right position 
	}

	public void SetUnclock(){
		m_IsUnclock = true;
	}

	public void ShowSymbolIdle(){
		if (m_IsUnclock) {
			print ("Call Idle Animtion");
			//gameObject.SetActive (true);
			m_Sprite.material.color = m_EndColor;
		}
	}

	public void Hide(){
		if (m_Sprite != null) {
			m_Sprite.material.color=m_StartColor;
		}

	}
}

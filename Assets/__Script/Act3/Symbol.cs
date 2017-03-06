using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Symbol : MonoBehaviour {
	
	public bool IsUnclock { set; get;}
	int m_SymbolIdx = -1;
	[SerializeField] Color m_StartColor;
	[SerializeField] Color m_EndColor;
	SpriteRenderer m_SpriteRenderer;
	[SerializeField] Animator m_Anim;


	// Use this for initialization
	void Awake () {
		//gameObject.SetActive (false);
		m_SpriteRenderer = GetComponent<SpriteRenderer>();
//		m_Sprite.material.color=m_StartColor;
//		if (GetComponent<Animator> ()) {
//			m_Anim = GetComponent<Animator> ();
//		}
	}

	// Update is called once per frame
	void Update () {

	}

	public void HitSymbol(){
		if (!IsUnclock) {
			IsUnclock = true;
			m_SpriteRenderer.color = new Color (0, 255, 0);
			print ("### Hit!!");
		}

	}

	public void MissSymbol(){
		if (!IsUnclock) {
			IsUnclock = false;
			m_SpriteRenderer.color = new Color (255, 0, 0);
			print ("### Miss!!");
		}

	}

	public bool SetSprite(Sprite spr, int idx){
		if (idx != m_SymbolIdx) {
			m_SpriteRenderer.sprite = spr;
			m_SpriteRenderer.color = new Color (255f, 255f, 255f, 255f);
			IsUnclock = false;
			m_SymbolIdx = idx;
			return true;
		} else {
			return false;
		}

	}

	public int GetSymbolIdx(){
		if (m_SymbolIdx >= 0 && m_SymbolIdx < 4) {
			return m_SymbolIdx;
		} else {
			return -1;
		}

	}
		




	// old code 
//	public void ShowWithAlpha(Color m_AlphaColor){
//		//gameObject.SetActive (true);
//		//m_Sprite.color = m_StartColor;
//		if(!IsUnclock){
//			IsUnclock = true;
//		}
//		print ("Call showing animation");
//		m_SpriteRenderer.material.color = m_AlphaColor;
//		// move symbol to the right position 
//	}
//
//
//
//	public void ShowSymbolIdle(){
//		if (IsUnclock) {
//			print ("Call Idle Animtion");
//			//gameObject.SetActive (true);
//			m_SpriteRenderer.material.color = m_EndColor;
//		}
//	}
//
//	public void Hide(){
//		if (m_SpriteRenderer != null) {
//			m_SpriteRenderer.material.color=m_StartColor;
//		}
//
//	}
}

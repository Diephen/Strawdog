using UnityEngine;
using System.Collections;

public class HighlightSprite : MonoBehaviour {
	[SerializeField] GameObject m_Hi;

	// Use this for initialization
	void Awake () {
		m_Hi.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void EnableHighlight(){
		if (m_Hi != null) {
			m_Hi.SetActive (true);
		}

	}

	public void DisableHighlight(){
		if (m_Hi != null) {
			m_Hi.SetActive (false);
		}
	}
}

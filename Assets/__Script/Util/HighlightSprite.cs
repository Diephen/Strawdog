using UnityEngine;
using System.Collections;

public class HighlightSprite : MonoBehaviour {
	[SerializeField] GameObject m_Hi;
	[SerializeField] GameObject[] m_DesText;
	[SerializeField] bool _isDoor = false;
	bool _isDoorClosed = true;
	bool m_IsFlicker = false;
	SpriteRenderer m_Sprite;

	Color m_StartColor = Color.white;
	Color m_EndColor = Color.white;

	void OnEnable(){
		Events.G.AddListener<OfficeDoorEvent> (ChangeTextHandle);

	}

	void OnDisable(){
		Events.G.RemoveListener<OfficeDoorEvent> (ChangeTextHandle);
	}

	// Use this for initialization
	void Awake () {
		m_Hi.SetActive (false);
		m_Sprite = m_Hi.GetComponent<SpriteRenderer> ();
		m_EndColor.a = 0;
		if (m_DesText != null) {
			foreach (GameObject g in m_DesText) {
				g.SetActive (false);
			}

		}
	}
	
	// Update is called once per frame
	void Update () {
		if (m_IsFlicker) {
			m_Sprite.material.color = Color.Lerp (m_StartColor, m_EndColor, Mathf.PingPong (Time.time, 1));
		} else {
			m_Sprite.material.color = m_StartColor;
		}
		
	}

	public void EnableHighlight(){
		if (m_Hi != null) {
			m_Hi.SetActive (true);
		}
		print ("show text");
		ShowText ();

	}

	public void DisableHighlight(){
		if (m_Hi != null) {
			m_Hi.SetActive (false);
			m_IsFlicker = false;
			HideText ();
		}
	}

	public void EnableHignlightFlicker(){
		EnableHighlight ();
		m_IsFlicker = true;

	}

	public void DisableFlicker(){
		//EnableHighlight ();
		m_IsFlicker = false;
	}

	void ShowText(){
		print ("show text");
		if (m_DesText != null) {
			print ("show text");
			if (_isDoor) {
				// open
				m_DesText [0].SetActive (true);
				m_DesText [1].SetActive (true);
			} else {
				m_DesText [0].SetActive (true);
			}
		}
	}

	void HideText(){
		if (m_DesText != null) {
			foreach (GameObject g in m_DesText) {
				g.SetActive (false);
			}
		}
	}

	void ChangeTextHandle(OfficeDoorEvent e){
		if (_isDoor && m_DesText != null) {
			//_isDoorClosed = !_isDoorClosed;
			if (e.Opened) {
				m_DesText [2].SetActive (true);
				m_DesText [1].SetActive (false);
			} else {
				m_DesText [1].SetActive (true);
				m_DesText [2].SetActive (false);
			}
		}
		
	}



}

using UnityEngine;
using System.Collections;

public class HighlightSprite : MonoBehaviour {
	[SerializeField] GameObject m_Hi;
	[SerializeField] GameObject[] m_DesText;
	[SerializeField] bool _isDoor = false;
	[SerializeField] bool _isSwitchingCharacter = false;
	[SerializeField] bool _isPrisoner = false;
	bool _isDoorClosed = true;
	bool m_IsFlicker = false;
	SpriteRenderer m_Sprite;

	Color m_StartColor = Color.white;
	Color m_EndColor = Color.white;

	void OnEnable(){
		Events.G.AddListener<OfficeDoorEvent> (ChangeTextHandle);
		Events.G.AddListener<LockCellEvent> (ChangeTextCellHandle);
		Events.G.AddListener<DisableMoveEvent> (ChangeCharacterHandle);
	}

	void OnDisable(){
		Events.G.RemoveListener<OfficeDoorEvent> (ChangeTextHandle);
		Events.G.RemoveListener<LockCellEvent> (ChangeTextCellHandle);
		Events.G.RemoveListener<DisableMoveEvent> (ChangeCharacterHandle);
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
		//print ("show text");
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
		//print ("show text");
		if (m_DesText.Length>=1) {
			//print ("show text");
			if (_isDoor && !_isSwitchingCharacter) {
				// open
				//print("Door open: " + !_isDoorClosed);
				if (_isDoorClosed) {
					m_DesText [0].SetActive (true);
					m_DesText [1].SetActive (true);
				} else {
					m_DesText [0].SetActive (true);
					m_DesText [2].SetActive (true);
				}

			} else if (!_isDoor && !_isSwitchingCharacter) {
				m_DesText [0].SetActive (true);
			} else if(!_isDoor && _isSwitchingCharacter){
				// starts with the first one 
				if (!_isPrisoner) {
					m_DesText [0].SetActive (true);
				} else {
					m_DesText [1].SetActive (true);
				}

				
			}
		}
	}

	void HideText(){
		if (m_DesText.Length>=1) {
			foreach (GameObject g in m_DesText) {
				g.SetActive (false);
			}
		}
	}

	void ChangeTextHandle(OfficeDoorEvent e){
		if (_isDoor && m_DesText != null) {
			_isDoorClosed = !e.Opened;
			if (e.Opened) {
				//show close text 
				m_DesText [2].SetActive (true);
				m_DesText [1].SetActive (false);
			} else {
				m_DesText [1].SetActive (true);
				m_DesText [2].SetActive (false);
			}
		}
		
	}

	void ChangeTextCellHandle(LockCellEvent e){
		if (_isDoor && m_DesText != null) {
			_isDoorClosed = e.Locked;
			if (!e.Locked) {
				m_DesText [2].SetActive (true);
				m_DesText [1].SetActive (false);
			} else {
				m_DesText [1].SetActive (true);
				m_DesText [2].SetActive (false);
			}
		}
	}

	void ChangeCharacterHandle(DisableMoveEvent e){
		if (_isSwitchingCharacter && e.WhoAmI == CharacterIdentity.Guard) {
			//print ("## switch to prisoner");
			_isPrisoner = !_isPrisoner;
		}
	}



}

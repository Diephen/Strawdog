using UnityEngine;
using System.Collections;

public class InterrogationPrisonerHandler : MonoBehaviour {
	// no animation injection for this one -- use puppet control 
	[SerializeField] Animator m_Anim;
	//[SerializeField] GuardHandle m_GuardHandle;
	[SerializeField] AnimationControl m_AnimCtrl;
	[SerializeField] InteractionSound m_ItrAudio;
	[SerializeField] NoteSymbol[] m_Notes;            // the content of the note
	[SerializeField] HighlightSprite m_HiNote;        // Note hightlight 
	[SerializeField] float m_NoteDuration;            // how long to read the note
	[SerializeField] Color m_StartColor;
	[SerializeField] Color m_EndColor;
	[SerializeField] InteractionProgress m_IntrProgress;

	int m_UnClockSymbolCount = -1;
	bool m_IsHoldDown = false;
	bool m_IsBombFound = false;

	// note reading 
	bool m_IsReadingNote = false;
	float m_ReadTime = 0f;
	//float m_StartTime = 10000000f;
	Timer m_NoteTimer;


	void Awake() {
		if (m_Anim == null) {
			m_Anim = GetComponent<Animator> ();
		}

		//m_AnimCtrl.SetAnimation (true);
		m_NoteTimer = new Timer(m_NoteDuration);
		//m_HiNote.EnableHighlight ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!m_IsBombFound) {
			if (Input.GetKeyDown (KeyCode.S)) {
				if (!m_IsReadingNote && !m_IsHoldDown) {
					StartCoroutine(m_IntrProgress.FadeIn (1.0f));
					m_IsReadingNote = true;
					NoteReading ();
					m_ItrAudio.PlayRead ();
				}
			}

			if (Input.GetKeyUp (KeyCode.S)) {
				if (m_IsReadingNote && !m_IsHoldDown) {
					m_IsReadingNote = false;
					StopReading ();
				}
			}

			if (!m_IsHoldDown && m_IsReadingNote) {
				CheckNotes ();
			} else {
				m_HiNote.DisableFlicker ();
				StartCoroutine(m_IntrProgress.FadeOut (0.5f));
				m_ItrAudio.StopPlayDrown ();
			}
		}
	}

	public void SetBombState(bool isfound){
		m_IsBombFound = isfound;
	}

	public void ForceToRead(){
		m_AnimCtrl.SetAnimation (true);
//		if(m_Anim.isActiveAndEnabled){
		m_Anim.Play ("IP-ForceRead");
//		}
		m_IsHoldDown = true;
		m_HiNote.DisableHighlight();
		StopReading ();
	}

	public void BackToIdle(){
		m_Anim.Play ("IP-BackToIdle");

	}

	public void EndScene(){
		m_AnimCtrl.SetAnimation (true);
		//		if(m_Anim.isActiveAndEnabled){
		m_Anim.Play ("IP-Fall");
		//		}
		m_IsReadingNote = false;
		m_IsHoldDown = false;
		m_HiNote.DisableHighlight();
		StopReading ();
		m_ItrAudio.PlayFallOffChair ();
	}
		
	void NoteReading(){
		// show the previously unclocked symbols 
		for(int i=0; i<m_Notes.Length; i++){
			if (i <= m_UnClockSymbolCount) {
				print ("Show symbol No." + i);
				m_Notes [i].ShowSymbolIdle ();
			}
		}
		// counting time to unclock more symbols
		m_IsReadingNote = true;
		// start timer when start reading
		m_NoteTimer.Reset();
	}

	void EnableReading(){
		m_IsHoldDown = false;
		m_HiNote.EnableHighlight ();
	}

	// tracking the time of note unclocking 
	void CheckNotes(){
		m_HiNote.EnableHignlightFlicker ();
		if (m_UnClockSymbolCount < m_Notes.Length - 1) {
			// fade in the current note
			int TempCnt = -1;
			if (m_UnClockSymbolCount + 1 <= m_Notes.Length - 1) {
				TempCnt = m_UnClockSymbolCount + 1;
			}
			if (!m_NoteTimer.IsOffCooldown) {
				
				m_Notes [TempCnt].ShowWithAlpha (Color.Lerp (m_StartColor, m_EndColor, m_NoteTimer.PercentTimePassed));
			} else {
				m_UnClockSymbolCount += 1;
				m_Notes [m_UnClockSymbolCount].ShowSymbolIdle ();
				m_NoteTimer.Reset ();
			}

//			if (m_NoteTimer.IsOffCooldown) {
//				print ("Unclock new");
//				if (m_UnClockSymbolCount + 1 <= m_Notes.Length - 1) {
//					m_UnClockSymbolCount += 1;
//					//m_Notes [m_UnClockSymbolCount].GetComponent<NoteSymbol> ().isUnclock = true;
//					m_Notes [m_UnClockSymbolCount].GetComponent<NoteSymbol> ().Show ();
//					m_NoteTimer.Reset ();
//				}
//
//			}
		} else {
			m_NoteTimer.Reset();
		}
		m_IntrProgress.IncTime ((m_UnClockSymbolCount+1) * m_NoteTimer.CooldownTime + m_NoteTimer.TimePassed);
	}

	public void StopReading(){
		m_IsReadingNote = false;
		for(int i=0; i<m_Notes.Length; i++){
			m_Notes [i].Hide ();
		}
	}


	void DisAbleAnim(){
		m_AnimCtrl.SetAnimation (false);
		EnableReading ();
	}
}

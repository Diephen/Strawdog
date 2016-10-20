using UnityEngine;
using System.Collections;

// Test to use Dave Timer 
// add constraints to the key input 

public class KeyComboHandler : MonoBehaviour {
	private float m_CoolDownTime;
	private KeyCode[] m_KeyCombo;
	private Timer m_KeyTimer;
	private bool m_IsKeyComboPressed;

	public KeyComboHandler(float _listenTime, KeyCode[] _keyCombo){
		m_CoolDownTime = _listenTime;
		if (_keyCombo.Length == 3) {
			m_KeyCombo = new KeyCode[_keyCombo.Length];
			//return null;
		} else {
			Debug.Log ("ERROR Key Combo exceed size!!");
		}
	}

	public void SetKeyCombo(float _listenTime, KeyCode[] _keyCombo) { 
		m_CoolDownTime = _listenTime;
		if (_keyCombo.Length == 3) {
			m_KeyCombo = new KeyCode[_keyCombo.Length];
			m_KeyCombo = _keyCombo;
			//return null;
		} else {
			Debug.Log ("ERROR Key Combo exceed size!!");
		}
	}
	public bool IsPressCombo { get { return m_IsKeyComboPressed; } }


	// Use this for initialization
	void Awake () {
		m_KeyTimer = new Timer (m_CoolDownTime);
		m_IsKeyComboPressed = false;
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log ("Update?? " + m_KeyCombo[0].ToString());
		// check input ASD
		if(Input.GetKey(m_KeyCombo[0])){
			// start timer 
			if (m_KeyTimer.IsOffCooldown) {
				if ( Input.GetKey (m_KeyCombo[1]) || Input.GetKey (m_KeyCombo[2])) {
//					Debug.Log ("More Than one key");
					m_IsKeyComboPressed = true;
				} else {
//					Debug.Log("Single Key");
					m_IsKeyComboPressed = false;
				}
				m_KeyTimer.Reset ();
			} 
		}

		if(Input.GetKey(m_KeyCombo[1])){
			// start timer 
			if (m_KeyTimer.IsOffCooldown) {
				//Debug.Log ("End of timer");
				if ( Input.GetKey (m_KeyCombo[0]) || Input.GetKey (m_KeyCombo[2])) {
					//Debug.Log ("More Than one key");
					m_IsKeyComboPressed = true;
				} else {
					//Debug.Log("Single Key");
					m_IsKeyComboPressed = false;
				}
				m_KeyTimer.Reset ();
			} 
		}

		if(Input.GetKey(m_KeyCombo[2])){
			// start timer 
			if (m_KeyTimer.IsOffCooldown) {
				//Debug.Log ("End of timer");
				if ( Input.GetKey (m_KeyCombo[0]) || Input.GetKey (m_KeyCombo[1])) {
					//Debug.Log ("More Than one key");
					m_IsKeyComboPressed = true;
				} else {
					//Debug.Log("Single Key");
					m_IsKeyComboPressed = false;
				}
				m_KeyTimer.Reset ();
			} 
		}
	}


}

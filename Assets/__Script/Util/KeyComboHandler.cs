using UnityEngine;
using System.Collections;

// Test to use Dave Timer 
// add constraints to the key input 

public class KeyComboHandler : MonoBehaviour {
	[SerializeField] float m_CoolDownTime;
	private float m_StartListenTime = -1f;
	private float m_Interval;
	private KeyCode m_PrimeKey;

	private Timer m_KeyTimer;

	private bool m_IsKeyComboPressed;

	// Use this for initialization
	void Awake () {
		m_KeyTimer = new Timer (m_CoolDownTime);
	
	}
	
	// Update is called once per frame
	void Update () {
		//if(m_PrimeKey)

		// check input ASD
		if(Input.GetKey(KeyCode.A)){
			// start timer 
			if (m_KeyTimer.IsOffCooldown) {
				Debug.Log ("End of timer");
				if ( Input.GetKey (KeyCode.S) || Input.GetKey (KeyCode.D)) {
					Debug.Log ("More Than one key");
				} else {
					Debug.Log("Single Key");
				}
				m_KeyTimer.Reset ();
			} else {
				Debug.Log ("Still count");
			}
		}
	}


}

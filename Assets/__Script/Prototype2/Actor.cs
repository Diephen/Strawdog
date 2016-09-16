using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Actor : MonoBehaviour {
	public int m_Index;
	[SerializeField] Material m_DefaultColor;
	[SerializeField] Material m_ActivateColor;
	[SerializeField] Renderer m_Renderer;
	[SerializeField] TheatreManager m_TM;           // the theatre manager 
	[SerializeField] Text m_Sub;
	[SerializeField] string[] m_DisplayText;
	[SerializeField] bool m_IsActive = false;
	[SerializeField] Tile m_BckTile;
	[SerializeField] Finger[] m_Fingers; 
	[SerializeField] bool moveleft = false;
	[SerializeField] bool move = false;

  
	// Use this for initialization
	void Awake () {
		
		m_Renderer.material = m_DefaultColor;
	}

	void Start () {
//		m_TM.RegisterActors (gameObject, m_Index);
	}
	
	// Update is called once per frame
	void Update () {
		if (m_IsActive) {
			if (Input.GetKey (KeyCode.A)) {
				if (!move) {
					m_BckTile.MoveLeft ();
				}
//				Debug.Log ("DisPlay" + m_DisplayText [1]);
//				m_Sub.text = m_DisplayText [1];
				//m_BckTile.MoveLeft ();
				moveleft = true;
			}

			if (Input.GetKey (KeyCode.D)) {
				if (!move) {
					m_BckTile.MoveRight ();
				}
//				m_Sub.text = m_DisplayText [2];
				//m_BckTile.MoveRight ();
				moveleft = false;
			}


		
		}




	
	}

	public void Activate(){
		
		m_Renderer.material = m_ActivateColor;
		m_Sub.text = m_DisplayText[0];
		m_IsActive = true;
		foreach (Finger fg in m_Fingers) {
			fg.SetActive (true);
		}
	}

	public void DeActivate(){
		move = false;
		m_Renderer.material = m_DefaultColor;
		m_IsActive = false;
		foreach (Finger fg in m_Fingers) {
			fg.SetActive (false);
		}
	}




}

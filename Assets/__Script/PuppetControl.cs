using UnityEngine;
using System.Collections;

public class PuppetControl : MonoBehaviour {
	[SerializeField] GameObject[] m_Finger = new GameObject[3];
	[SerializeField] KeyCode[] m_ListenKey = new KeyCode[] {KeyCode.A, KeyCode.S, KeyCode.D};
	[SerializeField] float[] m_MoveDistance = new float[3];
	[SerializeField] float[] m_CrouchMoveDistance = new float[3];
	[SerializeField] bool[] m_Continuous = new bool[3];

	[SerializeField] float m_MoveSpeed = 2.0f;

	enum charState {idle, left, right, crouch};
	charState m_charState = charState.idle;
	bool dog = false;
	bool cat = false;
	Vector3 oldPos;
	Vector3 newPos;
	float startTime;
	// Use this for initialization
	void Start (){
		oldPos = m_Finger [2].transform.localPosition;
		newPos = oldPos;
		newPos.y -= m_MoveDistance [2];
		startTime = -1f;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown (m_ListenKey[0]) && 
		   Input.GetKeyDown (m_ListenKey[1]) && 
		   Input.GetKeyDown (m_ListenKey[2])){
			m_charState = charState.crouch;
			Crouch ();
		}
			
		//A-Key
		if (m_Continuous [0] && Input.GetKey (m_ListenKey [0]) 
			&& (m_charState == charState.idle || m_charState == charState.left)) {
			m_charState = charState.left;
			Walk (m_charState);
		}
		if (Input.GetKeyDown (m_ListenKey [0])) {
			FingerDown (0);
		} else if (Input.GetKeyUp (m_ListenKey [0])) {
			FingerUp (0);
		}

		//S-Key
		if (Input.GetKeyDown (m_ListenKey [1])) {
			FingerDown (1);
		} else if (Input.GetKeyUp (m_ListenKey [1])) {
			FingerUp (1);
		}

		// D-Key
		if (m_Continuous [2] && Input.GetKey (m_ListenKey [2])
			&& (m_charState == charState.idle || m_charState == charState.right)) {
			m_charState = charState.right;
			Walk (m_charState);
		}
		if (Input.GetKeyDown (m_ListenKey [2])) {
//			StartCoroutine(FingerDown (2));
//			startTime = Time.time;
//			dog = true;
//			cat = false;

		} else if (Input.GetKeyUp (m_ListenKey [2])) {
			FingerUp (2);
//			dog = false;
//			cat = true;
//			startTime = Time.time;
		}

//		if (dog) {
//			float distCovered = (Time.time - startTime) * 5f;
//			
//			m_Finger [2].transform.localPosition = Vector3.Lerp (m_Finger [2].transform.localPosition, newPos, distCovered);
//		}
//		if (cat) {
//			float distCovered = (Time.time - startTime) * 5f;
//
//			m_Finger [2].transform.localPosition = Vector3.Lerp (m_Finger [2].transform.localPosition, oldPos, distCovered);
//		}
	}

	void Crouch(){
	}

	void FingerUp(int index){
//		m_charState = charState.idle;
//		oldPos = m_Finger[2].transform.position;
//		newPos = oldPos;
//		newPos.y += m_MoveDistance [2];
//		m_Finger[2].transform.position = Vector2.Lerp(oldPos, newPos, Time.deltaTime);
	}

	void FingerDown(int index){
//		Vector2 pos = m_Finger [index].transform.position;
//
//		Vector3 pos = m_FingerH.position;
//		pos.y -= m_MoveDist;
//		m_FingerH.position = pos;
	}

	void Walk(charState dir){
		if (dir == charState.left) {
			transform.Translate (Vector2.left * m_MoveSpeed * Time.deltaTime);
		} else if (dir == charState.right) {
			transform.Translate (Vector2.right * m_MoveSpeed * Time.deltaTime);
		}
	}
}

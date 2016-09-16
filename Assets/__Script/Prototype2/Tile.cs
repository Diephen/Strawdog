using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {
	[SerializeField] float m_offsetX;
	[SerializeField] float m_width;
	[SerializeField] Vector3 m_Original;
	[SerializeField] float m_ScreenWidth;
	[SerializeField] float m_MoveSpeed;
	//[SerializeField] GameObject m_BckPrefab;
	[SerializeField] bool m_IsMovingRight;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void CheckBound(){
		if (m_IsMovingRight) {
			// if x + width/2 < screen left , move tile to left 2 offset 

		}
		
	}

	public void MoveLeft(){
		m_IsMovingRight = false;
		Vector3 npos = gameObject.transform.position;
		npos.x += m_MoveSpeed * Time.deltaTime;
		gameObject.transform.position = npos;
	}

	public void MoveRight(){
		m_IsMovingRight = true;
		Vector3 npos = gameObject.transform.position;
		npos.x -= m_MoveSpeed * Time.deltaTime;
		gameObject.transform.position = npos;
	}
}

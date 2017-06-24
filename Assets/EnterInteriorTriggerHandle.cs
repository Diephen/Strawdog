using UnityEngine;
using System.Collections;

public class EnterInteriorTriggerHandle : MonoBehaviour {
	[SerializeField] SpriteRenderer m_OutsideWall;
	[SerializeField] float m_FadeOutDistance;             // when the wall fully fades out 
	Transform m_PlayerPos;
	float m_FadeSpeed = 0.8f;
	Color m_StartColor;
	Color m_EndColor;
	bool m_IsWallFade = false;

	// track player position 

	// Use this for initialization
	void Start () {
		//m_PlayerPos = GameObject.FindObjectOfType<GuardHandle> ().transform.position;
		m_StartColor = m_OutsideWall.color;
		m_EndColor = m_StartColor;
		m_EndColor.a = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (m_PlayerPos != null) {
//			Debug.Log ("[Test]" + m_PlayerPos.position);
		}
	}


	void OnTriggerEnter2D(Collider2D other) {
		//Debug.Log ("Attention");
		if(other.name == "GuardStructure"){
			m_PlayerPos = other.gameObject.transform;
			if (!m_IsWallFade) {
				m_IsWallFade = true;
				StartCoroutine (FadeOut ());
			}
		}

	}

	void FadeInterior(){
		
	}


	IEnumerator FadeOut()
	{
		yield return new WaitForSeconds (1);
		while (m_OutsideWall.color.a > m_EndColor.a) {
			m_OutsideWall.color = Color.Lerp(m_OutsideWall.color,m_EndColor,Time.deltaTime * m_FadeSpeed);
			yield return null;
		}

	}
		

	void SetAlpha(){
		
	}

}

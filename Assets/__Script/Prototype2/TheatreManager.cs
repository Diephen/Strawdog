using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class TheatreManager : MonoBehaviour {
	//[SerializeField] private List<GameObject> m_Actors;
	[SerializeField] private GameObject[] m_Actors;
	[SerializeField] private int m_MAXNUM;

	// the light in the scene 
	[SerializeField] GameObject m_SpotLight;        // the spotlight in the scene 
	private Transform m_SpotLightTransform;
	bool m_StartGame = false;
	[SerializeField] GameObject m_Title;
	[SerializeField] AudioSource m_AS;

	// 
	[SerializeField] int m_CurIndex;

	// Use this for initialization
	void Awake(){
		//m_Actors = new List<GameObject>();
		m_Actors = new GameObject[m_MAXNUM];
		m_CurIndex = 0;
		m_SpotLightTransform = m_SpotLight.transform;
		//MoveLightTo (m_Actors [m_CurIndex].transform.position);	
	}

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		// add the moving control of the light 
		if(Input.anyKeyDown && !m_StartGame){

			m_StartGame = true;
			m_Title.SetActive (false);
			m_Actors [m_CurIndex].GetComponent<Actor>().Activate ();
			if (!m_AS.isPlaying) {
				m_AS.Play ();
			}

		}
		if(Input.GetKeyDown(KeyCode.Space)){
			
			m_Actors [m_CurIndex].GetComponent<Actor>().DeActivate();
			if (m_CurIndex == 0) {
				m_CurIndex = 1;
			} else {
				m_CurIndex = 0;
			}
			MoveLightTo (m_Actors [m_CurIndex].transform.position);
			m_Actors [m_CurIndex].GetComponent<Actor>().Activate ();
			if (!m_AS.isPlaying) {
				m_AS.Play ();
			}
		}
		/*
		if(Input.GetKeyDown(KeyCode.LeftArrow)){
			// 
			m_Actors [m_CurIndex].GetComponent<Actor>().DeActivate();
			Debug.Log("Move Left");
			if (m_CurIndex == 0) {
				m_CurIndex = m_MAXNUM-1;
			} else {
				m_CurIndex -= 1;
			}
			MoveLightTo (m_Actors [m_CurIndex].transform.position);
			m_Actors [m_CurIndex].GetComponent<Actor>().Activate ();

		}

		if(Input.GetKeyDown(KeyCode.RightArrow)){
			// 
			Debug.Log("Move Right");
			m_Actors [m_CurIndex].GetComponent<Actor>().DeActivate();
			if (m_CurIndex == m_MAXNUM - 1) {
				m_CurIndex = 0;
			} else {
				m_CurIndex += 1;
			}
			MoveLightTo (m_Actors [m_CurIndex].transform.position);
			m_Actors [m_CurIndex].GetComponent<Actor>().Activate();
		}
		*/ 
		if (Input.GetKeyDown (KeyCode.R)) {
			SceneManager.LoadScene ("ManWolf");
		}
	}

	void MoveLightTo(Vector3 position){
		Vector3 pos = m_SpotLightTransform.position;
		pos.x = position.x;
		m_SpotLightTransform.position = pos;

	}

	public void RegisterActors(GameObject actor, int index){
		m_Actors [index] = actor;
	}
}

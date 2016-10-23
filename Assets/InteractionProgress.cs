using UnityEngine;
using System.Collections;

public class InteractionProgress : MonoBehaviour {
	[SerializeField] Renderer m_Progress;
	[SerializeField] float m_TimeToComplete;
	[SerializeField] float m_TimePassed; 

	bool isComplete = false;

	//public int timeToComplete = 3;
	
	// Update is called once per frame
	void Update () {
		if (!isComplete) {
			UpdateBar (m_TimePassed);
		} else {
			UpdateBar (m_TimeToComplete);
		}
	
	}

	// Use this for initialization
	void Start () {
		m_TimePassed = 0f;
		m_Progress.material.SetFloat ("_Progress", 0f);
		//Use this to Start progress
		//StartCoroutine(RadialProgress(3));
	}

	void UpdateBar(float time){
		float p = time / m_TimeToComplete;
		m_Progress.material.SetFloat ("_Progress", p);
	}
		
	public void IncTime(float time){
		m_TimePassed = time;
		if (m_TimePassed < m_TimeToComplete) {
			
		} else {
			isComplete = true;
		}
	}


	IEnumerator RadialProgress(float time)
	{
		float rate = 1 / time;
		float i = 0;
		while (i < 1)
		{
			i += Time.deltaTime * rate;
			m_Progress.material.SetFloat("_Progress", i);
			yield return 0;
		}
	}

	public IEnumerator FadeIn(float fadeintime){
		float i = 0;
		while (i < fadeintime)
		{
			i += Time.deltaTime;
			yield return 0;
		}
	}
}

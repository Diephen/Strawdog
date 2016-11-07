using UnityEngine;
using System.Collections;

public class InteractionProgress : MonoBehaviour {
	[SerializeField] Renderer m_Progress;
	[SerializeField] float m_TimeToComplete;
	[SerializeField] float m_TimePassed; 

	[SerializeField] Renderer m_Bck;


	bool isComplete = false;
	bool isFadeOut = false;
	//int m_FadeOption = 0;     // 1 fade in || 2 fade out 
	bool isFadingIn = false;
	bool isFadingOut = false;
	Color bckColor;
	//public int timeToComplete = 3;
	
	// Update is called once per frame
	void Update () {
		if (!isComplete) {
			UpdateBar (m_TimePassed);
		} else {
			
			if (!isFadeOut) {
				UpdateBar (m_TimeToComplete);
				isFadeOut = true;
				print ("FadeOut");
				StartCoroutine (FadeOut (4f));
			}
		}
	
	}

	// Use this for initialization
	void Start () {
		m_TimePassed = 0f;
		m_Progress.material.SetFloat ("_Progress", 0f);
		//Use this to Start progress
		//StartCoroutine(RadialProgress(3));
		m_Progress.material.SetFloat("_AlphaValue", 0f);
		bckColor = m_Bck.material.color;
		bckColor.a = 0;
		m_Bck.material.SetColor ("_Color", bckColor);
	}

	void UpdateBar(float time){
		float p = time / m_TimeToComplete;
		m_Progress.material.SetFloat ("_Progress", p);
	}
		
	public void IncTime(float time){
		m_TimePassed = time;
		if (m_TimePassed >= m_TimeToComplete) {
			isComplete = true;
			print ("complete");	
		} else {
			
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
		isFadeOut = false;
		isFadingIn = true;
		float i = 0;
		float rate = 1 / fadeintime;
		while (i < 1 && isFadingIn)
		{
			i += Time.deltaTime * rate;
			m_Progress.material.SetFloat ("_AlphaValue", i);
			bckColor.a = i;
			m_Bck.material.SetColor ("_Color", bckColor);
			yield return 0;
		}
		m_Progress.material.SetFloat ("_AlphaValue", 1);
		bckColor.a = 1;
		m_Bck.material.SetColor ("_Color", bckColor);
	}

	public IEnumerator FadeOut(float fadeintime){
		isFadeOut = true;
		isFadingIn =false;
		float i = 1;
		float rate = 1 / fadeintime;
		while (i > 0 && isFadeOut)
		{
			i -= Time.deltaTime * rate;
			m_Progress.material.SetFloat ("_AlphaValue", i);
			bckColor.a = i;
			m_Bck.material.SetColor ("_Color", bckColor);
			yield return 0;
//			if (isFadeOut) {
//				i -= Time.deltaTime * rate;
//				m_Progress.material.SetFloat ("_AlphaValue", i);
//				bckColor.a = i;
//				m_Bck.material.SetColor ("_Color", bckColor);
//				yield return 0;
//			} else {
//				m_Progress.material.SetFloat ("_AlphaValue", 0);
//				bckColor.a = 0;
//				m_Bck.material.SetColor ("_Color", bckColor);
//				break;
//			}
				
		}
		m_Progress.material.SetFloat ("_AlphaValue", 0);
		bckColor.a = 0;
		m_Bck.material.SetColor ("_Color", bckColor);
		//break;

	}
}

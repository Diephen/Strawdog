using UnityEngine;
using System.Collections;

public class InteractionProgress : MonoBehaviour {
	[SerializeField] Renderer m_Progress;
	[SerializeField] float m_TimeToComplete;
	[SerializeField] float m_TimePassed; 

	[SerializeField] Renderer m_Bck;
	[SerializeField] bool m_IsShowFromStart = false;

	Animator m_UIAnim;


	bool isComplete = false;
	bool isFadeOut = false;
	//int m_FadeOption = 0;     // 1 fade in || 2 fade out 
	bool isFadingIn = false;
	bool isFadingOut = false;
	Color bckColor;
	//public int timeToComplete = 3;

	// Use this for initialization
	void Start () {
		m_TimePassed = 0f;
		m_Progress.material.SetFloat ("_Progress", 0f);
		m_UIAnim = GetComponent<Animator> ();
		if (m_IsShowFromStart) {
			m_Progress.material.SetFloat("_AlphaValue", 1f);
			bckColor = m_Bck.material.color;
			bckColor.a = 1;
			m_Bck.material.SetColor ("_Color", bckColor);

		} else {
			//Use this to Start progress
			//StartCoroutine(RadialProgress(3));
			m_Progress.material.SetFloat("_AlphaValue", 0f);
			bckColor = m_Bck.material.color;
			bckColor.a = 0;
			m_Bck.material.SetColor ("_Color", bckColor);

		}
	}

	void OnEnable(){
		Events.G.AddListener<UIProgressBar> (OnUITrigger);
	}

	void OnDisable(){
		Events.G.RemoveListener<UIProgressBar> (OnUITrigger);
	}


	// Update is called once per frame
	void Update () {
		if (!isComplete) {
			UpdateBar (m_TimePassed);
		} else {
			
			if (!isFadeOut) {
				UpdateBar (m_TimeToComplete);
				isFadeOut = true;
				print ("FadeOut");
				//StartCoroutine (FadeOut (4f));
				m_UIAnim.SetBool("IsDrop", false);
			}
		}
	
	}


	void UpdateBar(float time){
		float p = time / m_TimeToComplete;
		m_Progress.material.SetFloat ("_Progress", p);
	}
		
	public void IncTime(float time){
		if (time <= 0.05f && time > 0f) {
			time = 0.08f;
		} else {
			m_TimePassed = time;
		}

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
		m_Progress.material.SetFloat ("_AlphaValue", 1f);
		bckColor.a = 1f;
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
		bckColor.a = 0f	;
		m_Bck.material.SetColor ("_Color", bckColor);
		//break;

	}

	void OnUITrigger(UIProgressBar e){
		if (e.IsDrop) {
			ShowUI ();
		} else {
			HideUI ();
		}
		
	}

	void ShowUI(){
		//m_UIAnim.Play ("PBar-Drop");
		m_UIAnim.SetBool("IsDrop", true);
	}

	void HideUI(){
		//m_UIAnim.Play ("PBar-PullBack");
		m_UIAnim.SetBool("IsDrop", false);
	}
}

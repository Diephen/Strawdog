using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LogoManager : MonoBehaviour {
	[SerializeField] Fading _fade;
	Timer _fadeTimer;
	// Use this for initialization
	void Start () {
		_fadeTimer = new Timer (3.0f);
		_fadeTimer.Reset ();
	}
	
	// Update is called once per frame
	void Update () {
		if (_fadeTimer.IsOffCooldown || Input.anyKeyDown) {
			StartCoroutine(ChangeLevel());
		}
	}


	IEnumerator ChangeLevel(){
		yield return new WaitForSeconds(0.5f);
		float fadeTime = _fade.BeginFade (1);
		yield return new WaitForSeconds(fadeTime);
		SceneManager.LoadScene ("TriggerWarning");
	}
}

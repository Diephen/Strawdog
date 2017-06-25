using UnityEngine;
using System.Collections;

public class WallFade : MonoBehaviour {
	Renderer _renderer;
	float _tempAlpha;
	Color _tempColor;
	Color _emptyColor;

	[SerializeField] bool _isSeeThroughWall = false;

	void Start(){
		_renderer = gameObject.GetComponent <SpriteRenderer> ();
		_tempColor = _renderer.material.color;
		_tempAlpha = _tempColor.a;
		_emptyColor = Color.white;
		_emptyColor.a = 0.0f;
	}

	void OnEnable ()
	{
		Events.G.AddListener<LeftCellUnlockedEvent>(LeftCellUnlocked);
		Events.G.AddListener<LockCellEvent>(CloseDoor);
		Events.G.AddListener<OfficeDoorEvent>(CloseOffice);
	}

	void OnDisable ()
	{
		Events.G.RemoveListener<LeftCellUnlockedEvent>(LeftCellUnlocked);
		Events.G.RemoveListener<LockCellEvent>(CloseDoor);
		Events.G.RemoveListener<OfficeDoorEvent>(CloseOffice);
	}

	void LeftCellUnlocked(LeftCellUnlockedEvent e){
		if (!_isSeeThroughWall) {
			StartCoroutine (FadeOut (0.4f, 0.0f));
		} else {
			StartCoroutine (FadeOut (1.0f, 0.0f));
		}
		Debug.Log ("Break 1");
	}

	void CloseDoor(LockCellEvent e){
		if (!_isSeeThroughWall) {
			if (e.Locked) {
				StartCoroutine (FadeOut (0.4f, 1.0f));
			} else if (!e.Locked) {
				StartCoroutine (FadeIn (0.4f, 1.0f));
			}
			Debug.Log ("Break 2");
		}
	}

	void CloseOffice(OfficeDoorEvent e){
		if (!_isSeeThroughWall) {
			if (e.Opened) {
				StartCoroutine (FadeOut (0.4f, 1.0f));
			} else if (!e.Opened) {
				StartCoroutine (FadeIn (0.4f, 1.0f));
			}
			Debug.Log ("Break 3");
		}
	}

	public void FadeWall(){
		StartCoroutine (FadeOut (0.4f, 1.0f));
		Debug.Log ("Break 4");
	}

	IEnumerator FadeIn(float from, float to){
		float startTime = Time.time;
		bool i = true;
		while (i) {
			_tempAlpha = MathHelpers.LinMapFrom01 (from, to, 1.0f - (Time.time - startTime)/2f);
			_tempColor.a = _tempAlpha;
			_renderer.material.color = _tempColor;
			yield return null;
			if ((Time.time - startTime)/2f > 1.0f) {
				i = false;
			}
		}
	}

	IEnumerator FadeOut(float from, float to){
		float startTime = Time.time;
		bool i = true;
		while (i) {
			_tempAlpha = MathHelpers.LinMapFrom01 (from, to, (Time.time - startTime)/2f);
			_tempColor.a = _tempAlpha;
			_renderer.material.color = _tempColor;
			yield return null;
			if ((Time.time - startTime)/2f > 1.0f) {
				i = false;
			}
		}
	}
}

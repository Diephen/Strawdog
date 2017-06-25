using UnityEngine;
using System.Collections;

public class WallFade : MonoBehaviour {
	Renderer _renderer;
	float _tempAlpha;
	Color _tempColor;
	Color _emptyColor;

	[SerializeField] bool _isSeeThroughWall = false;
	[SerializeField] bool _isSeeInterrogationWall = false;

	bool _isFadingIn = true;

	void Start(){
		_renderer = GetComponent <SpriteRenderer> ();
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
		if (!_isSeeInterrogationWall) {
			if (!_isSeeThroughWall) {
				_isFadingIn = false;
				StartCoroutine (FadeOut (0.4f, 0.0f));
			} else {
				_isFadingIn = false;
				StartCoroutine (FadeOut (1.0f, 0.0f));
			}
		}
	}

	void CloseDoor(LockCellEvent e){
		if (!_isSeeThroughWall && !_isSeeInterrogationWall) {
			if (e.Locked) {
				_isFadingIn = false;
				StartCoroutine (FadeOut (0.4f, 1.0f));
			} else if (!e.Locked) {
				_isFadingIn = true;
				StartCoroutine (FadeIn (0.4f, 1.0f));
			}
		}
	}

	void CloseOffice(OfficeDoorEvent e){
		if (!_isSeeThroughWall && !_isSeeInterrogationWall) {
			if (e.Opened) {
				_isFadingIn = false;
				StartCoroutine (FadeOut (0.4f, 1.0f));
			} else if (!e.Opened) {
				_isFadingIn = true;
				StartCoroutine (FadeIn (0.4f, 1.0f));
			}
		}
	}

	public void FadeWall(){
		_isFadingIn = false;
		StartCoroutine (FadeOut (0.4f, 1.0f));
	}

	IEnumerator FadeIn(float from, float to){
		float startTime = Time.time;
		bool i = true;
		while (i && _isFadingIn) {
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
		while (i && !_isFadingIn) {
			_tempAlpha = MathHelpers.LinMapFrom01 (from, to, (Time.time - startTime)/2f);
			_tempColor.a = _tempAlpha;
			_renderer.material.color = _tempColor;
			yield return null;
			if ((Time.time - startTime)/2f > 1.0f) {
				i = false;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Guard" || other.tag == "Prisoner") {
			_isFadingIn = true;
			StartCoroutine (FadeIn (0.1f, _renderer.material.color.a));
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.tag == "Guard" || other.tag == "Prisoner") {
			_isFadingIn = false;
			StartCoroutine (FadeOut (_renderer.material.color.a, 0.7f));
		}
	}
}

using UnityEngine;
using System.Collections;

public class WallFade : MonoBehaviour {
	Renderer _renderer;
	float _tempAlpha;
	Color _tempColor;

	void Start(){
		_renderer = gameObject.GetComponent <SpriteRenderer> ();
		_tempColor = _renderer.material.color;
		_tempAlpha = _tempColor.a;
	}

	void OnEnable ()
	{
		Events.G.AddListener<LeftCellUnlockedEvent>(LeftCellUnlocked);
		Events.G.AddListener<LockCellEvent>(CloseDoor);

	}

	void OnDisable ()
	{
		Events.G.RemoveListener<LeftCellUnlockedEvent>(LeftCellUnlocked);
		Events.G.RemoveListener<LockCellEvent>(CloseDoor);
	}

	void LeftCellUnlocked(LeftCellUnlockedEvent e){
	}

	void CloseDoor(LockCellEvent e){
		if (e.Locked) {
			StartCoroutine (FadeOut ());
		} else if (!e.Locked){
			StartCoroutine (FadeIn ());
		}
	}

	IEnumerator FadeIn(){
		float startTime = Time.time;
		bool i = true;
		while (i) {
			_tempAlpha = MathHelpers.LinMapFrom01 (0.0f, 1.0f, 1.0f - (Time.time - startTime)/2f);
			_tempColor.a = _tempAlpha;
			_renderer.material.color = _tempColor;
			yield return null;
			if ((Time.time - startTime)/2f > 1.0f) {
				i = false;
			}
		}
	}

	IEnumerator FadeOut(){
		float startTime = Time.time;
		bool i = true;
		while (i) {
			_tempAlpha = MathHelpers.LinMapFrom01 (0.0f, 1.0f, (Time.time - startTime)/2f);
			_tempColor.a = _tempAlpha;
			_renderer.material.color = _tempColor;
			yield return null;
			if ((Time.time - startTime)/2f > 1.0f) {
				i = false;
			}
		}
	}
}

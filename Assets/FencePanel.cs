using UnityEngine;
using System.Collections;

public class FencePanel : MonoBehaviour {
	//right, speak, right, left
	[SerializeField] SpriteRenderer[] _spriteRendererArray = new SpriteRenderer[4];
	[SerializeField] Sprite[] _spriteArray = new Sprite[4];
	[SerializeField] Sprite _noGlow;
	[SerializeField] Sprite _glow;

	float _tempAlpha;
	Color _tempColor;
	bool _fade = false;

	Timer _fadeTimer = new Timer (1.0f);

	int _codeCnt = 0;

	void Update(){
		if (_fade) {
			_tempAlpha = Mathf.Lerp (_spriteRendererArray [0].color.a, 0.0f, _fadeTimer.PercentTimePassed);
			_tempColor = _spriteRendererArray [0].color;
			_tempColor.a = _tempAlpha;
			for (int i = 0; i < _spriteRendererArray.Length; i++) {
				_spriteRendererArray [i].color = _tempColor;
			}
		}
		else {
			_tempAlpha = Mathf.Lerp (_spriteRendererArray [0].color.a, 1.0f, _fadeTimer.PercentTimePassed);
			_tempColor = _spriteRendererArray [0].color;
			_tempColor.a = _tempAlpha;
			for (int i = 0; i < _spriteRendererArray.Length; i++) {
				_spriteRendererArray [i].color = _tempColor;
			}
		}
	}

	public void Glow(int i){
		_spriteRendererArray [i].sprite = _glow;
	}

	public void GlowFail(){
		for (int i = 0; i < _spriteRendererArray.Length; i++) {
			_spriteRendererArray [i].sprite = _noGlow;
		}
	}

	public void GlowSuccess(){
		for (int i = 0; i < _spriteRendererArray.Length; i++) {
			_spriteRendererArray [i].sprite = _spriteArray[i];
		}
	}

	public void FadePanel(){
		_fadeTimer.Reset ();
		_fade = true;

	}

	public void FadeInPanel(){
		_fadeTimer.Reset ();
		_fade = false;
	}
}


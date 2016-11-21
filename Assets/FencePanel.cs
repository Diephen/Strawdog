using UnityEngine;
using System.Collections;

public class FencePanel : MonoBehaviour {
	//left, right, speak, crouch, speak (0, 2, 1, 3, 1);
	[SerializeField] SpriteRenderer[] _spriteRendererArray = new SpriteRenderer[4];
	[SerializeField] Sprite[] _spriteArray = new Sprite[4];
	[SerializeField] Sprite _noGlow;
	[SerializeField] Sprite _glow;

//	[SerializeField] GameObject _guard;
//	[SerializeField] GameObject _prisoner;
//	KeyCode[] _guardKeyCodes = new KeyCode[4];
//	KeyCode[] _prisonerKeyCodes;
	int _codeCnt = 0;


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

//	public void FadePanel(){
//	}
}


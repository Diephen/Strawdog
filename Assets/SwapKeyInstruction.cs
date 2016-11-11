using UnityEngine;
using System.Collections;

public class SwapKeyInstruction : MonoBehaviour {
	[SerializeField] Sprite _nonPress;
	[SerializeField] Sprite _Press;
	[SerializeField] KeyCode _key;
	SpriteRenderer _sp;

	void Start(){
		_sp = gameObject.GetComponent<SpriteRenderer> ();
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (_key)) {
			_sp.sprite = _Press;
		}
		if (Input.GetKeyUp (_key)) {
			_sp.sprite = _nonPress;
		}
	}
}

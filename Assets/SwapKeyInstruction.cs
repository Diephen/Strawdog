using UnityEngine;
using System.Collections;

public class SwapKeyInstruction : MonoBehaviour {
	[SerializeField] Sprite _nonPress;
	[SerializeField] Sprite _Press;
	[SerializeField] KeyCode _key;
	[SerializeField] Sprite _glow;
	SpriteRenderer _sp;
	bool _isGlow = false;

	void Start(){
		_sp = gameObject.GetComponent<SpriteRenderer> ();
	}

	// Update is called once per frame
	void Update () {
		if (_isGlow) {
			if (Input.GetKey (_key)) {
				_sp.sprite = _Press;
			} 
			else if (Input.GetKeyUp (_key)) {
				_sp.sprite = _nonPress;
			}
			else if (_glow != null) {
				_sp.sprite = _glow;
			}
		}
		else {
			if (Input.GetKey (_key)) {
				_sp.sprite = _Press;
			}
			else if (Input.GetKeyUp (_key)) {
				_sp.sprite = _nonPress;
			}
			else {
				_sp.sprite = _nonPress;
			}
		}

	}

	void EncounterTouch(EncounterTouchEvent e){
		if (e.OnGuard) {
			_isGlow = true;
		}
		else {
			_isGlow = false;
		}
	}

	void OnEnable(){
		Events.G.AddListener<EncounterTouchEvent>(EncounterTouch);
	}

	void OnDisable(){
		Events.G.RemoveListener<EncounterTouchEvent>(EncounterTouch);
	}
}

using UnityEngine;
using System.Collections;

public class CreditScroll : MonoBehaviour {
	float speed = 0.02f;
	[SerializeField] TextMesh _textMesh;
	// Use this for initialization
	void Start () {
	
	}

	void Update(){
		if (Input.GetKeyDown(KeyCode.Return)||Input.GetKeyDown(KeyCode.KeypadEnter)) {
			speed = 0.08f;
		}
		else if(Input.GetKeyUp(KeyCode.Return)||Input.GetKeyUp(KeyCode.KeypadEnter)) {
			speed = 0.03f;
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		transform.Translate (Vector2.up * speed);

		if (transform.position.y > -1.3f && _textMesh.color.a != 1.0f) {
			Color tempColor = _textMesh.color;
			tempColor.a = 1.0f;
			_textMesh.color = tempColor;
		}

		if (transform.position.y > 33.0f) {
			Events.G.Raise(new LoadVeryBeginningEvent());
		}
	}
}

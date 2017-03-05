using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillInTutorialText : MonoBehaviour {

	TextMesh _textMesh;

	// Use this for initialization
	void Start () {
		_textMesh = gameObject.GetComponent<TextMesh>();
		string text = "Hold the <i>'"+GameStateManager.gameStateManager._pDownKey.ToString()+"'</i> key and the <i>'"+GameStateManager.gameStateManager._gDownKey.ToString()+"'</i> \nwhen the characters glow to begin";
		_textMesh.text = text;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

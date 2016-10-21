using UnityEngine;
using System.Collections;
using UnityEngine.VR;

public class HelpMenu : MonoBehaviour {
	Canvas _canvas;
	// Use this for initialization
	void Start () {
		_canvas = gameObject.GetComponent<Canvas> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.H)) {
			_canvas.enabled = !_canvas.enabled;
		}
	}
}

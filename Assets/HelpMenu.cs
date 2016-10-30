using UnityEngine;
using System.Collections;
using UnityEngine.VR;

public class HelpMenu : MonoBehaviour {
	[SerializeField] GameObject _controls;
	[SerializeField] GameObject _start;

	void Update () {
		if (Input.GetKeyDown (KeyCode.H)) {
			_controls.SetActive(!_controls.activeSelf);
		}
		if (Input.GetKeyDown (KeyCode.Space)) {
			_start.SetActive (false);
		}
	}
}

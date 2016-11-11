using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class HelpMenu : MonoBehaviour {
	[SerializeField] GameObject _controls;

	static HelpMenu _helpInstance = null;

	void Awake() {
		if (_helpInstance) {
			Destroy (gameObject);
		}
		else {
			_helpInstance = this;
			DontDestroyOnLoad (gameObject);
		}
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.H)) {
			_controls.SetActive (!_controls.activeSelf);
		}
	}

	void OnEnable(){
		SceneManager.sceneLoaded += CheckStartUI;
	}

	void OnDisable(){
		SceneManager.sceneLoaded -= CheckStartUI;
	}

	void CheckStartUI(Scene scene, LoadSceneMode mode){
		_controls.SetActive (false);
	}
}

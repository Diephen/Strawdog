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
		if (SceneManager.GetActiveScene ().name == "Tutorial") {
			if (Input.GetKeyDown (KeyCode.Space)) {
				Events.G.Raise (new TutorialEndEvent ());
			}
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

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class HelpMenu : MonoBehaviour {
	[SerializeField] GameObject _controls;

	static HelpMenu _helpInstance = null;
	KeyCode _helpKey;

	void Awake() {
		if (_helpInstance) {
			Destroy (gameObject);
		}
		else {
			_helpInstance = this;
			DontDestroyOnLoad (gameObject);
		}
	}

	void Start(){
		_helpKey = GameStateManager.gameStateManager._helpKey;
	}

	void Update () {
		if (Input.GetKeyDown (_helpKey)) {
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

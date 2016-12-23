using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameStateManager : MonoBehaviour {

	public static GameStateManager gameStateManager;

	public static bool[] _acquiredStates = new bool[7] {false, false, false, false, false, false, false};
	// 0 : Killed at ditch
	// 1 : Prisoner Escapes
	// 2 : Execution for Crimes
	// 3 : Stopped Escape
	// 4 : Happy Ending?
	// 5 : Plant Bomb
	// 6 : Final Ending
	public SceneIndex _prevScene { get; set; }
	public SceneIndex _currScene { get; set; }
	public CharacterIdentity _currChar { get; set; }
	public int _playthroughCnt { get; set; }

	//Key Codes
	public KeyCode _gLeftKey { get; set; }
	public KeyCode _gUpKey { get; set; }
	public KeyCode _gDownKey { get; set; }
	public KeyCode _gRightKey { get; set; }

	public KeyCode _pLeftKey { get; set; }
	public KeyCode _pUpKey { get; set; }
	public KeyCode _pDownKey { get; set; }
	public KeyCode _pRightKey { get; set; }

	public KeyCode _helpKey { get; set; }



	void Awake () {
		if (gameStateManager == null) {
			DontDestroyOnLoad (gameObject);
			gameStateManager = this;
		}
		else if (gameStateManager != this) {
			Destroy (gameObject);
		}

		_gLeftKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("gLeftKey", "A"));
		_gDownKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("gDownKey", "S"));
		_gRightKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("gRightKey", "D"));
		_gUpKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("gUpKey", "W"));

		_pLeftKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("pLeftKey", "LeftArrow"));
		_pDownKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("pDownKey", "DownArrow"));
		_pRightKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("pRightKey", "RightArrow"));
		_pUpKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("pUpKey", "UpArrow"));

		_helpKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("helpKey", "H"));

	}
		
	public void Save () {
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/save.dat");
	
		SaveData data = new SaveData ();
		//storing data
		data.endingSave = _acquiredStates;
		data.prevScene = _prevScene;
		data.currScene = _currScene;
		data.currChar = _currChar;
		data.playthroughCnt = _playthroughCnt;

		bf.Serialize (file, data);
		file.Close();
	}

	public void Load(){
		if (File.Exists (Application.persistentDataPath + "/save.dat")) {
			
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/save.dat", FileMode.Open);
			SaveData data = (SaveData)bf.Deserialize (file);
			file.Close ();
			//setting data value
			_acquiredStates = data.endingSave;
			_prevScene = data.prevScene;
			_currScene = data.currScene;
			_currChar = data.currChar;
			_playthroughCnt = data.playthroughCnt;
		}
	}

	public void Delete(){
		if (File.Exists (Application.persistentDataPath + "/save.dat")) {
			File.Delete (Application.persistentDataPath + "/save.dat");
		}
	}


	void OnEnable(){
	}

	void OnDisable(){
	}
}


[Serializable]
class SaveData
{
	public bool[] endingSave;
	public SceneIndex prevScene;
	public SceneIndex currScene;
	public CharacterIdentity currChar;
	public int playthroughCnt;
}
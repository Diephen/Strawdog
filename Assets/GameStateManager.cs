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
	SceneIndex _prevScene;
	SceneIndex _currScene;
	CharacterIdentity _currChar;


	void Awake () {
		if (gameStateManager == null) {
			DontDestroyOnLoad (gameObject);
			gameStateManager = this;
		}
		else if (gameStateManager != this) {
			Destroy (gameObject);
		}
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
}
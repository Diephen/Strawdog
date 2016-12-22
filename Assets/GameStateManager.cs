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
		data.endingSave = _acquiredStates;

		bf.Serialize (file, data);
		file.Close;
	}

	public void Load(){
		if (File.Exists (Application.persistentDataPath + "/save.dat")) {
			
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/save.dat");
			SaveData data = (SaveData)bf.Deserialize (file);
			file.Close ();

			_acquiredStates = data.endingSave;
		}
	}
}


[Serializable]
class SaveData
{
	public bool[] endingSave;
}
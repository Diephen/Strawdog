using UnityEngine;
using System.Collections;

public class TempSceneMove : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.anyKeyDown) {
			Events.G.Raise (new Plant_LeaveFoodStorageEvent ());
		}
	}
}

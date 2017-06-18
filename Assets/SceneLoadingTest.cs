using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoadingTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		#if (UNITY_EDITOR)

		if(Input.GetKeyDown(KeyCode.Space)){
			GameStateManager.gameStateManager._executionAsGuard = true;
			Events.G.Raise(new Load4_1Event());
		}

		#endif
	}
}

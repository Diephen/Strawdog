using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour {


	public void NewButton(string newGameLevel){
		Debug.Log ("New Button Pressed");
	}

	public void ContinueButton(){
		Debug.Log ("Continue Button Pressed");
	}

	public void OptionsButton(){
		Debug.Log ("Options Button Pressed");
	}

	public void QuitButton(){
		Debug.Log ("Quit Button Pressed");
		Application.Quit ();
	}
}

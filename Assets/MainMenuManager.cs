using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public enum languageSetting {
	English, Chinese, Korean
}

public class MainMenuManager : MonoBehaviour {
	public Toggle _fullscreenToggle;
	public Toggle _subtitleToggle;
	public Dropdown _languagesDropdown;

	public static bool subtitle = false;
	public static languageSetting currentLanguage;

	[SerializeField]Transform menuPanel;
	Event keyEvent;
	Text buttonText;
	KeyCode newKey;

	bool waitingForKey;


	[SerializeField] GameStateManager _gameStateManager;
	[SerializeField] GameObject _optionMenu;

	[SerializeField] string[] _menuEnglish;
	[SerializeField] string[] _menuChinese;
	[SerializeField] string[] _menuKorean;
	[SerializeField] Text[] _menuText;

	void Awake(){
		if (_gameStateManager == null) {
			_gameStateManager = GameObject.Find ("SceneManagement").gameObject.GetComponent<GameStateManager> ();
		}
	}

	void Start(){
		ResetLanguage();
		if (menuPanel == null) {
			menuPanel = GameObject.Find ("ControlsRebinding").transform;
		}
		menuPanel.gameObject.SetActive(false);
		waitingForKey = false;

		//Iterate through GameObject and asign Child name
		for(int i = 0; i < menuPanel.childCount; i++)
		{
			if (menuPanel.GetChild (i).name == "GLeftKey") {
				menuPanel.GetChild (i).GetComponentInChildren<Text> ().text = GameStateManager.gameStateManager._gLeftKey.ToString ();
			}
			else if(menuPanel.GetChild(i).name == "GDownKey"){
				menuPanel.GetChild (i).GetComponentInChildren<Text> ().text = GameStateManager.gameStateManager._gDownKey.ToString ();
			}
			else if(menuPanel.GetChild(i).name == "GRightKey"){
				menuPanel.GetChild (i).GetComponentInChildren<Text> ().text = GameStateManager.gameStateManager._gRightKey.ToString ();
			}
			else if(menuPanel.GetChild(i).name == "GUpKey"){
				menuPanel.GetChild (i).GetComponentInChildren<Text> ().text = GameStateManager.gameStateManager._gUpKey.ToString ();
			}
			else if(menuPanel.GetChild(i).name == "PLeftKey"){
				menuPanel.GetChild (i).GetComponentInChildren<Text> ().text = GameStateManager.gameStateManager._pLeftKey.ToString ();
			}
			else if(menuPanel.GetChild(i).name == "PDownKey"){
				menuPanel.GetChild (i).GetComponentInChildren<Text> ().text = GameStateManager.gameStateManager._pDownKey.ToString ();
			}
			else if(menuPanel.GetChild(i).name == "PRightKey"){
				menuPanel.GetChild (i).GetComponentInChildren<Text> ().text = GameStateManager.gameStateManager._pRightKey.ToString ();
			}
			else if(menuPanel.GetChild(i).name == "PUpKey"){
				menuPanel.GetChild (i).GetComponentInChildren<Text> ().text = GameStateManager.gameStateManager._pUpKey.ToString ();
			}
			else if(menuPanel.GetChild(i).name == "HelpKey"){
				menuPanel.GetChild (i).GetComponentInChildren<Text> ().text = GameStateManager.gameStateManager._helpKey.ToString ();
			}
		}
	}

	void ResetLanguage(){
		int thisLanguage = PlayerPrefs.GetInt("currentLanguage");
		for(int i = 0; i < _menuText.Length; i++){
			if(thisLanguage == 0){
				_menuText[i].text = _menuEnglish[i];
			} else if (thisLanguage == 1) {
				_menuText[i].text = _menuChinese[i];
			} else {
				_menuText[i].text = _menuKorean[i];
			}
		}
	}

	void OnGUI()
	{
		/*keyEvent dictates what key our user presses
		 * bt using Event.current to detect the current
		 * event
		 */
		keyEvent = Event.current;

		//Executes if a button gets pressed and
		//the user presses a key
		if(keyEvent.isKey && waitingForKey)
		{
			newKey = keyEvent.keyCode; //Assigns newKey to the key user presses
			waitingForKey = false;
		}
	}

	/*Buttons cannot call on Coroutines via OnClick().
	 * Instead, we have it call StartAssignment, which will
	 * call a coroutine in this script instead, only if we
	 * are not already waiting for a key to be pressed.
	 */
	public void StartAssignment(string keyName)
	{
		if(!waitingForKey)
			StartCoroutine(AssignKey(keyName));
	}

	//Assigns buttonText to the text component of
	//the button that was pressed
	public void SendText(Text text)
	{
		buttonText = text;
	}

	//Used for controlling the flow of our below Coroutine
	IEnumerator WaitForKey()
	{
		while(!keyEvent.isKey)
			yield return null;
	}

	/*AssignKey takes a keyName as a parameter. The
	 * keyName is checked in a switch statement. Each
	 * case assigns the command that keyName represents
	 * to the new key that the user presses, which is grabbed
	 * in the OnGUI() function, above.
	 */
	public IEnumerator AssignKey(string keyName)
	{
		waitingForKey = true;

		yield return WaitForKey(); //Executes endlessly until user presses a key

		switch(keyName)
		{
		case "gLeftKey":
			GameStateManager.gameStateManager._gLeftKey = newKey; //Set forward to new keycode
			buttonText.text = GameStateManager.gameStateManager._gLeftKey.ToString(); //Set button text to new key
			PlayerPrefs.SetString("gLeftKey", GameStateManager.gameStateManager._gLeftKey.ToString()); //save new key to PlayerPrefs
			break;
		case "gDownKey":
			GameStateManager.gameStateManager._gDownKey = newKey; //set backward to new keycode
			buttonText.text = GameStateManager.gameStateManager._gDownKey.ToString(); //set button text to new key
			PlayerPrefs.SetString("gDownKey", GameStateManager.gameStateManager._gDownKey.ToString()); //save new key to PlayerPrefs
			break;
		case "gRightKey":
			GameStateManager.gameStateManager._gRightKey = newKey; //set left to new keycode
			buttonText.text = GameStateManager.gameStateManager._gRightKey.ToString(); //set button text to new key
			PlayerPrefs.SetString("gRightKey", GameStateManager.gameStateManager._gRightKey.ToString()); //save new key to playerprefs
			break;
		case "gUpKey":
			GameStateManager.gameStateManager._gUpKey = newKey; //set right to new keycode
			buttonText.text = GameStateManager.gameStateManager._gUpKey.ToString(); //set button text to new key
			PlayerPrefs.SetString("gUpKey", GameStateManager.gameStateManager._gUpKey.ToString()); //save new key to playerprefs
			break;
		case "pLeftKey":
			GameStateManager.gameStateManager._pLeftKey = newKey; //Set forward to new keycode
			buttonText.text = GameStateManager.gameStateManager._pLeftKey.ToString(); //Set button text to new key
			PlayerPrefs.SetString("pLeftKey", GameStateManager.gameStateManager._pLeftKey.ToString()); //save new key to PlayerPrefs
			break;
		case "pDownKey":
			GameStateManager.gameStateManager._pDownKey = newKey; //set backward to new keycode
			buttonText.text = GameStateManager.gameStateManager._pDownKey.ToString(); //set button text to new key
			PlayerPrefs.SetString("pDownKey", GameStateManager.gameStateManager._pDownKey.ToString()); //save new key to PlayerPrefs
			break;
		case "pRightKey":
			GameStateManager.gameStateManager._pRightKey = newKey; //set left to new keycode
			buttonText.text = GameStateManager.gameStateManager._pRightKey.ToString(); //set button text to new key
			PlayerPrefs.SetString("pRightKey", GameStateManager.gameStateManager._pRightKey.ToString()); //save new key to playerprefs
			break;
		case "pUpKey":
			GameStateManager.gameStateManager._pUpKey = newKey; //set right to new keycode
			buttonText.text = GameStateManager.gameStateManager._pUpKey.ToString(); //set button text to new key
			PlayerPrefs.SetString("pUpKey", GameStateManager.gameStateManager._pUpKey.ToString()); //save new key to playerprefs
			break;
		case "helpKey":
			GameStateManager.gameStateManager._helpKey = newKey; //set jump to new keycode
			buttonText.text = GameStateManager.gameStateManager._helpKey.ToString(); //set button text to new key
			PlayerPrefs.SetString("helpKey", GameStateManager.gameStateManager._helpKey.ToString()); //save new key to playerprefs
			break;
		}

		yield return null;
	}

	void OnEnable(){
		_fullscreenToggle.isOn = Screen.fullScreen;
		if (PlayerPrefs.GetInt ("subtitle") == 1) {
			_subtitleToggle.isOn = true;
		}
		else {
			_subtitleToggle.isOn = false;
		}
		_languagesDropdown.value = PlayerPrefs.GetInt ("currentLanguage");

		_fullscreenToggle.onValueChanged.AddListener(delegate {OnFullscreenToggle();});
		_subtitleToggle.onValueChanged.AddListener(delegate {OnSubtitleToggle();});
		_languagesDropdown.onValueChanged.AddListener (delegate {OnLanguagesDropdown ();});
	}

	public void NewButton(){
		Events.G.Raise (new LoadTutorialEvent ());
	}

	public void ContinueButton(){
		Debug.Log ("Continue Button Pressed");
	}

	public void OptionsButton(){
		_optionMenu.SetActive(true);
	}

	public void QuitButton(){
		Debug.Log ("Quit Button Pressed");
		Application.Quit ();
	}

	public void BackButton(){
		_optionMenu.SetActive(false);
	}

	public void ControlsButton(){
		menuPanel.gameObject.SetActive (true);
	}

	public void AcceptButton(){
		menuPanel.gameObject.SetActive (false);
	}


	public void OnFullscreenToggle(){
		Screen.fullScreen = _fullscreenToggle.isOn;
	}

	public void OnSubtitleToggle(){
		subtitle = _subtitleToggle.isOn;
		if (subtitle) {
			PlayerPrefs.SetInt ("subtitle", 1);
		}
		else {
			PlayerPrefs.SetInt ("subtitle", 0);
		}
	}

	public void OnLanguagesDropdown(){
		if (_languagesDropdown.value == 0) {
			currentLanguage = languageSetting.English;
		}
		else if (_languagesDropdown.value == 1) {
			currentLanguage = languageSetting.Chinese;
		}
		else if (_languagesDropdown.value == 2) {
			currentLanguage = languageSetting.Korean;
		}
		PlayerPrefs.SetInt("currentLanguage", _languagesDropdown.value);
		ResetLanguage();
	}

	public void DeleteSaveFile(){
		_gameStateManager.Delete ();
	}
		
	public void ResetToDefault(){
		//Iterate through GameObject and asign Child name
		for(int i = 0; i < menuPanel.childCount; i++)
		{
			if (menuPanel.GetChild (i).name == "GLeftKey") {
				GameStateManager.gameStateManager._gLeftKey = KeyCode.LeftArrow; //Set forward to new keycode
				menuPanel.GetChild (i).GetComponentInChildren<Text> ().text = GameStateManager.gameStateManager._gLeftKey.ToString(); //Set button text to new key
				PlayerPrefs.SetString("gLeftKey", GameStateManager.gameStateManager._gLeftKey.ToString()); //save new key to PlayerPrefs
			}
			else if(menuPanel.GetChild(i).name == "GDownKey"){
				GameStateManager.gameStateManager._gDownKey = KeyCode.DownArrow; //Set forward to new keycode
				menuPanel.GetChild (i).GetComponentInChildren<Text> ().text = GameStateManager.gameStateManager._gDownKey.ToString(); //Set button text to new key
				PlayerPrefs.SetString("gDownKey", GameStateManager.gameStateManager._gDownKey.ToString()); //save new key to PlayerPrefs
			}
			else if(menuPanel.GetChild(i).name == "GRightKey"){
				GameStateManager.gameStateManager._gRightKey = KeyCode.RightArrow; //Set forward to new keycode
				menuPanel.GetChild (i).GetComponentInChildren<Text> ().text = GameStateManager.gameStateManager._gRightKey.ToString(); //Set button text to new key
				PlayerPrefs.SetString("gRightKey", GameStateManager.gameStateManager._gRightKey.ToString()); //save new key to PlayerPrefs
			}
			else if(menuPanel.GetChild(i).name == "GUpKey"){
				GameStateManager.gameStateManager._gUpKey = KeyCode.UpArrow; //Set forward to new keycode
				menuPanel.GetChild (i).GetComponentInChildren<Text> ().text = GameStateManager.gameStateManager._gUpKey.ToString(); //Set button text to new key
				PlayerPrefs.SetString("gUpKey", GameStateManager.gameStateManager._gUpKey.ToString()); //save new key to PlayerPrefs
			}
			else if(menuPanel.GetChild(i).name == "PLeftKey"){
				GameStateManager.gameStateManager._pLeftKey = KeyCode.A; //Set forward to new keycode
				menuPanel.GetChild (i).GetComponentInChildren<Text> ().text = GameStateManager.gameStateManager._pLeftKey.ToString(); //Set button text to new key
				PlayerPrefs.SetString("pLeftKey", GameStateManager.gameStateManager._pLeftKey.ToString()); //save new key to PlayerPrefs
			}
			else if(menuPanel.GetChild(i).name == "PDownKey"){
				GameStateManager.gameStateManager._pDownKey = KeyCode.S; //Set forward to new keycode
				menuPanel.GetChild (i).GetComponentInChildren<Text> ().text = GameStateManager.gameStateManager._pDownKey.ToString(); //Set button text to new key
				PlayerPrefs.SetString("pDownKey", GameStateManager.gameStateManager._pDownKey.ToString()); //save new key to PlayerPrefs
			}
			else if(menuPanel.GetChild(i).name == "PRightKey"){
				GameStateManager.gameStateManager._pRightKey = KeyCode.D; //Set forward to new keycode
				menuPanel.GetChild (i).GetComponentInChildren<Text> ().text = GameStateManager.gameStateManager._pRightKey.ToString(); //Set button text to new key
				PlayerPrefs.SetString("pRightKey", GameStateManager.gameStateManager._pRightKey.ToString()); //save new key to PlayerPrefs
			}
			else if(menuPanel.GetChild(i).name == "PUpKey"){
				GameStateManager.gameStateManager._pUpKey = KeyCode.W; //Set forward to new keycode
				menuPanel.GetChild (i).GetComponentInChildren<Text> ().text = GameStateManager.gameStateManager._pUpKey.ToString(); //Set button text to new key
				PlayerPrefs.SetString("pUpKey", GameStateManager.gameStateManager._pUpKey.ToString()); //save new key to PlayerPrefs
			}
			else if(menuPanel.GetChild(i).name == "HelpKey"){
				GameStateManager.gameStateManager._helpKey = KeyCode.H; //Set forward to new keycode
				menuPanel.GetChild (i).GetComponentInChildren<Text> ().text = GameStateManager.gameStateManager._helpKey.ToString(); //Set button text to new key
				PlayerPrefs.SetString("helpKey", GameStateManager.gameStateManager._helpKey.ToString()); //save new key to PlayerPrefs
			}
		}
	}
}

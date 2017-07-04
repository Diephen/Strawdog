using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageSwitchForText : MonoBehaviour {
	[SerializeField] bool _isTextMesh = false;
	[SerializeField] bool _isChinese = true;
	TextMesh _myTextMesh;
	Text _myText;
	languageSetting _CurLanguage;
	[SerializeField] string[] _textStrings;

	// Use this for initialization
	void Awake () {
		if (_isTextMesh) {
			_myTextMesh = GetComponent<TextMesh> ();
		}else{
			_myText = GetComponent<Text> ();
		}

	}
	
	// Update is called once per frame
	void Update () {
		if (_isChinese) {
			Events.G.Raise (new LanguageSwitchEvent (languageSetting.Chinese));
		} else {
			Events.G.Raise (new LanguageSwitchEvent (languageSetting.English));
		}
		
	}

	void ChangeTextLanguage(LanguageSwitchEvent e){
		if (_CurLanguage != e.lg) {
			_CurLanguage = e.lg;
			switch(_CurLanguage){
			case languageSetting.English:
				//_isTextMesh ? _myTextMesh.text = _textStrings [0] : _myText.text = _textStrings [0];
				if (_isTextMesh) {
					_myTextMesh.text = _textStrings [0];
					//_myTextMesh.font = Resources.Load ("Fonts/Adler") as Font;
				}else{
					_myText.text = _textStrings [0];
					_myText.font = Resources.Load ("Fonts/Adler") as Font;
				}
				break;
			case languageSetting.Chinese:
				//_isTextMesh ? _myTextMesh.text = _textStrings [1] : _myText.text = _textStrings [1];
				if (_isTextMesh) {
					_myTextMesh.text = _textStrings [1];
					//_myTextMesh.font = Resources.Load ("Fonts/TypeLand") as Font;
				}else{
					_myText.text = _textStrings [1];
					_myText.font = Resources.Load ("Fonts/TypeLand") as Font;
				}
				break;
			case languageSetting.Korean:
				if (_isTextMesh) {
					_myTextMesh.text = _textStrings [2];
				}else{
					_myText.text = _textStrings [2];
				}
				//_isTextMesh ? _myTextMesh.text = _textStrings [2] : _myText.text = _textStrings [2];
				break;

			}
		}


		
	}


	void OnEnable(){
		Events.G.AddListener<LanguageSwitchEvent>(ChangeTextLanguage);
		//print ("enable language script");
	}

	void OnDisable(){
		Events.G.RemoveListener<LanguageSwitchEvent>(ChangeTextLanguage);
		//print ("disable language script");
	}
}

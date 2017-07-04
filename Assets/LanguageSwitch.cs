using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageSwitch : MonoBehaviour {
	// 0 - english 1- chinese 3 - Korean
	[SerializeField] bool _isChinese = true;
	[SerializeField] Sprite[] _TextImages;
	SpriteRenderer _LanSpriteRenderer;
	languageSetting _CurLanguage = 0;

	bool isCh = false;

	// Use this for initialization
	void Start () {
		_LanSpriteRenderer = GetComponent<SpriteRenderer> ();

	}
	
	// Update is called once per frame
	void Update () {
		if (_isChinese) {
			Events.G.Raise (new LanguageSwitchEvent(languageSetting.Chinese));
		}
//		if (Input.GetKeyDown (KeyCode.L)) {
//			
//			isCh = !isCh;
//			if (isCh) {
//				Events.G.Raise (new LanguageSwitchEvent(languageSetting.Chinese));
//			} else {
//				Events.G.Raise (new LanguageSwitchEvent(languageSetting.English));
//			}
//		}



	}



	void LanguageSwitchHandle(LanguageSwitchEvent e){
		if (_CurLanguage != e.lg) {
			_CurLanguage = e.lg;
			_LanSpriteRenderer.sprite = _TextImages [(int)_CurLanguage];
		}

//		switch(_CurLanguage){
//		case languageSetting.English:
//			
//
//		}
	}

	void OnEnable(){
		Events.G.AddListener<LanguageSwitchEvent>(LanguageSwitchHandle);
		//print ("enable language script");
	}

	void OnDisable(){
		Events.G.RemoveListener<LanguageSwitchEvent>(LanguageSwitchHandle);
		//print ("disable language script");
	}
}

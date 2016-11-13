using UnityEngine;
using System.Collections;

public class SecretDoorPanel : MonoBehaviour {
	//left, right, speak, crouch, speak (0, 2, 1, 3, 1);
	[SerializeField] SpriteRenderer[] _spriteRendererArray = new SpriteRenderer[5];
	[SerializeField] Sprite[] _spriteArray = new Sprite[5];
	[SerializeField] Sprite _noGlow;
	[SerializeField] Sprite _glow;

	[SerializeField] GameObject _guard;
	[SerializeField] GameObject _prisoner;
	KeyCode[] _guardKeyCodes = new KeyCode[4];
	KeyCode[] _prisonerKeyCodes;
	int _codeCnt = 0;

	bool _isSecret = false;
	bool _answer = false;
	bool _solved = false;
	AudioSource _audioSource;
	Timer _offsetTimer = new Timer (0.4f);
	Timer _waitToStartTimer = new Timer(1.0f);

	[SerializeField] bool _isFlip = false;

	// Use this for initialization
	void Start () {
		_audioSource = gameObject.AddComponent<AudioSource> ();
		_audioSource.clip = Resources.Load<AudioClip> ("Sounds/PianoPluckShorter");
		_audioSource.volume = 0.3f;
		PuppetControl tempPuppetControl = _guard.GetComponent <PuppetControl> ();
		_guardKeyCodes [0] = tempPuppetControl.GetKeyCodes() [0];
		_guardKeyCodes [1] = tempPuppetControl.GetKeyCodes() [1];
		_guardKeyCodes [2] = tempPuppetControl.GetKeyCodes() [2];
		_guardKeyCodes [3] = tempPuppetControl.GetKeyCodes() [3];

		_prisonerKeyCodes = _prisoner.GetComponent<PuppetControl> ().GetKeyCodes ();
		if (_isFlip) {
			KeyCode tempCode = _guardKeyCodes [0];
			_guardKeyCodes [0] = _guardKeyCodes [2];
			_guardKeyCodes [2] = tempCode;
		}
	}
	
	// Update is called once per frame
	void Update () {
		CheckPassword ();
	}

	void Glow(int i){
		_spriteRendererArray [i].sprite = _glow;
	}

	void GlowFail(){
		for (int i = 0; i < _spriteRendererArray.Length; i++) {
			_spriteRendererArray [i].sprite = _noGlow;
		}
	}

	void GlowSuccess(){
		for (int i = 0; i < _spriteRendererArray.Length; i++) {
			_spriteRendererArray [i].sprite = _spriteArray[i];
		}
	}

	void SecretDoor (TransitionSecretDoorEvent e){
		_isSecret = e.SecretOn;
		_waitToStartTimer.Reset ();
	}

	void CheckPassword(){
		if (_isSecret && _waitToStartTimer.IsOffCooldown && !_solved) {
			if (_codeCnt == 0) {
				if (Input.GetKeyDown (_guardKeyCodes [0]) || Input.GetKeyDown (_prisonerKeyCodes [0])) {
					_audioSource.Play ();
					_offsetTimer.Reset ();
					Glow (_codeCnt);
					_codeCnt = 1;
					_answer = true;
				}
				else if (Input.anyKeyDown) {
					_audioSource.Play ();
					_offsetTimer.Reset ();
					Glow (_codeCnt);
					_codeCnt = 1;
				}
			}
			else if (_codeCnt == 1 && _offsetTimer.IsOffCooldown) {
				if (Input.GetKeyDown (_guardKeyCodes [2]) || Input.GetKeyDown (_prisonerKeyCodes [2])) {
					_audioSource.Play ();
					_offsetTimer.Reset ();
					Glow (_codeCnt);
					_codeCnt = 2;
				}
				else if (Input.anyKeyDown) {
					_answer = false;
					_audioSource.Play ();
					_offsetTimer.Reset ();
					Glow (_codeCnt);
					_codeCnt = 2;
				}
			}
			else if (_codeCnt == 2 && _offsetTimer.IsOffCooldown) {
				if (Input.GetKeyDown (_guardKeyCodes [1]) || Input.GetKeyDown (_prisonerKeyCodes [1])) {
					_audioSource.Play ();
					_offsetTimer.Reset ();
					Glow (_codeCnt);
					_codeCnt = 3;
					Events.G.Raise (new StopSecretExitEvent (true));
				}
				else if (Input.anyKeyDown) {
					_answer = false;
					_audioSource.Play ();
					_offsetTimer.Reset ();
					Glow (_codeCnt);
					_codeCnt = 3;
					Events.G.Raise (new StopSecretExitEvent (true));
				}
			}
			else if (_codeCnt == 3 && _offsetTimer.IsOffCooldown) {
				if (Input.GetKeyDown (_guardKeyCodes [3]) || Input.GetKeyDown (_prisonerKeyCodes [3])) {
					_audioSource.Play ();
					_offsetTimer.Reset ();
					Glow (_codeCnt);
					_codeCnt = 4;
				}
				else if (Input.anyKeyDown) {
					_answer = false;
					_audioSource.Play ();
					_offsetTimer.Reset ();
					Glow (_codeCnt);
					_codeCnt = 4;
				}
			}
			else if (_codeCnt == 4 && _offsetTimer.IsOffCooldown) {
				if (Input.GetKeyDown (_guardKeyCodes [1]) || Input.GetKeyDown (_prisonerKeyCodes [1])) {
					_audioSource.Play ();
					Glow (_codeCnt);
					_codeCnt = 5;
					if (_answer) {
						StartCoroutine (ShowResults(true));
					}
					else {
						StartCoroutine (ShowResults(false));
					}
				}
				else if (Input.anyKeyDown) {
					_answer = false;
					_audioSource.Play ();
					Glow (_codeCnt);
					_codeCnt = 5;
					StartCoroutine (ShowResults(false));
				}
			}
		}
		else if (!_isSecret) {
			GlowFail ();
			_codeCnt = 0;
		}
	}

	IEnumerator ShowResults(bool result){
		yield return new WaitForSeconds(1);
		if (result) {
			GlowSuccess ();
			_solved = true;
			_codeCnt = 0;
		}
		else {
			GlowFail ();
			_codeCnt = 0;
			Events.G.Raise (new StopSecretExitEvent (false));
		}
	}

	void OnEnable(){
		Events.G.AddListener<TransitionSecretDoorEvent>(SecretDoor);
	}
	void OnDisable(){
		Events.G.RemoveListener<TransitionSecretDoorEvent>(SecretDoor);
	}
}


using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Act5Handle : MonoBehaviour {
	[SerializeField] AudioClip _order1;
	[SerializeField] AudioClip _order2;
	[SerializeField] AudioClip _order3;
	[SerializeField] AudioClip _order4;

	[SerializeField] Text _title;
	[SerializeField] Text _space;

	AudioSource _audioSource;

	Color _originalColor;
	Timer _textOnTimer = new Timer(4.0f);
	Timer _textOffTimer = new Timer(1.0f);
	bool _transition = false;
	bool _once = true;
	bool _trans2 = false;
	float _tempAlpha;
	Color _tempTextColor;

	string _part1 = "You are from a wealthy family and you do not enjoy conflict.\n\nBut a war breaks out and your family pulls some strings so that you can avoid fighting in the war.\n\nInstead you are assigned to manage a prison\n\n\nYou are okay with this position";
	string _part2 = "The higher ups told me that there was rebellion brewing.\nThey tell me to get more information on this";
	string _part3 = "As the war rages on, my area also gets attacked and the prison got cut off on resources.\n\nI have too many prisoners, meaning too many mouths to feed\n\nThe higher ups tell me that they will try sending resources but it wont be for a while\n\nSomething drastic needs to happen";
	string _part4 = "It seems that the war has been lost\nand the support doesnt seem to be on its way.\n\nWith no supplies, my men are dropping dead of hunger\n\nThey are willing to eat anything, or anyone...\n\nAt least this way, some of us can still survive";
	string _part5 = "I hear that a rescue team is on the way\nbut what happened in the prison must never be heard of\n\nBurn the whole facility, all of them...";
	string _part6 = "I stand trial for my war crimes\n\nand I am relieved as my involvement in the war and crimes have been determined to be insufficient for punishment\n";
	string _part7 = "Filled with guilt.\nI live in denial of what happened back in the prison\n\nI decide to build a puppet theater";

	string _next1 = "(Press Space) Next";
	string _next2 = "(Press Space) Give Order: 'Extract Information from Captured Prisoners'";
	string _next3 = "(Press Space) Give Order: 'Execute All Suspicious Individuals'";
	string _next4 = "(Press Space) Give Order: 'Relocate Selected Prisoners'";
	string _next5 = "(Press Space) Give Order: 'Final Order, Destory All Evidence'";
	string _next6 = "(Press Space) Years Later";
	string _next7 = "(Press Space) Tell the Story?";

	int cnt = 1;

	// Use this for initialization
	void Start () {
		_audioSource = gameObject.AddComponent<AudioSource> ();
		_audioSource.volume = 0.6f;
		_originalColor = _title.color;
		_tempTextColor = _title.color;
		_textOnTimer.Reset ();
		_title.text = _part1;
		_space.text = _next1;
	}

	void Update(){
		if (_textOnTimer.IsOffCooldown && Input.GetKeyDown (KeyCode.Space)) {
			_transition = true;
			_textOffTimer.Reset ();
			_trans2 = true;
			if (cnt == 2) {
				_audioSource.clip = _order1;
				_audioSource.Play ();
			}
			else if (cnt == 3) {
				_audioSource.clip = _order2;
				_audioSource.Play ();
			}
			else if (cnt == 4) {
				_audioSource.clip = _order3;
				_audioSource.Play ();
			}
			else if (cnt == 5) {
				_audioSource.clip = _order4;
				_audioSource.Play ();
			}
			else if (cnt == 7) {
				Events.G.Raise (new StartCreditsEvent ());
			}
		}
	}

	void FixedUpdate () {
		if (!_transition) {
			_tempAlpha = Mathf.Lerp (0.0f, 1.0f, _textOnTimer.PercentTimePassed);
				_tempTextColor.a = _tempAlpha;
				_space.color = _tempTextColor;
				_title.color = _tempTextColor;
		}
		else {
			_tempAlpha = Mathf.Lerp (1.0f, 0.0f, _textOffTimer.PercentTimePassed);
			_tempTextColor.a = _tempAlpha;
			_space.color = _tempTextColor;
			_title.color = _tempTextColor;
		}

		if (_textOffTimer.IsOffCooldown && _trans2) {
			cnt++;
			if (cnt == 2) {
				_title.text = _part2;
				_space.text = _next2;
			}
			else if (cnt == 3) {
				_title.text = _part3;
				_space.text = _next3;
			}
			else if (cnt == 4) {
				_title.text = _part4;
				_space.text = _next4;
			}
			else if (cnt == 5) {
				_title.text = _part5;
				_space.text = _next5;
			}
			else if (cnt == 6) {
				_title.text = _part6;
				_space.text = _next6;
			}
			else if (cnt == 7) {
				_title.text = _part7;
				_space.text = _next7;
			}
			_textOnTimer.Reset ();
			if (cnt == 8) {
				_transition = true;
			}
			else {
				_transition = false;
			}
			_trans2 = false;
		}
	}
}

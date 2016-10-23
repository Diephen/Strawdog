using UnityEngine;
using System.Collections;

public class FrameScript : MonoBehaviour {
	[SerializeField] GameObject _leftFlap;
	[SerializeField] GameObject _rightFlap;
	[SerializeField] MinMax _leftFlapRange = new MinMax(-12.11f , -4.61f);
	[SerializeField] MinMax _rightFlapRange = new MinMax(4.46f , 11.96f);
	[SerializeField] MinMax _colorFlapRange = new MinMax(0.5f , 1.0f);
	Vector3 _leftTempPos;
	Vector3 _rightTempPos;
	SpriteRenderer _leftSprite;
	SpriteRenderer _rightSprite;

	Timer _flapTimer = new Timer(1.0f);
	bool _open = false;
	bool _done = true;
	Color _color;
	float _tempColor;
	[SerializeField] AudioClip _openSound;
	[SerializeField] AudioClip _closeSound;
	AudioSource _audioSource;

	void Start () {
		_leftTempPos = _leftFlap.transform.localPosition;
		_rightTempPos = _rightFlap.transform.localPosition;
		_audioSource = gameObject.GetComponent<AudioSource> ();
		_leftSprite = _leftFlap.GetComponent<SpriteRenderer> ();
		_rightSprite = _rightFlap.GetComponent<SpriteRenderer> ();
		_color = _leftSprite.color;
	}
	
	void Update () {
		if (!_done) {
			if (_open) {
				_leftTempPos.x = MathHelpers.LinMapFrom01 (_leftFlapRange.Min, _leftFlapRange.Max, _flapTimer.PercentTimeLeft);
				_rightTempPos.x = MathHelpers.LinMapFrom01 (_rightFlapRange.Min, _rightFlapRange.Max, _flapTimer.PercentTimePassed);
				_tempColor = MathHelpers.LinMapFrom01 (_colorFlapRange.Min, _colorFlapRange.Max, _flapTimer.PercentTimeLeft);
			}
			else {
				_leftTempPos.x = MathHelpers.LinMapFrom01 (_leftFlapRange.Min, _leftFlapRange.Max, _flapTimer.PercentTimePassed);
				_rightTempPos.x = MathHelpers.LinMapFrom01 (_rightFlapRange.Min, _rightFlapRange.Max, _flapTimer.PercentTimeLeft);
				_tempColor = MathHelpers.LinMapFrom01 (_colorFlapRange.Min, _colorFlapRange.Max, _flapTimer.PercentTimePassed);
			}
			_color.r = _tempColor;
			_color.g = _tempColor;
			_color.b = _tempColor;
			_leftSprite.color = _color;
			_rightSprite.color = _color;

			_leftFlap.transform.localPosition = _leftTempPos;
			_rightFlap.transform.localPosition = _rightTempPos;
			if (_flapTimer.PercentTimePassed == 1.0f) {
				_done = true;
			}
		}

		if (Input.GetKeyDown (KeyCode.LeftBracket)) {
			_flapTimer.Reset ();
			_open = true;
			_done = false;
			_audioSource.clip = _openSound;
			_audioSource.Play ();
		}
		if (Input.GetKeyDown (KeyCode.RightBracket)) {
			_flapTimer.Reset ();
			_open = false;
			_done = false;
			_audioSource.clip = _closeSound;
			_audioSource.Play ();
		}
	}

	public void CloseFlap(){
		_flapTimer.Reset();
		_open = false;
		_done = false;
		_audioSource.clip = _closeSound;
		_audioSource.Play ();

	}

	public void OpenFlap(){
		_flapTimer.Reset ();
		_open = true;
		_done = false;
		_audioSource.clip = _openSound;
		_audioSource.Play ();
	}
}

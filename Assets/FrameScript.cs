using UnityEngine;
using System.Collections;

public class FrameScript : MonoBehaviour {
	[SerializeField] GameObject _leftFlap;
	[SerializeField] GameObject _rightFlap;
	[SerializeField] MinMax _leftFlapRange = new MinMax(-12.11f , -4.61f);
	[SerializeField] MinMax _rightFlapRange = new MinMax(4.46f , 11.96f);
	Vector3 _leftTempPos;
	Vector3 _rightTempPos;
//	[SerializeField] AnimationCurve _leftFlapCurve;
//	[SerializeField] AnimationCurve _rightFlapCurve;
	Timer _flapTimer = new Timer(1.0f);
	bool _open = false;

	[SerializeField] AudioClip _openSound;
	[SerializeField] AudioClip _closeSound;
	AudioSource _audioSource;

	// Use this for initialization
	void Start () {
		_leftTempPos = _leftFlap.transform.localPosition;
		_rightTempPos = _rightFlap.transform.localPosition;
		_audioSource = gameObject.GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
			if (_open) {
				_leftTempPos.x = MathHelpers.LinMapFrom01 (_leftFlapRange.Min, _leftFlapRange.Max, _flapTimer.PercentTimeLeft);
				_rightTempPos.x = MathHelpers.LinMapFrom01 (_rightFlapRange.Min, _rightFlapRange.Max, _flapTimer.PercentTimePassed);
			}
			else {
				_leftTempPos.x = MathHelpers.LinMapFrom01 (_leftFlapRange.Min, _leftFlapRange.Max, _flapTimer.PercentTimePassed);
				_rightTempPos.x = MathHelpers.LinMapFrom01 (_rightFlapRange.Min, _rightFlapRange.Max, _flapTimer.PercentTimeLeft);
			}
		_leftFlap.transform.localPosition = _leftTempPos;
		_rightFlap.transform.localPosition = _rightTempPos;

		if (Input.GetKeyDown (KeyCode.Q)) {
			_flapTimer.Reset ();
			_open = true;
			_audioSource.clip = _openSound;
			_audioSource.Play ();
		}
		if (Input.GetKeyDown (KeyCode.W)) {
			_flapTimer.Reset ();
			_open = false;
			_audioSource.clip = _closeSound;
			_audioSource.Play ();
		}
	}

	public void CloseFlap(){
		_flapTimer.Reset();
		_open = false;
		_audioSource.clip = _closeSound;
		_audioSource.Play ();

	}

	public void OpenFlap(){
		_flapTimer.Reset ();
		_open = true;
		_audioSource.clip = _openSound;
		_audioSource.Play ();
	}
}

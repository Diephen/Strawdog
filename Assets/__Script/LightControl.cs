using UnityEngine;
using System.Collections;

public class LightControl : MonoBehaviour {

	[SerializeField] AnimationCurve _flickerCurve;
	[SerializeField] MinMax _flickerRange = new MinMax(2.5f, 8f); 
	[SerializeField] MinMax _interactFlickerRange = new MinMax(4f, 8f); 
	bool flickerDone1 = true;
	bool flickerDone2 = true;
	float timer1 = 0f;
	float startTime1;
	float timer2 = 0f;
	float startTime2;
	const float _defaultDuration = 1.5f;
	float _flickerDuration1;
	float _flickerDuration2;
	Light _lightComponent1;
	Light _lightComponent2;

	[SerializeField] Light _directionalLight;
	[SerializeField] Color _originalDLight;
	[SerializeField] Color _interactDLight;
	Color _transitionalDLight;

	Timer _dLightTimer;
	float _dLightTransitionTime = 2.0f;
	bool _dLightInteractOn = false;
	bool _dLightInteractStart = false;
	[SerializeField] Light _prisonerLight;
	[SerializeField] Light _soldierLight;


	float _timeOn = 0.1f;
	float _timeOff = 0.05f;
	float _changeTime = 0;

	void Awake(){
		_dLightTimer = new Timer (_dLightTransitionTime);
	}

	void FixedUpdate() {
		if (!flickerDone1) {
			timer1 = (Time.time - startTime1) / _flickerDuration1;
			if (_dLightInteractOn) {
				_lightComponent1.intensity = MathHelpers.LinMapFrom01 (_interactFlickerRange.Min, _interactFlickerRange.Max, _flickerCurve.Evaluate (timer1));
			} else {
				_lightComponent1.intensity = MathHelpers.LinMapFrom01 (_flickerRange.Min, _flickerRange.Max, _flickerCurve.Evaluate (timer1));
			}
			//Debug.Log (_lightComponent1.intensity);
			if (timer1 >= 1f) {
				flickerDone1 = true;
			}
		}
		if (!flickerDone2) {
			timer2 = (Time.time - startTime2) / _flickerDuration2;
			if (_dLightInteractOn) {
				_lightComponent2.intensity = MathHelpers.LinMapFrom01 (_interactFlickerRange.Min, _interactFlickerRange.Max, _flickerCurve.Evaluate (timer2));
			} else {
				_lightComponent2.intensity = MathHelpers.LinMapFrom01 (_flickerRange.Min, _flickerRange.Max, _flickerCurve.Evaluate (timer2));
			}
			//Debug.Log (_lightComponent2.intensity);
			if (timer2 >= 1f) {
				flickerDone2 = true;
			}
		}

		if (_dLightInteractStart) {
			//directional Light Lerp (interaction)
			_dLightInteractOn = true;
			_transitionalDLight = Color.Lerp (_interactDLight, _originalDLight, _dLightTimer.PercentTimeLeft);
			_directionalLight.color = _transitionalDLight;

			_soldierLight.intensity = Mathf.Lerp (_interactFlickerRange.Min, _soldierLight.intensity, _dLightTimer.PercentTimeLeft);
			_prisonerLight.intensity = Mathf.Lerp (_interactFlickerRange.Min, _prisonerLight.intensity, _dLightTimer.PercentTimeLeft);

		} else if (!_dLightInteractStart && _dLightInteractOn){
			//directional Light Lerp (non-interaction)
			_transitionalDLight = Color.Lerp (_originalDLight, _transitionalDLight, _dLightTimer.PercentTimeLeft);
			_directionalLight.color = _transitionalDLight;

			_soldierLight.intensity = Mathf.Lerp (_flickerRange.Min, _soldierLight.intensity, _dLightTimer.PercentTimeLeft);
			_prisonerLight.intensity = Mathf.Lerp (_flickerRange.Min, _prisonerLight.intensity, _dLightTimer.PercentTimeLeft);

			if (_dLightTimer.IsOffCooldown) {
				_dLightInteractOn = false;
			}
		}



	}
		
	public void SpotlightFlicker(GameObject spotlight, float duration = _defaultDuration){	
		if (flickerDone1) {
			_flickerDuration1 = duration;
			flickerDone1 = false;
			timer1 = 0f;
			_lightComponent1 = spotlight.GetComponent <Light> ();
			startTime1 = Time.time;
		} else if (flickerDone2) {
			_flickerDuration2 = duration;
			flickerDone2 = false;
			timer2 = 0f;
			_lightComponent2 = spotlight.GetComponent <Light> ();
			startTime2 = Time.time;
		}
	}


	void OnEnable ()
	{
		Events.G.AddListener<GuardEngaginPrisonerEvent>(ChangeDirectionalLightColor);
	}

	void OnDisable ()
	{
		Events.G.RemoveListener<GuardEngaginPrisonerEvent>(ChangeDirectionalLightColor);
	}

	void ChangeDirectionalLightColor (GuardEngaginPrisonerEvent e)
	{
		_dLightTimer.Reset ();
		if (e.Engaged) {
			_dLightInteractStart = true;
		} else {
			_dLightInteractStart = false;
		}
	}

}

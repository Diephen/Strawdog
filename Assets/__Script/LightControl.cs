using UnityEngine;
using System.Collections;

public class LightControl : MonoBehaviour {

	[SerializeField] AnimationCurve _flickerCurve;
	[SerializeField] MinMax _flickerRange = new MinMax(2.5f, 8f); 
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

	void FixedUpdate() {
		if (!flickerDone1) {
			timer1 = (Time.time - startTime1) / _flickerDuration1;
			_lightComponent1.intensity = MathHelpers.LinMapFrom01 (_flickerRange.Min, _flickerRange.Max, _flickerCurve.Evaluate (timer1));
			//Debug.Log (_lightComponent1.intensity);
			if (timer1 >= 1f) {
				flickerDone1 = true;
			}
		}
		if (!flickerDone2) {
			timer2 = (Time.time - startTime2) / _flickerDuration2;
			_lightComponent2.intensity = MathHelpers.LinMapFrom01 (_flickerRange.Min, _flickerRange.Max, _flickerCurve.Evaluate (timer2));
			//Debug.Log (_lightComponent2.intensity);
			if (timer2 >= 1f) {
				flickerDone2 = true;
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
}

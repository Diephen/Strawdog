using UnityEngine;
using System.Collections;

public class LightControl : MonoBehaviour {

	[SerializeField] AnimationCurve _flickerCurve;
	[SerializeField] MinMax _flickerRange = new MinMax(2.5f, 6f); 
	bool flickerDone = true;
	float timer = 0f;
	float startTime;
	float lightIntensity;
	[SerializeField] float _flickerDuration = 2f;
	Light _lightComponent;

	void FixedUpdate() {
		if (!flickerDone) {
			timer = (Time.time - startTime) / _flickerDuration;
			Debug.Log ("Flickr");
			_lightComponent.intensity = MathHelpers.LinMapFrom01 (_flickerRange.Min, _flickerRange.Max, _flickerCurve.Evaluate (timer));
			if (timer >= 1f) {
				flickerDone = true;
			}
		}
	}

	public void SpotlightFlicker(GameObject spotlight){	
		if (flickerDone) {
			flickerDone = false;
			timer = 0f;
			_lightComponent = spotlight.GetComponent <Light> ();
			startTime = Time.time;
		}
	}
}

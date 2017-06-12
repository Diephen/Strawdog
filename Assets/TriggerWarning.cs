using UnityEngine;
using System.Collections;

public class TriggerWarning : MonoBehaviour {
	[SerializeField] float duration;
	[SerializeField] TextMesh _text;
	float _startTime;

	void Start () {
		_startTime = Time.time;
	}
	
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space)){
			Events.G.Raise (new LoadMainMenuEvent ());
			_text.color = Color.white;
		}
	}
}

using UnityEngine;
using System.Collections;

public class TutorialForceH : MonoBehaviour {
	[SerializeField] MeshRenderer _meshRenderer;
	[SerializeField] TextMesh _textmesh;
	Timer _nextTimer = new Timer(1.0f);
	MeshRenderer _thisMesh;
	bool _once = false;

	Color _tempTextColor;

	void Start(){
		Events.G.Raise (new DisableMoveEvent ());
		_tempTextColor = _textmesh.color;
		_thisMesh = GetComponent<MeshRenderer> ();
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.H) && !_once) {
			Events.G.Raise (new EnableMoveEvent ());
			_nextTimer.Reset ();
			_meshRenderer.enabled = false;
			_thisMesh.enabled = false;
			_once = true;
		}

		if (_once) {
			float tempAlpha = Mathf.Lerp (0.0f, 1.0f, _nextTimer.PercentTimePassed);
			_tempTextColor.a = tempAlpha;
			_textmesh.color = _tempTextColor;
		}

	}
}

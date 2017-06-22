using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallFadeTransition : MonoBehaviour {
	[SerializeField] bool _IsSimpleTransition = false;
	Renderer _renderer;
	float _tempAlpha;
	Color _tempColor;
	[SerializeField] SpriteRenderer[] _Spr;
	[SerializeField] BoxCollider2D[] _B2D;
	Animator _LiftAnimator;
	bool _isWallUp = false;


	// Use this for initialization
	void Start () {
		_Spr = gameObject.GetComponentsInChildren <SpriteRenderer> ();
		_B2D = gameObject.GetComponentsInChildren <BoxCollider2D> ();
		if (gameObject.GetComponent<Animator> ()) {
			_LiftAnimator = gameObject.GetComponent<Animator> ();
		}
		
	}

	void OnEnable ()
	{
		Events.G.AddListener<OfficeDoorEvent>(CloseOffice);
		Events.G.AddListener<WallTransitionEvent>(WallLift);
	}

	void OnDisable ()
	{
		Events.G.RemoveListener<OfficeDoorEvent>(CloseOffice);
		Events.G.RemoveListener<WallTransitionEvent>(WallLift);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void CloseOffice(OfficeDoorEvent e){
		if (!_IsSimpleTransition) {
			if (e.Opened) {
				//StartCoroutine (FadeOut ());
				EnterRoom();
			} else if (!e.Opened){
				//StartCoroutine (FadeIn ());
				ExitRoom();
			}
		}
	}


	public void FadeWall(){
		StartCoroutine (FadeOut ());
	}

	void EnterRoom(){
		// scene transition ot the inside area
		foreach(SpriteRenderer spr in _Spr){
			//print ("setfalse");
			spr.enabled = false;
		}
		foreach (BoxCollider2D b2d in _B2D) {
			b2d.enabled = false;
		}

	}

	void ExitRoom(){
		// scene transition to the outside area 
		foreach(SpriteRenderer spr in _Spr){
			spr.enabled = true;
		}
		foreach (BoxCollider2D b2d in _B2D) {
			b2d.enabled = true;
		}
	}

	IEnumerator FadeIn(){
		float startTime = Time.time;
		bool i = true;
		while (i) {
			_tempAlpha = MathHelpers.LinMapFrom01 (0.6f, 1.0f, 1.0f - (Time.time - startTime)/2f);
			_tempColor.a = _tempAlpha;
			_renderer.material.color = _tempColor;
			yield return null;
			if ((Time.time - startTime)/2f > 1.0f) {
				i = false;
			}
		}
	}

	IEnumerator FadeOut(){
		float startTime = Time.time;
		bool i = true;
		while (i) {
			_tempAlpha = MathHelpers.LinMapFrom01 (0.6f, 1.0f, (Time.time - startTime)/2f);
			_tempColor.a = _tempAlpha;
			_renderer.material.color = _tempColor;
			yield return null;
			if ((Time.time - startTime)/2f > 1.0f) {
				i = false;
			}
		}
	}

	// lift wall 
	void WallLift(WallTransitionEvent e){
		
		if (e.GoUp != _isWallUp && _IsSimpleTransition && _LiftAnimator!= null) {
			_isWallUp = e.GoUp;
			if (_isWallUp) {
				_LiftAnimator.Play ("Lift");
			} else {
				_LiftAnimator.Play ("Down");
			}
		}
	}

}

//using UnityEngine;
//using System.Collections;
//
//public class Detection : MonoBehaviour {
//
//	patrol _patrolScript;
//	bool _callOnce = false;
//
//	// Use this for initialization
//	void Start () {
//		_patrolScript = transform.parent.parent.gameObject.GetComponent<patrol> ();
//	}
//	
//	// Update is called once per frame
//	void Update () {
//
//	}
//
//	void OnTriggerEnter2D(Collider2D other) {
//		if (other.tag == "Prisoner") {
//			_patrolScript.StopAndLook ();
//		}
//	}
//
//	void OnTriggerExit2D(Collider2D other){
//		if (other.tag == "Prisoner") {
//			_patrolScript.CarryOn ();
//		}
//	}
//
//	void IsHidden(PrisonerHideEvent e){
//		if (e.Hidden) {
//			if (!_callOnce) {
//				_patrolScript.CarryOn ();
//				_callOnce = true;
//			}
//		}
//		else {
//			_callOnce = e.Hidden;
//		}
//	}
//
//	void OnEnable(){
//		Events.G.AddListener<PrisonerHideEvent>(IsHidden);
//	}
//	void OnDisable(){
//		Events.G.RemoveListener<PrisonerHideEvent>(IsHidden);
//	}
//}

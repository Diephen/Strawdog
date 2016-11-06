using UnityEngine;
using System.Collections;

public class FootstepHandle : MonoBehaviour {
	[SerializeField] AudioClip _footStep;
	[SerializeField] PuppetControl _guardPuppetControl;

	void OnTriggerEnter2d(Collider2D other){
		if (other.tag == "Guard") {
			Debug.Log ("SAD");
			_guardPuppetControl.m_Audio[0] = _footStep;
		}
	}
}

using UnityEngine;
using System.Collections;

public class GuardExecutionHandle : MonoBehaviour {
	[SerializeField] InteractionSound m_ItrSound;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void PlayReloadAfterShooting(){
		m_ItrSound.PlayReload ();
	}
}

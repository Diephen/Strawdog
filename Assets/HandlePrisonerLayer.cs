using UnityEngine;
using System.Collections;

public class HandlePrisonerLayer : MonoBehaviour {
	[SerializeField] BoxCollider2D _stopleft;
	BoxCollider2D _thisCol;

	// Use this for initialization
	void Start () {
		_thisCol = gameObject.GetComponent<BoxCollider2D> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void GuardLeft(LeftCellUnlockedEvent e){
		_thisCol.enabled = true;
		_stopleft.enabled = true;
	}

	void OnEnable(){
		Events.G.AddListener<LeftCellUnlockedEvent> (GuardLeft);
	}

	void OnDisable ()
	{
		Events.G.RemoveListener<LeftCellUnlockedEvent> (GuardLeft);
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Prisoner") {
			_stopleft.enabled = false;

			_thisCol.enabled = false;
		}
	}
}

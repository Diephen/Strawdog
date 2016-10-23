using UnityEngine;
using System.Collections;

public class HandlePrisonerLayer : MonoBehaviour {
	[SerializeField] BoxCollider2D _stopleft;
	BoxCollider2D _thisCol;
	[SerializeField] SpriteRenderer[] m_StringSprites;
	[SerializeField] BoxCollider2D m_BedCol;
	[SerializeField] WallFade m_Wall;

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
			SpriteRenderer[] sprites = other.gameObject.GetComponentsInChildren<SpriteRenderer> ();
			foreach (SpriteRenderer spr in sprites) {
				spr.sortingOrder = spr.sortingOrder + 10;
			}
			foreach (SpriteRenderer spr in m_StringSprites) {
				spr.sortingOrder = spr.sortingOrder + 10;
			}
			_stopleft.enabled = false;
			_thisCol.enabled = false;
			m_BedCol.enabled = false;
			m_Wall.FadeWall ();
			//Events.G.Raise (new LockCellEvent (true));
		}
	}
}

using UnityEngine;
using System.Collections;

public class SoldierDragHandle : MonoBehaviour {
	Animator m_Anim;
	[SerializeField] Transform m_SpawnPos;
	[SerializeField] float m_WalkSpeed;
	bool m_IsWalkToPrisoner = false;
	Transform m_PrisonerPos;
	bool m_IsPrisonerGetBomb = false;
	[SerializeField] SpriteRenderer m_GunSpr;

	bool m_IsCaught = false;

	void OnEnable(){
		Events.G.AddListener<CallGuardInCell> (OnPrisonerSleepInCell);
		Events.G.AddListener<Act2_SoldierAppear> (OnSoldierAppear);
		Events.G.AddListener<Act2_PrisonerGetBomb> (OnPrisonerGetsBomb);
	}

	void OnDisable(){
		Events.G.RemoveListener<CallGuardInCell> (OnPrisonerSleepInCell);
		Events.G.RemoveListener<Act2_SoldierAppear> (OnSoldierAppear);
		Events.G.RemoveListener<Act2_PrisonerGetBomb> (OnPrisonerGetsBomb);
	}

	//

	// Use this for initialization
	void Start () {
		m_Anim = GetComponent<Animator> ();
		m_PrisonerPos = FindObjectOfType<Cell_PrisonerTrigger> ().gameObject.transform;
	}
	
	// Update is called once per frame
	void Update () {
		if (m_IsWalkToPrisoner) {
			print ("Soldier Walking" + Mathf.Abs (m_PrisonerPos.position.x - transform.position.x));
			if (Mathf.Abs (m_PrisonerPos.position.x - transform.position.x) > 5f) {
				print ("Soldier Walking");
				Vector3 npos = transform.position;
				npos.x += m_WalkSpeed * Time.deltaTime;
				transform.position = npos;
			} else {
				if (!m_IsCaught) {
					DragPrisonerExplore ();
					m_IsCaught = true;
				}

			}
		}

	}

	void OnSoldierAppear(Act2_SoldierAppear e){
		print ("Soldier appear on the way back");
		transform.position = m_SpawnPos.position;
		//m_Anim.Play ("soldier-exp-Idle");
	}

	public void WalkToPrisoner(){
		m_Anim.Play ("soldier-exp-Walk");
		m_IsWalkToPrisoner = true;

	}

	void OnPrisonerSleepInCell(CallGuardInCell e){
		print ("Soldier Walks in");
		m_Anim.Play ("soldier-jc-drag");


	}

	void DragPrisoner(){
		Events.G.Raise (new DragPrisonerInJail ());
	}

	void DragPrisonerExplore(){
		Events.G.Raise (new Act2_SoldierDragPrisonerExplore ());
		//m_Anim.Play ("soldier-exp-Drag");
		m_Anim.Play ("soldier-exp-PointGun");

	}

	void OnEndJailSequence(){
		// fire end scene event
		Events.G.Raise(new PrisonerSleepEvent());
	}

	void OnEndExploreSequence(){
		if (m_IsPrisonerGetBomb) {
			Events.G.Raise (new PrisonerFoundBombEvent ());
		} else {
			Events.G.Raise (new PrisonerWentBack ());
		}
	}

	void OnPrisonerGetsBomb(Act2_PrisonerGetBomb e){
		m_IsPrisonerGetBomb = true;
	}

	void GunFront(){
		m_GunSpr.sortingOrder += 3;
	}
		
}

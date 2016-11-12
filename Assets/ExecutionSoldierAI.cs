using UnityEngine;
using System.Collections;

public class ExecutionSoldierAI : MonoBehaviour {
	Animator m_Anim;
	[SerializeField] ExecutionGuardHandle m_GuardHandle;
	[SerializeField] ExecutionPrisonerHandle m_PrisonerHandle;
	[SerializeField] GameObject m_Prisoner;
	[SerializeField] SpriteRenderer m_GunSprite;
	[SerializeField] float m_Speed;
	[SerializeField] float WaitToCatchDuration = 1f;
	[SerializeField] float CatchDuration = 3f;
	[SerializeField] float WaitToShootBoth = 10f;
	GameObject m_FlashLight;
	Timer m_CatchTimer;
	Timer m_EncounterTimer;

	bool m_IsPrisonerStray;
	bool m_IsCatch = false;
	bool m_IsGuardEncounter = false;
	bool m_IsSwitchToGuard = false;
	float step;
	// Use this for initialization
	void Start () {
		m_CatchTimer = new Timer (WaitToCatchDuration);
		m_EncounterTimer = new Timer (WaitToShootBoth);
		m_Anim = GetComponent<Animator> ();
		//m_FlashLight = GameObject.FindObjectOfType<rotateTowards> ();
	}

	void OnEnable ()
	{
		Events.G.AddListener<AboutToStrayOutOfLineEvent>(OnPrisonerStray);
		Events.G.AddListener<ExecutionBreakFree>(OnPrisonerBreakFree);
		Events.G.AddListener<ShootSwitchEvent> (OnSwitchToGuard);
		Events.G.AddListener<ExecutionEncounter> (OnEncounter);
	}

	void OnDisable ()
	{
		Events.G.RemoveListener<AboutToStrayOutOfLineEvent> (OnPrisonerStray);
		Events.G.RemoveListener<ExecutionBreakFree>(OnPrisonerBreakFree);
		Events.G.RemoveListener<ShootSwitchEvent> (OnSwitchToGuard);
		Events.G.AddListener<ExecutionEncounter> (OnEncounter);
	}

	void OnSwitchToGuard(ShootSwitchEvent e){
		m_IsSwitchToGuard = true;
		m_EncounterTimer.Reset ();
	}

	void OnEncounter(ExecutionEncounter e){
		if (e.ExeType == ExecutionType.Prisoner && !m_IsGuardEncounter) {
			m_IsGuardEncounter = true;
		}
	}

	void OnPrisonerBreakFree(ExecutionBreakFree e){
		m_Anim.Play ("soldier-exe-Alert");
	}


	void OnPrisonerStray(AboutToStrayOutOfLineEvent e){
		//m_IsCatch = true;
		if(!m_IsSwitchToGuard && !m_IsGuardEncounter){
			m_IsPrisonerStray = e.Straying;
			if (m_IsPrisonerStray) {
				m_CatchTimer.Reset ();
			}
		}
	}

	void MoveToPrisoner(){
		print ("catching");
		Vector3 FinalPos = transform.position;

		FinalPos.x = m_Prisoner.transform.position.x;
		//transform.position = FinalPos;
		//Vector3.MoveTowards (transform.position, FinalPos, step);

		if (Mathf.Abs (transform.position.x - FinalPos.x) > 2f) {
			transform.position = Vector3.MoveTowards (transform.position, FinalPos, step);
		} else {
			print ("call reset event");
			m_IsCatch = false;
			Events.G.Raise (new RestartExecution ());
		}
	}
		
	
	// Update is called once per frame
	void Update () {
		if (!m_IsCatch && m_CatchTimer.IsOffCooldown && m_IsPrisonerStray) {
			print ("restart level");
			m_IsCatch = true;
			// end of scene sequence
			Events.G.Raise (new DisableMoveEvent (CharacterIdentity.Both));
			step = Time.deltaTime * Mathf.Abs (m_Prisoner.transform.position.x - transform.position.x) / CatchDuration;
		}

		if (m_IsCatch) {
			MoveToPrisoner ();
		}

		if (m_IsGuardEncounter && m_EncounterTimer.IsOffCooldown) {
			// Shooting animation
			ShootBoth();
			m_IsGuardEncounter = false;

			
		}
	
	}

	void ShootBoth(){
		m_Anim.Play ("soldier-exe-ShootLeft");
	}

	void PrisonerDie(){
		m_PrisonerHandle.Death ();
	}

	void GuardDie(){
		m_GuardHandle.Death ();
	}

	void EndOfExecution(){
		Events.G.Raise (new SoldierExecuteBoth ());
	}

	void GunFront(){
		m_GunSprite.sortingOrder += 1;
	}
}

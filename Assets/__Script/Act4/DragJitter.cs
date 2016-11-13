using UnityEngine;
using System.Collections;

public class DragJitter : MonoBehaviour {
	[SerializeField] AnimationCurve _joltCurve;
	[SerializeField] float _pushOffset = 4f;

	PuppetControl _prisonerPuppetControl;
	KeyCode[] _prisonerKeyCodes;
	Timer _pushTimer;

	AudioSource _jitterSource;

	[SerializeField] bool _isThePrisoner;
	[SerializeField] bool _isExecution = false;
	int _inputCount = 0;
	int _freedomCount = 20;
	bool isJitterEnabled = true;
	bool isLineStop = false;
	bool isPushBack = false;
	float m_PushBackSpeed;

	[SerializeField] AudioClip[] _ropeTug;
	[SerializeField] AudioClip _ropeBreak;
	Transform m_PrisonerTrans;

	Vector3 m_PushbackPos;

	// Use this for initialization
	void Start () {
		_pushTimer = new Timer (3.0f);
		_pushTimer.CooldownTime = _pushOffset;

		if (_isThePrisoner) {
			_prisonerPuppetControl = gameObject.GetComponent <PuppetControl> ();
			_prisonerKeyCodes = _prisonerPuppetControl.GetKeyCodes ();
		}
		_jitterSource = gameObject.AddComponent<AudioSource> ();
		_jitterSource.volume = 0.7f;
		if (GameObject.FindObjectOfType<DitchPrisonerHandle> () != null) {
			m_PrisonerTrans = GameObject.FindObjectOfType<DitchPrisonerHandle> ().transform;
		} else {
			
		}

	}

	void FixedUpdate(){
		if (isLineStop && isPushBack) {
			PushBack ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!_isExecution && !isLineStop) {
			// add the walking code here 
			gameObject.transform.Translate (Vector2.left * Time.deltaTime * _joltCurve.Evaluate (_pushTimer.PercentTimePassed) * 5.0f);
			if (_pushTimer.IsOffCooldown) {
				_pushTimer.Reset ();
			}
		}


		if (_isThePrisoner && isJitterEnabled) {
			if (Input.GetKeyDown (_prisonerKeyCodes [0]) ||
			   Input.GetKeyDown (_prisonerKeyCodes [1]) ||
			   Input.GetKeyDown (_prisonerKeyCodes [2]) ||
			   Input.GetKeyDown (_prisonerKeyCodes [3])) {

				if (!_jitterSource.isPlaying) {
					_jitterSource.clip = _ropeTug[Random.Range (0, 5)];
					_jitterSource.Play ();
				}
				_inputCount++;
			}
		}

		if (_inputCount > _freedomCount) {
			_jitterSource.clip = _ropeBreak;
			_jitterSource.Play ();
			_prisonerPuppetControl.EnableContinuousWalk ();
			if (!_isExecution) {
				Events.G.Raise (new BrokeFree ());
			} else {
				Events.G.Raise (new ExecutionBreakFree ());
			}
			Events.G.Raise (new EnableMoveEvent (CharacterIdentity.Prisoner));
			this.enabled = false;
		}
	}

	void OnEnable(){
		Events.G.AddListener<LineControlEvent> (OnLineControl);
		Events.G.AddListener<CutPrisonerBrforeOthers> (OnPrisonerCut);
	}

	void OnDisable(){
		Events.G.RemoveListener<LineControlEvent> (OnLineControl);
		Events.G.RemoveListener<CutPrisonerBrforeOthers> (OnPrisonerCut);
	}

	public void DisableJitter(){
		isJitterEnabled = false;
	}

	void OnLineControl(LineControlEvent e){
		if (e.IsStop) {
			LineStop ();
		}else{
			LineResume ();
		}
	
	}

	void LineStop(){
		print ("Stop Jitter");
		isLineStop = true;
	}

	void LineResume(){
		print ("Resume Jitter");
		isLineStop = false;
	}

	void OnPrisonerCut(CutPrisonerBrforeOthers e){
		print ("Make room for prisoner");
		m_PushbackPos = transform.position;
		print (m_PrisonerTrans.position.x + 5f);
		//m_PushbackPos.x = m_PushbackPos.x - m_PrisonerTrans.position.x + 5f;
		m_PushbackPos.x += 3f;
		print ("current: " + transform.position.x + " Target: " + m_PushbackPos.x );
		isPushBack = true;
		m_PushBackSpeed = Time.deltaTime * Mathf.Abs (transform.position.x - m_PushbackPos.x);
		if (!isLineStop) {
			isLineStop = true;
		}
	}

	void PushBack(){
		if (Mathf.Abs(transform.position.x - m_PushbackPos.x)>0) {
			print ("Make room for prisoner");
			transform.position = Vector3.MoveTowards (transform.position, m_PushbackPos, m_PushBackSpeed);
		} else {
			isPushBack = false;
		}
	}
}

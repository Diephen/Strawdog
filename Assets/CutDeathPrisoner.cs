using UnityEngine;
using System.Collections;

public class CutDeathPrisoner : MonoBehaviour {
	Animator m_Anim;
	[SerializeField] AnimationControl m_AnimControl;
	SpriteRenderer[] m_Sprites;
	BoxCollider2D[] m_BoxColls;
	Rigidbody2D[] m_Rigidy;
	[SerializeField] HingeJoint2D m_StringJoint;
	[SerializeField] Vector2 m_Force;
	[SerializeField] DragJitter m_Drag;
	[SerializeField] Rigidbody2D m_WholeBodyRig;
	Timer m_DestoryTimer;
	bool isDead = false;
	// Use this for initialization
	void Awake(){
		
		m_Anim = GetComponent<Animator> ();
		m_Sprites = GetComponentsInChildren<SpriteRenderer> ();
		m_BoxColls = this.gameObject.GetComponentsInChildren<BoxCollider2D> ();
		m_Rigidy = new Rigidbody2D[m_BoxColls.Length];
		for (int i = 0; i < m_BoxColls.Length; i++) {
			m_Rigidy [i] = m_BoxColls [i].gameObject.GetComponent<Rigidbody2D> ();
			m_BoxColls [i].enabled = false;
		}
		//m_WholeBodyColl.enabled = false;
		m_Sprites = GetComponentsInChildren<SpriteRenderer>();
		m_DestoryTimer = new Timer (15f);

	}

	void Start () {
		if (m_AnimControl == null) {
			print ("WTF");
			m_AnimControl = GetComponent<AnimationControl> ();
		}

	}
	
	// Update is called once per frame
	void Update () {
		#if UNITY_EDITOR
		if(Input.GetKeyDown(KeyCode.LeftShift)){
			Death();
		}
		#endif
		if (m_DestoryTimer.IsOffCooldown && isDead) {
			Destroy (gameObject);
		}
	}

	public void Death(){
		print ("Play death animation");
		m_AnimControl.SetAnimation (true);
		m_Anim.Play ("cp-ditch-Death");
		m_Drag.enabled = false;
		m_WholeBodyRig.AddForce (m_Force);
	}

	public void EndAnimation(){
		m_StringJoint.enabled = false;
		m_AnimControl.SetAnimation (false);
		m_StringJoint.gameObject.GetComponent<Rigidbody2D> ().AddForce (m_Force);
		foreach (Rigidbody2D rig in m_Rigidy) {
			rig.AddForce (m_Force);
		}
		foreach (BoxCollider2D bc in m_BoxColls) {
			bc.enabled = true;
		}
		isDead = true;
		m_DestoryTimer.Reset ();
	
	}

	void FallInDitch(){
		foreach (SpriteRenderer spr in m_Sprites) {
			spr.sortingOrder -= 6;
		}
	}

}

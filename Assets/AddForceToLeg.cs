using UnityEngine;
using System.Collections;

public class AddForceToLeg : MonoBehaviour {
	[SerializeField] CharacterIdentity _whoAmI;
	Rigidbody2D rb;
	bool drag = false;
	bool left = false;
	float force = 10.0f;
	// Use this for initialization
	void Start () {
		rb = gameObject.GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(drag){
			if (left) {
				rb.AddForce (Vector2.left * force);
			}
			else {
				rb.AddForce (Vector2.right * force);
			}

		}
	}

	void DragLegs(IsWalkingEvent e){
		if (e.WhoAmI == _whoAmI) {
			drag = e.IsWalking;
			left = e.IsLeft;
		}
	}

	void OnEnable(){
		Events.G.AddListener<IsWalkingEvent> (DragLegs);
	}

	void OnDisable ()
	{
		Events.G.RemoveListener<IsWalkingEvent>(DragLegs);
	}
}

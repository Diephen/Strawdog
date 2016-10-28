using UnityEngine;
using System.Collections;

public class AddForceToLeg : MonoBehaviour {
	[SerializeField] CharacterIdentity _whoAmI;
	Rigidbody2D rb;
	bool drag = false;
	bool left = false;
	MinMax _forceRange = new MinMax(8.0f, 12.0f);
	float force = 10.0f;
	[SerializeField] bool flip;

	void Start () {
		rb = gameObject.GetComponent<Rigidbody2D> ();
	}
	
	void FixedUpdate () {
		if(drag){
			force = MathHelpers.LinMapFrom01 (_forceRange.Min, _forceRange.Max, Random.value);
			if (flip) {
				if (left) {
					rb.AddForce (Vector2.left * force);
				}
				else {
					rb.AddForce (Vector2.right * force);
				}
			}
			else {
				if (left) {
					rb.AddForce (Vector2.right * force);
				}
				else {
					rb.AddForce (Vector2.left * force);
				}
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

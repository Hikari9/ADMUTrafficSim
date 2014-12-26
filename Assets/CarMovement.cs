using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class CarMovement : MonoBehaviour {

	Vector3 acceleration = new Vector3 (20f, 0f, 0f);
	float targetVelocity = 15f;

	// Use this for initialization
	void Start () {
		// Debug.Log (this.transform.transform.rotation);
	}

	public static bool OutOfBounds(Vector3 pos) {
		// Debug.Log ("Checking bounds + " + pos);
		return (Mathf.Max (new float[]{Mathf.Abs (pos.x), Mathf.Abs (pos.y), Mathf.Abs (pos.z)}) > 100);
	}
	
	// Update is called once per frame
	void Update () {
		Rigidbody rb = transform.rigidbody;
		if (rb.velocity.magnitude < this.targetVelocity) {
			rb.AddRelativeForce (this.acceleration);
		}
		if (OutOfBounds (this.gameObject.transform.position))
			Destroy (this.gameObject);
	}
}

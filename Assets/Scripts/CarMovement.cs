using UnityEngine;
using System.Collections;

public class CarMovement : MonoBehaviour {

	public float acceleration = 5f;
	public Vector3 targetVelocity = new Vector3 (40f, 0f, 0f);
	Vector3 originalTargetVelocity;
	// Use this for initialization
	void Start () {
		originalTargetVelocity = targetVelocity;
		// Debug.Log (this.transform.transform.rotation);
	}

	public static bool OutOfBounds(Vector3 pos) {
		// Debug.Log ("Checking bounds + " + pos);
		return (Mathf.Max (new float[]{Mathf.Abs (pos.x), Mathf.Abs (pos.y), Mathf.Abs (pos.z)}) > 100);
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 localVelocity = transform.InverseTransformDirection (rigidbody.velocity);
		Vector3 need = targetVelocity - localVelocity;
		Vector3 addend = need * this.acceleration * Time.deltaTime;
		rigidbody.velocity += transform.TransformDirection (addend);
		// Debug.Log (rigidbody.velocity);
		if (OutOfBounds (this.gameObject.transform.position)) {
			// Debug.Log ("OUT");
			CarSpawner.Main.DestroyCar(this.gameObject);
		}
	}

	public void OnTriggerStay(Collider collision) {
		// slow down car to avoid collision
		// Debug.Log ("Enter collision with: " + collision);
		Transform car1 = this.transform;
		Transform car2 = collision.transform;
		if (Mathf.Abs (Vector3.Dot (car1.position, car2.position)) < 1e-6f) {
			if (car1.position.sqrMagnitude < car2.position.sqrMagnitude)
				this.GetComponentInParent<CarMovement> ().targetVelocity = Vector3.zero;
		}
		else
			if (car1.position.sqrMagnitude > car2.position.sqrMagnitude)
				this.GetComponentInParent<CarMovement>().targetVelocity = Vector3.zero;
	}
	public void OnTriggerExit(Collider collision) {
		// Debug.Log ("Exit collision with: " + collision);
		Transform car1 = this.transform;
		Transform car2 = collision.transform;
		car1.GetComponentInParent<CarMovement> ().targetVelocity = car1.GetComponentInParent<CarMovement> ().originalTargetVelocity;
		car2.GetComponentInParent<CarMovement> ().targetVelocity = car2.GetComponentInParent<CarMovement> ().originalTargetVelocity;
	}
}

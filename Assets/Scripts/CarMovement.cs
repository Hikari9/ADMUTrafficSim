using UnityEngine;
using System.Collections;

public class CarMovement : MonoBehaviour {

	public float acceleration = 15f;
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

	public void OnTriggerEnter(Collider collision) {
		OnTriggerStay (collision);
	}

	public void OnTriggerStay(Collider collision) {
		// slow down car to avoid collision
		// Debug.Log ("Enter collision: " + this.transform.parent.rotation.eulerAngles.y);
		Transform car1 = this.transform;
		Transform car2 = collision.transform;
		if (car1.localPosition.z > car2.localPosition.z)
			this.GetComponent<CarMovement>().targetVelocity = Vector3.zero;
		else if (car2.rigidbody.IsSleeping())
			car1.GetComponent<CarMovement> ().targetVelocity = originalTargetVelocity;
		if (Mathf.Abs (car1.parent.rotation.eulerAngles.y - car2.parent.eulerAngles.y) > 1e-6f && car2.rigidbody.IsSleeping ())
			car1.GetComponent<CarMovement> ().targetVelocity = originalTargetVelocity;
	}
	public void OnTriggerExit(Collider collision) {
		// Debug.Log ("Exit collision with: " + collision);
		Transform car1 = this.transform;
		Transform car2 = collision.transform;
		car1.GetComponent<CarMovement> ().targetVelocity = car1.GetComponent<CarMovement> ().originalTargetVelocity;
		car2.GetComponent<CarMovement> ().targetVelocity = car2.GetComponent<CarMovement> ().originalTargetVelocity;
	}
}

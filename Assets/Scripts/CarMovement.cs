using UnityEngine;
using System.Collections;

public class CarMovement : MonoBehaviour {

	public float acceleration = 3f;
	public Vector3 targetVelocity = new Vector3 (10f, 0f, 0f);
	Vector3 originalTargetVelocity;

	public const int STOP = 0, GO = 1, NORMAL = -1;
	public int movement = NORMAL;
	int prevMovement;

	// Use this for initialization
	void Start () {
		prevMovement = movement;
		originalTargetVelocity = targetVelocity;
		// Debug.Log (this.transform.transform.rotation);
	}

	public static bool OutOfBounds(Vector3 pos) {
		// Debug.Log ("Checking bounds + " + pos);
		return (Mathf.Max (new float[]{Mathf.Abs (pos.x), Mathf.Abs (pos.y), Mathf.Abs (pos.z)}) > GameObject.FindGameObjectWithTag("GameMaster").GetComponent<CarSpawner>().DEFAULT_NORTH_POSITION.magnitude + 10);
	}

	// Update is called once per frame
	void Update () {
		Vector3 localVelocity = transform.InverseTransformDirection (rigidbody.velocity);
		Vector3 need = targetVelocity - localVelocity;
		Vector3 addend = need * Mathf.Min (1f, Time.deltaTime * acceleration);
		rigidbody.velocity += transform.TransformDirection (addend);
		// Debug.Log (rigidbody.velocity);
		if (OutOfBounds (this.gameObject.transform.position)) {
			// Debug.Log ("OUT");
			GameObject.FindGameObjectWithTag ("GameMaster").GetComponent<CarSpawner> ().DestroyCar (this.gameObject);
		}
	}

	public void SetToOriginal() {
		targetVelocity = originalTargetVelocity;
	}

	/*
	public void OnTriggerEnter(Collider collision) {
		OnTriggerStay (collision);
	}*/

	public void OnTriggerStay(Collider collision) {
		if (movement == STOP) return;
		// slow down car to avoid collision
		// Debug.Log ("Enter collision: " + this.transform.parent.rotation.eulerAngles.y);
		Transform car1 = this.transform;
		Transform car2 = collision.transform;
		// if (car1.localPosition - car1.GetComponent<CarMovement>(). > car2.localPosition.sqrMagnitude)
		if (car1.localPosition.z >= car2.localPosition.z)
			this.GetComponent<CarMovement>().targetVelocity = Vector3.zero;
		else if (Mathf.Abs (car1.parent.rotation.eulerAngles.y - car2.parent.eulerAngles.y) > 1e-6f && car2.rigidbody.IsSleeping ())
			car1.GetComponent<CarMovement> ().targetVelocity = car1.GetComponent<CarMovement> ().originalTargetVelocity;
	}
	public void OnTriggerExit(Collider collision) {
		// Debug.Log ("Exit collision with: " + collision);
		Transform car1 = this.transform;
		Transform car2 = collision.transform;
		if (car1.GetComponent<CarMovement>().movement != CarMovement.STOP)
			car1.GetComponent<CarMovement> ().targetVelocity = car1.GetComponent<CarMovement> ().originalTargetVelocity;
		if (car2.GetComponent<CarMovement>().movement != CarMovement.STOP)
			car2.GetComponent<CarMovement> ().targetVelocity = car2.GetComponent<CarMovement> ().originalTargetVelocity;
	}
}

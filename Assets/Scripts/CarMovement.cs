using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CarMovement : MonoBehaviour {

	public float acceleration = 3f;
	public Vector3 targetVelocity = new Vector3 (10f, 0f, 0f);
	Vector3 originalTargetVelocity;

	public const int STOP = 0, GO = 1, NORMAL = -1;
	public int movement = NORMAL;
	// int prevMovement;

	// Use this for initialization
	void Start () {
		// prevMovement = movement;
		originalTargetVelocity = targetVelocity;
		// Debug.Log (this.transform.transform.rotation);
	}

	public static bool OutOfBounds(Vector3 pos) {
		// Debug.Log ("Checking bounds + " + pos);
		// return (Mathf.Max (new float[]{Mathf.Abs (pos.x), Mathf.Abs (pos.y), Mathf.Abs (pos.z)}) > GameObject.FindGameObjectWithTag("GameMaster").GetComponent<CarSpawner>().DEFAULT_NORTH_POSITION.magnitude + 10);
		return pos.z < -100f;
	}

	// Update is called once per frame
	void Update () {
		if (movement == STOP) {
			if (transform.localPosition.z < 10)
				movement = GO;
			else
				targetVelocity = Vector3.zero;
		}
		if (movement == GO) {
			targetVelocity = originalTargetVelocity;
			// movement = NORMAL;
		}  
		Vector3 localVelocity = transform.InverseTransformDirection (GetComponent<Rigidbody>().velocity);
		Vector3 need = targetVelocity - localVelocity;
		Vector3 addend = need * Mathf.Min (1f, Time.deltaTime * acceleration);
		GetComponent<Rigidbody>().velocity += transform.TransformDirection (addend);
		// Debug.Log (rigidbody.velocity);
		if (OutOfBounds (transform.localPosition)) {
			// Debug.Log ("OUT");
			Command.GetCarSpawner ().DestroyCar (this.gameObject);
		}
		if (transform.localPosition.z < -10) {
			Command.GetCarSpawner ().PassCar (this.gameObject);
		}
	}

	public void SetToOriginal() {
		targetVelocity = originalTargetVelocity;
	}

	/*
	public void OnTriggerEnter(Collider collision) {
		OnTriggerStay (collision);
	}*/

	public void OnCollisionEnter(Collision collision) {
		
		if (collision.collider.tag == "car") {
			// not same road. add collision
			if (Mathf.Abs (this.transform.parent.rotation.eulerAngles.y - collision.transform.parent.eulerAngles.y) > 1e-6f)
				GameObject.FindGameObjectWithTag ("GameMaster").GetComponent<GameGUI> ().AddCollision ();

			Command.GetCarSpawner ().DestroyCar (collision.collider.gameObject);
			Command.GetCarSpawner ().DestroyCar (this.gameObject);
		}
	}
	


	public void OnTriggerStay(Collider collision) {
		if (movement != NORMAL) return;
		// slow down car to avoid collision
		// Debug.Log ("Enter collision: " + this.transform.parent.rotation.eulerAngles.y);
		Transform car1 = this.transform;
		Transform car2 = collision.transform;
		if (car1 == null || car2 == null || car2.GetComponent<CarMovement> () == null) return;
		// if (car1.localPosition - car1.GetComponent<CarMovement>(). > car2.localPosition.sqrMagnitude)
		if (Mathf.Abs (car1.parent.rotation.eulerAngles.y - car2.parent.eulerAngles.y) > 1e-6f && car2.GetComponent<Rigidbody>().IsSleeping ())
			car1.GetComponent<CarMovement> ().targetVelocity = car1.GetComponent<CarMovement> ().originalTargetVelocity;
		else if (car1.localPosition.z >= car2.localPosition.z) {
			this.targetVelocity = Vector3.zero;
			if (car2.GetComponent<Rigidbody>().IsSleeping () && Mathf.Abs (car1.parent.rotation.eulerAngles.y - car2.parent.eulerAngles.y) < 1e-6f) {
				// this.targetVelocity = this.originalTargetVelocity;
				car2.GetComponent<CarMovement> ().targetVelocity = car2.GetComponent<CarMovement> ().originalTargetVelocity;
			}
		}
	}
	public void OnTriggerExit(Collider collision) {
		// Debug.Log ("Exit collision with: " + collision);
		Transform car1 = this.transform;
		Transform car2 = collision.transform;
		if (car1 == null || car2 == null || car2.GetComponent<CarMovement> () == null) return;
		if (car1.GetComponent<CarMovement>().movement != CarMovement.STOP)
			car1.GetComponent<CarMovement> ().targetVelocity = car1.GetComponent<CarMovement> ().originalTargetVelocity;
		if (car2.GetComponent<CarMovement>().movement != CarMovement.STOP)
			car2.GetComponent<CarMovement> ().targetVelocity = car2.GetComponent<CarMovement> ().originalTargetVelocity;
	}
}

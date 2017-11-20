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
			if (OrthogonalCars(this.transform, collision.transform))
				GameObject.FindGameObjectWithTag ("GameMaster").GetComponent<GameGUI> ().AddCollision ();
			Command.GetCarSpawner ().DestroyCar (collision.collider.gameObject);
			Command.GetCarSpawner ().DestroyCar (this.gameObject);
		}
	}

    string spawnLabel;

    public string GetSpawnLabel() {
        return spawnLabel;
    }

    public void SetSpawnLabel(string spawnLabel) {
        this.spawnLabel = spawnLabel;
    }

    public int GetSpawnDirection() {
        return GetSpawnLabel() == "North" || GetSpawnLabel() == "South" ? 1 : 0;
    }

    public bool IsSleeping() {
        return GetComponent<Rigidbody>().IsSleeping() || Mathf.Abs(GetComponent<Rigidbody>().velocity.sqrMagnitude) < 1e-12f;
    }

    public static bool OrthogonalCars(Transform car1, Transform car2) {
        if (car1 == null || car2 == null)
            return false;
        CarMovement car1m = car1.GetComponent<CarMovement>();
        CarMovement car2m = car2.GetComponent<CarMovement>();
        if (car1m == null || car2m == null)
            return false;
        return car1m.GetSpawnDirection() != car2m.GetSpawnDirection();
    }

    public static bool CarIsBehind(Transform car1, Transform car2) {
        if (car1 == null || car2 == null)
            return false;
        CarMovement car1m = car1.GetComponent<CarMovement>();
        CarMovement car2m = car2.GetComponent<CarMovement>();
        if (car1m == null || car2m == null)
            return false;
        if (car1m.GetSpawnLabel() != car2m.GetSpawnLabel())
            return false;
        return car1m.transform.localPosition.z < car2m.transform.localPosition.z;
    }

    public Dictionary<CarMovement, bool> carsInFront = new Dictionary<CarMovement, bool>();
    public Dictionary<CarMovement, bool> carsAcross = new Dictionary<CarMovement, bool>();

    public void OnTriggerEnter(Collider collision) {
        Transform car1 = this.transform;
        Transform car2 = collision.transform;
        if (OrthogonalCars(car1, car2) && !carsAcross.ContainsKey(car2.GetComponent<CarMovement>())) {
            carsAcross.Add(car2.GetComponent<CarMovement>(), true);
        }
        if (CarIsBehind(car1, car2) && !carsInFront.ContainsKey(car2.GetComponent<CarMovement>())) {
            carsInFront.Add(car2.GetComponent<CarMovement>(), true);
        }
    }
	
	public void OnTriggerStay(Collider collision) {
		if (movement != NORMAL) return;
		Transform car1 = this.transform;
		Transform car2 = collision.transform;
        if (car1 == null || car2 == null)
            return;
        CarMovement car1m = car1.GetComponent<CarMovement>();
        CarMovement car2m = car2.GetComponent<CarMovement>();
        if (car1m == null || car2m == null)
            return;
        if (OrthogonalCars(car1, car2)) {
            if (car2m.targetVelocity.Equals(Vector3.zero))
                car1m.targetVelocity = car1m.originalTargetVelocity;
            else if (car1.localPosition.z > car2.localPosition.z) { // not in front
                car1m.targetVelocity = Vector3.zero;
                if (car2m.IsSleeping()) {
                    car2m.targetVelocity = car2m.originalTargetVelocity;
                }
            }
        } else {
            if (CarIsBehind(car1, car2)) {
                car1m.targetVelocity = Vector3.zero;
            }
        }
	}

	public void OnTriggerExit(Collider collision) {
        // Debug.Log ("Exit collision with: " + collision);
        Transform car1 = this.transform;
        Transform car2 = collision.transform;
        CarMovement car2m = car2.GetComponent<CarMovement>();
        if (car2m != null && carsAcross.ContainsKey(car2m)) {
            carsAcross.Remove(car2m);
        }
        if (car2m != null && carsInFront.ContainsKey(car2m)) {
            carsInFront.Remove(car2m);
        }
        if (car1.GetComponent<CarMovement>().movement != CarMovement.STOP)
			car1.GetComponent<CarMovement> ().targetVelocity = car1.GetComponent<CarMovement> ().originalTargetVelocity;
		if (car2m != null && car2.GetComponent<CarMovement>().movement != CarMovement.STOP)
			car2.GetComponent<CarMovement> ().targetVelocity = car2.GetComponent<CarMovement> ().originalTargetVelocity;
	}
}

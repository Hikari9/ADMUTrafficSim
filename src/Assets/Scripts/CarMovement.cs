using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CarMovement : MonoBehaviour {

	public float acceleration = 3f;
	public Vector3 targetVelocity = new Vector3 (10f, 0f, 0f);
	public Vector3 originalTargetVelocity;

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

    public bool IsBeforeIntersection() {
        return transform.localPosition.z > 27f;
    }

    // Update is called once per frame
    void Update () {

        // change target velocity if needed
        float speed = originalTargetVelocity.magnitude;

        // relevant only if there are cars in front or intersection is near
        if (carsInFront.Count > 0 || !IsBeforeIntersection()) {
            // get minimum velocity of cars in front
            foreach (CarMovement car in carsInFront.Keys) {
                speed = Mathf.Min(speed, car.GetComponent<Rigidbody>().velocity.magnitude / 2f);
            }
            // stop if there are cars across with right-of-way
            foreach (CarMovement car in carsAcross.Keys) {
                if (car.transform.localPosition.z < transform.localPosition.z) {
                    speed = 0f;
                    break;
                }
            }
        }

        targetVelocity = originalTargetVelocity.normalized * speed;

        // update of real velocity starts here
		Vector3 localVelocity = transform.InverseTransformDirection(GetComponent<Rigidbody>().velocity);
		Vector3 need = targetVelocity - localVelocity;
		Vector3 addend = need * Mathf.Min(1f, Time.deltaTime * acceleration);
		GetComponent<Rigidbody>().velocity += transform.TransformDirection(addend);
		if (OutOfBounds (transform.localPosition)) {
			Command.GetCarSpawner().DestroyCar (this.gameObject);
		}
		if (transform.localPosition.z < -10) {
			Command.GetCarSpawner().PassCar (this.gameObject);
		}
	}

	public void SetToOriginal() {
		targetVelocity = originalTargetVelocity;
	}

	public void OnCollisionEnter(Collision collision) {
		if (collision.collider.tag == "car") {
			// not same road. add collision
			if (OrthogonalCars(this.transform, collision.transform))
				GameObject.FindGameObjectWithTag ("GameMaster").GetComponent<GameGUI> ().AddCollision ();
			Command.GetCarSpawner().DestroyCar(collision.collider.gameObject);
			Command.GetCarSpawner().DestroyCar(this.gameObject);
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
        return car1.localPosition.z > car2.localPosition.z;
    }

    public Dictionary<CarMovement, int> carsInFront = new Dictionary<CarMovement, int>();
    public Dictionary<CarMovement, int> carsAcross = new Dictionary<CarMovement, int>();

    public void OnTriggerEnter(Collider collision) {
        Transform car1 = this.transform;
        Transform car2 = collision.transform;
        CarMovement car2m = car2.GetComponent<CarMovement>();
        if (OrthogonalCars(car1, car2)) {
            if (!carsAcross.ContainsKey(car2m)) {
                carsAcross.Add(car2m, 1);
            } else {
                carsAcross[car2m] += 1;
            }
        }
        if (CarIsBehind(car1, car2)) {
            if (!carsInFront.ContainsKey(car2m)) {
                carsInFront.Add(car2m, 1);
            } else {
                carsInFront[car2m] += 1;
            }
        }
    }

	public void OnTriggerExit(Collider collision) {
        // Debug.Log ("Exit collision with: " + collision);
        Transform car1 = this.transform;
        Transform car2 = collision.transform;
        CarMovement car2m = car2.GetComponent<CarMovement>();
        if (car2m != null) {
            if (carsAcross.ContainsKey(car2m)) {
                if (--carsAcross[car2m] <= 0) {
                    carsAcross.Remove(car2m);
                }
            }
            if (carsInFront.ContainsKey(car2m)) {
                if (--carsInFront[car2m] <= 0) {
                    carsInFront.Remove(car2m);
                }
            }
        }
	}
}

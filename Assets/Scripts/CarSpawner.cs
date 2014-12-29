using UnityEngine;
using System.Collections;

public class CarSpawner : MonoBehaviour {
	// Use this for initialization
	void Start () {
		Main = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public static CarSpawner Main;
	public GameObject[] cars;
	public Vector2 DEFAULT_NORTH_POSITION = new Vector2(-5, 50);

	public void spawn(float degrees) {
		if (cars.Length == 0) {
			throw new UnityException("No car prefabs set!");
		}
		GameObject parent = new GameObject ();
		parent.transform.position = Vector3.zero;
		parent.name = "spawned car";
		parent.hideFlags |= HideFlags.HideInHierarchy;
		int id = Random.Range (0, cars.Length);
		GameObject car = (GameObject)Instantiate (cars[id]);
		car.transform.SetParent (parent.transform);
		car.tag = "car";
		car.transform.localPosition += new Vector3(DEFAULT_NORTH_POSITION.x, 0, DEFAULT_NORTH_POSITION.y);
		// car.transform.localRotation = new Quaternion (-0.7f, 0, 0.7f, 0);
		parent.transform.Rotate (new Vector3 (0, degrees, 0));

		/*

		GameObject road = Command.GetRoadFromAngle (degrees);
		if (StopCommand.Roads.Contains (road)) {
			car.GetComponent<CarMovement> ().movement = CarMovement.STOP;
		}
		else if (GoCommand.Roads.Contains (road)) {
			car.GetComponent<CarMovement> ().movement = CarMovement.GO;
		}

		*/
	}

	public void DestroyCar(GameObject car) {
		car.tag = "Untagged";
		if (car.transform.parent) {
			// Debug.Log ("Destroying parent.");
			Destroy (car.transform.parent.gameObject);
		}
		// Debug.Log ("Destroying car object.");
		car.collider.enabled = false;
		Destroy (car);
	}

	/* old spawn
	public static void spawn(float north, float east) {
		if (Time.time - spawnTime > spawnTimeAllowance) {
			spawnTime = Time.time;
			Vector3 pos = new Vector3 (north, 0f, east);
			Quaternion dir = new Quaternion(-0.7f, 0, 0.7f, 0) * Quaternion.LookRotation (new Vector3(-north, 0f, east));
			Instantiate (prefab, pos, dir);
		}
	} */

	public void spawnNorth() {
		// spawn (0, 50);
		spawn (0);
	}

	public void spawnEast() {
		// spawn (50, 0);
		spawn (90);
	}

	public void spawnSouth() {
		// spawn (0, -50);
		spawn (180);
	}

	public void spawnWest() {
		// spawn (-50, 0);
		spawn (270);
	}

}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CarSpawner : MonoBehaviour {
	// Use this for initialization
	void Start () {
		// Main = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// public static CarSpawner Main;
	public GameObject[] cars;
	public Vector2 DEFAULT_NORTH_POSITION = new Vector2(-5, 50);

	Dictionary<GameObject, Queue<GameObject>> Roads = new Dictionary<GameObject, Queue<GameObject>> ();

	public Queue<GameObject> GetRoadQueue(GameObject road) {
		if (!Roads.ContainsKey (road))
			Roads.Add (road, new Queue<GameObject> ());
		Queue<GameObject> Q = Roads [road];
		while (Q.Count > 0) {
			GameObject car = Q.Peek ();
			if (car == null || (car.transform.localPosition.z < 15 && car.GetComponent<CarMovement>().movement != CarMovement.STOP))
				Q.Dequeue ();
			else break;
		}
		return Q;
	}

	public void AddCarToRoad(GameObject car, GameObject road) {
		GetRoadQueue (road).Enqueue (car);
	}

	public GameObject GetRoadHead(GameObject road) {
		var Q = GetRoadQueue (road);
		if (Q.Count == 0) return null;
		return Q.Peek ();
	}

	public void Spawn(float degrees) {
		if (cars.Length == 0) {
			throw new UnityException("No car prefabs set!");
		}
		GameObject parent = new GameObject ();
		parent.transform.position = Vector3.zero;
		parent.name = "Spawned car";
		parent.hideFlags |= HideFlags.HideInHierarchy;
		int id = Random.Range (0, cars.Length);
		GameObject car = (GameObject)Instantiate (cars[id]);
		car.transform.SetParent (parent.transform);
		car.tag = "car";
		car.transform.localPosition += new Vector3(DEFAULT_NORTH_POSITION.x, 0, DEFAULT_NORTH_POSITION.y);
		// car.transform.localRotation = new Quaternion (-0.7f, 0, 0.7f, 0);
		parent.transform.Rotate (new Vector3 (0, degrees, 0));
		AddCarToRoad (car, Command.GetRoadFromAngle (degrees));
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

	/* old Spawn
	public static void Spawn(float north, float east) {
		if (Time.time - SpawnTime > SpawnTimeAllowance) {
			SpawnTime = Time.time;
			Vector3 pos = new Vector3 (north, 0f, east);
			Quaternion dir = new Quaternion(-0.7f, 0, 0.7f, 0) * Quaternion.LookRotation (new Vector3(-north, 0f, east));
			Instantiate (prefab, pos, dir);
		}
	} */

	public void SpawnNorth() {
		// Spawn (0, 50);
		Spawn (0);
	}

	public void SpawnEast() {
		// Spawn (50, 0);
		Spawn (90);
	}

	public void SpawnSouth() {
		// Spawn (0, -50);
		Spawn (180);
	}

	public void SpawnWest() {
		// Spawn (-50, 0);
		Spawn (270);
	}

}

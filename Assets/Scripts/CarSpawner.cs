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

	Dictionary<GameObject, LinkedList<GameObject>> Roads = new Dictionary<GameObject, LinkedList<GameObject>> ();

	public LinkedList<GameObject> GetRoadQueue(GameObject road) {
		if (!Roads.ContainsKey (road))
			Roads.Add (road, new LinkedList<GameObject> ());
		var Q = Roads [road];
		while (Q.Count > 0) {
			try {
				GameObject car = Q.First.Value;
				if (car == null)
					// car destroyed
					Q.RemoveFirst ();
				else if (car.transform.localPosition.z < 10 && car.GetComponent<CarMovement>().movement != CarMovement.STOP) {
					car.GetComponent<CarMovement>().movement = CarMovement.NORMAL;
					Q.RemoveFirst ();
				}
				else break;
			}
			catch (MissingReferenceException ex) {
				Debug.Log("GameObject does not exist");
				Q.RemoveFirst();
			}
		}
		return Q;
	}

	Dictionary<GameObject, int> RoadScore = new Dictionary<GameObject, int>();
	Dictionary<GameObject, bool> PassedCars = new Dictionary<GameObject, bool>();
	public void PassCar(GameObject car) {
		if (PassedCars.ContainsKey (car))
			return;
		PassedCars.Add (car, true);
		GameObject road = Command.GetRoadFromAngle (car.transform.parent.rotation.eulerAngles.y);
		//Debug.Log ("Passed car on road: " + road);
		if (!RoadScore.ContainsKey (road))
			RoadScore.Add (road, 1);
		else
			RoadScore[road]++;
	}
	public int GetRoadScore(GameObject road) {
		if (!RoadScore.ContainsKey (road))
			return 0;
		return RoadScore[road];
	}

	public void AddCarToRoad(GameObject car, GameObject road) {
		GetRoadQueue (road).AddLast (car);
	}

	public GameObject GetRoadHead(GameObject road) {
		var Q = GetRoadQueue (road);
		if (Q.Count == 0) return null;
		return Q.First.Value;
	}

	public GameObject GetRoadTail(GameObject road) {
		var Q = GetRoadQueue (road);
		if (Q.Count == 0) return null;
		return Q.Last.Value;
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
		var currentRoad = Command.GetRoadFromAngle (degrees);

		car.transform.SetParent (parent.transform);
		car.tag = "car";

		var tail = GetRoadTail (currentRoad);
		// if (tail)
			// Debug.Log (tail.transform.parent.InverseTransformPoint (tail.renderer.bounds.center + tail.renderer.bounds.extents).z);
		car.transform.localPosition += new Vector3 (DEFAULT_NORTH_POSITION.x, 0, tail == null ? DEFAULT_NORTH_POSITION.y : Mathf.Max (DEFAULT_NORTH_POSITION.y, tail.transform.parent.InverseTransformPoint (tail.renderer.bounds.center + tail.renderer.bounds.extents).z + car.transform.parent.InverseTransformPoint(car.renderer.bounds.extents).z + 5f));

		// car.transform.localRotation = new Quaternion (-0.7f, 0, 0.7f, 0);
		
		parent.transform.Rotate (new Vector3 (0, degrees, 0));
		AddCarToRoad (car, currentRoad);
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

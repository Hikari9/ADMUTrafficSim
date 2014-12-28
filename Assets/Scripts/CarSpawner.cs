using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class CarSpawner : MonoBehaviour {

	public GameObject[] cars;
	public Vector2 DEFAULT_NORTH_POSITION = new Vector2(-5, 50);

	public void spawn(float degrees) {
		if (cars.Length == 0) {
			throw new UnityException("No car prefabs set!");
		}
		GameObject parent = new GameObject ();
		parent.transform.position = Vector3.zero;

		GameObject car = (GameObject)Instantiate (cars[Random.Range (0, cars.Length - 1)]);
		car.transform.SetParent (parent.transform);
		car.transform.localPosition = new Vector3(DEFAULT_NORTH_POSITION.x, 0, DEFAULT_NORTH_POSITION.y);
		car.transform.localRotation = new Quaternion (-0.7f, 0, 0.7f, 0);

		parent.transform.Rotate (new Vector3 (0, degrees, 0));
	}

	public void DestroyCar(GameObject car) {
		if (car.transform.parent.gameObject) {
			// Debug.Log ("Destroying parent.");
			Destroy (car.transform.parent.gameObject);
		}
		// Debug.Log ("Destroying car object.");
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

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

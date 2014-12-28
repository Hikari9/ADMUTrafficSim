using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class CarSpawner : MonoBehaviour {

	public static GameObject prefab;

	public static Vector2 DEFAULT_NORTH_POSITION = new Vector2(-5, 50);

	public static void spawn(float degrees) {
		GameObject parent = new GameObject ();
		parent.transform.position = Vector3.zero;

		GameObject car = (GameObject)Instantiate (prefab);
		car.transform.SetParent (parent.transform);
		car.transform.localPosition = new Vector3(DEFAULT_NORTH_POSITION.x, 0, DEFAULT_NORTH_POSITION.y);
		car.transform.localRotation = new Quaternion (-0.7f, 0, 0.7f, 0);

		parent.transform.Rotate (new Vector3 (0, degrees, 0));
	}

	public static void DestroyCar(GameObject car) {
		if (car.transform.parent) {
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

	public static void spawnNorth() {
		// spawn (0, 50);
		spawn (0);
	}

	public static void spawnEast() {
		// spawn (50, 0);
		spawn (90);
	}

	public static void spawnSouth() {
		// spawn (0, -50);
		spawn (180);
	}

	public static void spawnWest() {
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

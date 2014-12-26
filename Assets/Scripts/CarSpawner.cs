using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class CarSpawner : MonoBehaviour {

	public static GameObject prefab;
	static float spawnTime = 0;
	static float spawnTimeAllowance = 0.25f;

	public static void spawn(float north, float east) {
		if (Time.time - spawnTime > spawnTimeAllowance) {
			spawnTime = Time.time;
			Vector3 pos = new Vector3 (north, 0f, east);
			Quaternion dir = new Quaternion(-0.7f, 0, 0.7f, 0) * Quaternion.LookRotation (new Vector3(-north, 0f, east));
			Instantiate (prefab, pos, dir);
		}
	}

	public static void spawnNorth() {
		spawn (0, 50);
	}

	public static void spawnEast() {
		spawn (50, 0);
	}

	public static void spawnSouth() {
		spawn (0, -50);
	}

	public static void spawnWest() {
		spawn (-50, 0);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

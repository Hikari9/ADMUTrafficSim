using UnityEngine;
using System.Collections;

public class CommandReceiver : MonoBehaviour {

	public string spawnNorth = "up";
	public string spawnEast = "right";
	public string spawnSouth = "down";
	public string spawnWest = "left";

	CarSpawner spawn = null;

	// Use this for initialization
	void Start () {
		spawn = GetComponent<CarSpawner>();
	}
	
	// Update is called once per frame
	void Update () {
		try {
			if (Input.GetKeyDown (spawnNorth))
				spawn.spawnNorth();
			if (Input.GetKeyDown (spawnSouth))
				spawn.spawnSouth();
			if (Input.GetKeyDown (spawnEast))
				spawn.spawnEast();
			if (Input.GetKeyDown(spawnWest))
				spawn.spawnWest();
		}
		catch {
			Debug.Log ("Exception thrown.");
		}
	}
}

using UnityEngine;
using System.Collections;

public class CommandReceiver : MonoBehaviour {

	public string spawnNorth = "up";
	public string spawnEast = "right";
	public string spawnSouth = "down";
	public string spawnWest = "left";

	public GameObject prefab = null;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		CarSpawner.prefab = prefab;
		try {
			if (Input.GetKeyDown (spawnNorth))
				CarSpawner.spawnNorth();
			if (Input.GetKeyDown (spawnSouth))
				CarSpawner.spawnSouth();
			if (Input.GetKeyDown (spawnEast))
				CarSpawner.spawnEast();
			if (Input.GetKeyDown(spawnWest))
				CarSpawner.spawnWest();
		}
		catch {
			Debug.Log ("Exception thrown.");
		}
	}
}

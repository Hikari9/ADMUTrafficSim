using UnityEngine;
using System.Collections;

public class CommandReceiver : MonoBehaviour {

	public bool enableSpawn = false;

	public string spawnNorth = "up";
	public string spawnEast = "right";
	public string spawnSouth = "down";
	public string spawnWest = "left";

	public bool enableKeyCommands = false;

	public string stopCommand = "s";
	public string goCommand = "g";

	CarSpawner spawn = null;

	// Use this for initialization
	void Start () {
		spawn = GetComponent<CarSpawner> ();
	}

	// Update is called once per frame
	void Update () {
		try {
			if (enableSpawn) {
				if (Input.GetKeyDown (spawnNorth))
					spawn.SpawnNorth();
				if (Input.GetKeyDown (spawnSouth))
					spawn.SpawnSouth();
				if (Input.GetKeyDown (spawnEast))
					spawn.SpawnEast();
				if (Input.GetKeyDown(spawnWest))
					spawn.SpawnWest();
			}
			if (enableKeyCommands) {
				if (Input.GetKeyDown (this.goCommand)) {
					GameObject.FindGameObjectWithTag("GameController").GetComponent<GoCommand>().PerformCommand ();
				}
				else if (Input.GetKeyDown (this.stopCommand)) {
					GameObject.FindGameObjectWithTag("GameController").GetComponent<StopCommand>().PerformCommand ();
				}
			}
		}
		catch(UnityException e) {
			Debug.Log (e);
			Debug.Log ("Exception thrown.");
		}
	}
}

using UnityEngine;
using System.Collections;

public class TrafficSimulator : MonoBehaviour {

	int spawns;
	public float spawnTimer = 1f;

	// Use this for initialization
	void Start () {
		spawns = 0;
		StartCoroutine (Spawner ());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public int GetScore() {
		// make a more efficient one next
		return spawns - GameObject.FindGameObjectsWithTag ("car").Length;
	}

	IEnumerator Spawner() {
		while (true) {
			var x = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<CarSpawner>();
			if (Random.value < 0.5f)
				x.SpawnNorth ();
			else
				x.SpawnWest ();
			yield return new WaitForSeconds(spawnTimer);
		}
	}
}

using UnityEngine;
using System.Collections;

public class TrafficSimulator : MonoBehaviour {
	public float minSpawnTimer = 1f;
	public float maxSpawnTimer = 2f;

	// Use this for initialization
	void Start () {
		StartCoroutine (Spawner ());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator Spawner() {
		while (true) {
			var x = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<CarSpawner>();
            // monte-carlo simulation
			float rand = Random.value;
			if (rand < 0.33f) // major road
				x.SpawnEast ();
			else if (rand < 0.66f) // major road
				x.SpawnWest ();
			else if (rand < 0.83f)
				x.SpawnSouth ();
			else
				x.SpawnNorth ();
			yield return new WaitForSeconds(Random.Range (minSpawnTimer, maxSpawnTimer));
		}
	}
}

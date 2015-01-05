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
			float rand = Random.value;
			if (rand < 0.25f)
				x.SpawnNorth ();
			else if (rand < 0.50f)
				x.SpawnWest ();
			else if (rand < 0.75f)
				x.SpawnEast ();
			else
				x.SpawnSouth ();
			yield return new WaitForSeconds(Random.Range (minSpawnTimer, maxSpawnTimer));
		}
	}
}

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
			if (Random.value < 0.5f)
				x.SpawnNorth ();
			else
				x.SpawnWest ();
			yield return new WaitForSeconds(Random.Range (minSpawnTimer, maxSpawnTimer));
		}
	}
}

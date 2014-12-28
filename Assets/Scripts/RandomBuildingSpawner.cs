using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class RandomBuildingSpawner : MonoBehaviour {

	public bool enable = false;

	public float roadRadius = 5f;

	[Header("Bounds")]
	public float length = 500f;
	public float width = 500f;

	[Header("Dimensions")]

	public float minLength = 25;
	public float maxLength = 50;
	public float minWidth = 25;
	public float maxWidth = 50;
	public float minHeight = 25;
	public float maxHeight = 50;

	[Header("Spawn Properties")]
	public int spawnSize = 100;
	public float surfaceLevel = 0;

	static object prev = null;


	[Range(0, 1)]
	/* public */ float density = 1f; // no code for this yet

	// Use this for initialization
	void Start () {
		Spawn ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!enable)
			Building.DestroyAll ();
	}
	
	void Spawn() {
		Building.DestroyAll ();
		for (int i = -1; i < 2; i+=2) {
			for (int j = -1; j < 2; j+=2) {
				MakeQuadrant(roadRadius * i, roadRadius * j, (length - roadRadius) * i, (width - roadRadius) * j);
			}
		}
	}

	void MakeQuadrant(float x1, float y1, float x2, float y2) {
		if (x1 > x2) {
			float temp = x1;
			x1 = x2;
			x2 = temp;
		}
		if (y1 > y2) {
			float temp = y1;
			y1 = y2;
			y2 = temp;
		}
		for (int spawns = 0; spawns < spawnSize; ++spawns) {
			float rx = Random.Range (x1, x2);
			float ry = Random.Range (y1, y2);
			float l = Random.Range (minLength, maxLength);
			float w = Random.Range (minWidth, maxWidth);
			float h = Random.Range (minHeight, maxHeight);

			if (rx + l > x2)
				l = x2 - rx;
			if (ry + w > y2)
				w = y2 - ry;
			new Building(rx, surfaceLevel, ry, l, h, w);
		}
	}
				
				
	public class Building {
		Vector3 position, direction;
		GameObject o;
		public Building(Vector3 position, Vector3 direction) {
			this.o = GameObject.CreatePrimitive(PrimitiveType.Cube);
			this.o.name = "Building";
			this.o.tag = "Spawner";
			this.o.hideFlags |= HideFlags.HideInHierarchy;
			this.setPosition(position);
			this.setDirection(direction);
			
			// Debug.Log ("Spawning: " + position + " and " + direction);
		}
		public Building(float x, float y, float z, float dx, float dy, float dz)
			: this(new Vector3(x, y, z), new Vector3(dx, dy, dz))
			{}
		public void setPosition(Vector3 position) {
			this.position = position;
			Update ();
		}
		public void setDirection(Vector3 direction) {
			this.direction = direction;
			Update ();
		}
		private void Update() {
			o.transform.position = position + direction / 2;
			o.transform.localScale = direction;
		}
		public static void DestroyAll() {
			foreach (GameObject building in GameObject.FindGameObjectsWithTag ("Spawner")) {
				DestroyImmediate (building);
			}
		}
	}
}

using UnityEngine;
using System.Collections;

public class AnimatedTransform : MonoBehaviour {

	Vector3 direction;
	public float speed = 5f;

	// Use this for initialization
	public void Start () {
		direction = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 addend = Time.deltaTime * direction * speed;
		this.transform.position += addend;
		direction -= addend;
	}

	public void AddDirectionX(string x) {
		AddDirection (float.Parse (x), 0, 0);
	}

	public void AddDirectionY(string y) {
		AddDirection (0, float.Parse (y), 0);
	}

	public void AddDirectionZ(string z) {
		AddDirection (0, 0, float.Parse (z));
	}

	public void AddDirection(float x, float y, float z) {
		this.direction += new Vector3 (x, y, z);
	}
}

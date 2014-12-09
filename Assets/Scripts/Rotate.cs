using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Rotate : MonoBehaviour {

	public float Speed = 1f;
	public float DegreesPerSecond = 60f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		this.gameObject.transform.Rotate (new Vector3(0, this.DegreesPerSecond * this.Speed * Time.deltaTime));
	}
}

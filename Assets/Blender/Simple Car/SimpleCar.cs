using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class SimpleCar : MonoBehaviour {

	public Material material;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		foreach (Transform child in transform)
			child.renderer.material = material;
	}
}

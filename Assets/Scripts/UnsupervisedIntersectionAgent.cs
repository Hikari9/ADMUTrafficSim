using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnsupervisedIntersectionAgent : MonoBehaviour {

    public Color go = Color.green;
    public Color stop = Color.yellow;

    GameObject car;
    CarMovement movement { get { return car.GetComponent<CarMovement>(); } }
    MeshRenderer renderer { get { return GetComponent<MeshRenderer>(); } }

	// Use this for initialization
	void Start () {
        car = transform.parent.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
        if (movement.targetVelocity.Equals(Vector3.zero)) {
            renderer.material.color = stop;
        } else {
            renderer.material.color = go;
        }
	}
}

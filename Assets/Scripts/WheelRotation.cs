using UnityEngine;
using System.Collections;

public class WheelRotation : MonoBehaviour {

	Vector3 radius;

	// Use this for initialization
	void Start () {
		radius = new Vector3(0, renderer.bounds.extents.y, 0); // safer to get y
	}
	
	// Update is called once per frame
	void Update () {
		Transform parent = transform.parent;
		if (parent.parent == null)
			throw new UnityException ("Wheel Rotation object has no grandparent reference! Use Car Spawner script to spawn cars to make this work.\n");
	
		// Debug.Log (parent.parent.rotation * parent.rigidbody.velocity);
		Vector3 velocity = parent.parent.transform.InverseTransformDirection (parent.rigidbody.velocity); // assume there is a grandparent

		// for now, set angular velocity to Z axis with magnitude v /r
		Vector3 omega = parent.parent.transform.TransformDirection (Vector3.Cross (radius, velocity) / radius.sqrMagnitude);
		transform.RotateAround (renderer.bounds.center, omega, omega.magnitude);
	}
}

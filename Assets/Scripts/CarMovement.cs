using UnityEngine;
using System.Collections;

public class CarMovement : MonoBehaviour {

	float acceleration = 5f;
	Vector3 targetVelocity = new Vector3 (40f, 0f, 0f);
	// Use this for initialization
	void Start () {
		// Debug.Log (this.transform.transform.rotation);
	}

	public static bool OutOfBounds(Vector3 pos) {
		// Debug.Log ("Checking bounds + " + pos);
		return (Mathf.Max (new float[]{Mathf.Abs (pos.x), Mathf.Abs (pos.y), Mathf.Abs (pos.z)}) > 100);
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 localVelocity = transform.InverseTransformDirection (rigidbody.velocity);
		Vector3 need = targetVelocity - localVelocity;
		Vector3 addend = need * this.acceleration * Time.deltaTime;
		rigidbody.velocity += transform.TransformDirection (addend);
		// Debug.Log (rigidbody.velocity);
		if (OutOfBounds (this.gameObject.transform.position) && GetComponent<CarSpawner>()) {
			GetComponent<CarSpawner>().DestroyCar(this.gameObject);
		}
	}
}

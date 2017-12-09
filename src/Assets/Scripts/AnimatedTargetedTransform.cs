using UnityEngine;
using System.Collections;

public class AnimatedTargetedTransform : MonoBehaviour {

	Quaternion originalRotation, targetRotation;
	Vector3 originalPosition, targetPosition;
	public float speed = 5f;
	
	// Use this for initialization
	void Start () {
		SetOriginal ();
	}
	
	// Update is called once per frame
	void Update () {
		{
			Quaternion newRotation = Quaternion.Slerp (transform.localRotation, targetRotation, Time.deltaTime * speed);
			transform.localRotation = newRotation;
		}
		{
			Vector3 need = targetPosition - transform.localPosition;
			Vector3 addend = need * Mathf.Min (1f, Time.deltaTime * speed);
			transform.localPosition += addend;
		}
	}

	public void SetOriginal() {
		originalRotation = transform.localRotation;
		originalPosition = transform.localPosition;
	}
	
	public void ResetRotation() {
		SetRotation(originalRotation);
	}

	public void ResetPosition() {
		SetPosition (originalPosition);
	}

	public void Reset() {
		ResetRotation ();
		ResetPosition ();
	}
	
	public void SetRotation(Quaternion rotation) {
		// Debug.Log ("Setting up rotation: " + rotation + " with current: " + this.transform.localEulerAngles);
		this.targetRotation = rotation;
	}

	public void SetPosition(Vector3 position) {
		this.targetPosition = position;
	}
}

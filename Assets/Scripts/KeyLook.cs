using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KeyLook : MonoBehaviour {

	public KeyCode[] leftLook = {KeyCode.A};
	public KeyCode[] rightLook = {KeyCode.D};
	public float rotationSpeed = 10;

	// Use this for initialization
	void Start () {
	
	}

	private float rotationAngle = 0;

	// Update is called once per frame
	void Update () {
		bool hasInput = false;
		foreach (KeyCode key in leftLook)
			if (Input.GetKeyDown (key)) {
				rotationAngle -= 90;
				hasInput = true;
				break;
			}
		foreach (KeyCode key in rightLook)
			if (Input.GetKeyDown (key)) {
				rotationAngle += 90;
			hasInput = true;
				break;
			}
		float rot = rotationAngle * Mathf.Min (1f, Time.smoothDeltaTime * rotationSpeed);
		transform.Rotate (0, rot, 0);
		rotationAngle -= rot;
		if (hasInput)
			foreach (var com in GetComponents<Command>())
				com.ResetCommand ();
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KeyLook : MonoBehaviour {

	public KeyCode[] leftLook = {KeyCode.A};
	public KeyCode[] rightLook = {KeyCode.D};
	public KeyCode[] left = {KeyCode.A};
	public KeyCode[] right = {KeyCode.D};
	public float rotationSpeed = 10;
	public bool northWestLock = true;

	// for north only constraints
	bool north = true;

	// Use this for initialization
	void Start () {
	
	}

	private float rotationAngle = 0;

	// Update is called once per frame
	void Update () {
		bool hasInput = false;
		foreach (KeyCode key in leftLook)
			if (Input.GetKeyDown (key)) {
				if (northWestLock && !north) break;
				rotationAngle -= 90;
				hasInput = true;
				if (northWestLock) north = false;
				break;
			}
		foreach (KeyCode key in rightLook)
			if (Input.GetKeyDown (key)) {
				if (northWestLock && north) break;
				rotationAngle += 90;
				hasInput = true;
				if (northWestLock) north = true;
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

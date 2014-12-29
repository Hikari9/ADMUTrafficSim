using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[ExecuteInEditMode]
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
		foreach (KeyCode key in leftLook) {
			if (Input.GetKeyDown (key)) {
				rotationAngle -= 90;
				break;
			}
		}
		foreach (KeyCode key in rightLook){
			if (Input.GetKeyDown (key)) {
				rotationAngle += 90;
				break;
			}
		}
		
		float rot = Time.smoothDeltaTime * rotationAngle * rotationSpeed;
		transform.Rotate (0, rot, 0);
		rotationAngle -= rot;
	}
}

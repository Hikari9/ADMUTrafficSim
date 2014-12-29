using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[ExecuteInEditMode]
public class HandKeyLook : MonoBehaviour {

	public KeyCode[] leftLook = {KeyCode.A};
	public KeyCode[] rightLook = {KeyCode.D};
	public float rotationSpeed = 10;

	// Use this for initialization
	void Start () {
	
	}

	public float rotationAngle = 0;

	// Update is called once per frame
	void Update () {
		float yangle = transform.eulerAngles.y;
		// float yangle = (transform.eulerAngles.y* ((float)Math.PI))/((float)180);
		// float xpos = 8 * ((float) Math.Sin(yangle));
		// float zpos = 8 * ((float) Math.Cos(yangle));
		
		// Debug.Log(xpos + " " + zpos);
		// transform.position.Set(xpos, -0.4f, zpos);
		
		//forward
		if(Math.Abs(0-yangle)<1e-6)
			transform.position.Set(4, 5, 8);
		//right
		else if(Math.Abs(90-yangle)<1e-6)
			transform.position.Set(8, 5, -4);
		//back
		else if(Math.Abs(180-yangle)<1e-6)
			transform.position.Set(-4, 5, -8);
		else if(Math.Abs(270-yangle)<1e-6)
			transform.position.Set(-8, 5, 4);
		
	}
}

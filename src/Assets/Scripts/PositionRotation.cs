using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PositionRotation : MonoBehaviour {
	public Vector3 position, rotation;
	public PositionRotation(Vector3 pos, Vector3 rot) {
		position = pos;
		rotation = rot;
	}
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StopCommand : Command {
	
	public override void PerformCommand() {
		base.PerformCommand ();
		GameObject currentRoad = GetRoadFromAngle (transform.rotation.eulerAngles.y);
		GetComponent<GoCommand>().Roads.Remove (currentRoad);
		if (Roads.Contains (currentRoad))
			return;
		Roads.Add (currentRoad);

		if (GetComponent<GoCommand> ())
			GetComponent<GoCommand> ().ResetCommand ();
		this.TransformCommand ();
	}

	// Use this for initialization
	void Start () {
		commandColor = Color.red;
	}
	void Update() {
		DeliverCarMovement (CarMovement.STOP);
	}

}

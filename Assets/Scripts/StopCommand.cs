using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StopCommand : Command {

	public static HashSet<GameObject> Roads = new HashSet<GameObject>();

	public override void PerformCommand() {
		base.PerformCommand ();
		GameObject currentRoad = GetRoadFromAngle (this.transform.rotation.eulerAngles.y);
		GoCommand.Roads.Remove (currentRoad);
		if (Roads.Contains (currentRoad))
			return;
		Roads.Add (currentRoad);
		GameObject[] cars = GameObject.FindGameObjectsWithTag ("car");
		// code to stop all cars, but very slow
		foreach (GameObject car in cars) {
			if (car.transform.localPosition.z > 10 && GetRoadFromAngle (car.transform.parent.rotation.eulerAngles.y) == currentRoad)
				car.GetComponent<CarMovement> ().movement = CarMovement.STOP;
		}
		/*
		// just stop the leading car and all's fine
		GameObject leading = null;
		foreach (GameObject car in cars) {
			if (car.transform.localPosition.z > 15 && GetRoadFromAngle(car.transform.parent.rotation.eulerAngles.y) == currentRoad) {
				if (leading == null || leading.transform.localPosition.z > car.transform.localPosition.z)
					leading = car;
			}
		}
		if (leading)
			leading.GetComponent<CarMovement> ().movement = CarMovement.STOP;
		*/
	}

	// Use this for initialization
	void Start () {
		commandColor = Color.red;
	}
	void Update() {
		if (Input.GetKeyDown (KeyCode.R))
			PerformCommand();
	}

}

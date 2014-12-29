using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GoCommand : Command {
	public static HashSet<GameObject> Roads = new HashSet<GameObject>();
	
	public override void PerformCommand() {
		
		base.PerformCommand ();
		StartCoroutine (UnfadeToNormal ());

		GameObject currentRoad = GetRoadFromAngle (this.transform.rotation.eulerAngles.y);
		StopCommand.Roads.Remove (currentRoad);
		if (Roads.Contains (currentRoad))
			return;
		Roads.Add (currentRoad);

		GameObject[] cars = GameObject.FindGameObjectsWithTag ("car");
		// code to stop all cars, but very slow
		/*
		foreach (GameObject car in cars) {
			if (GetRoadFromAngle (car.transform.parent.rotation.eulerAngles.y) == currentRoad)
				car.GetComponent<CarMovement> ().movement = CarMovement.GO;
		}
		*/

		// just stop the leading car and all's fine
		GameObject leading = null;
		foreach (GameObject car in cars) {
			if (GetRoadFromAngle(car.transform.parent.rotation.eulerAngles.y) == currentRoad) {
				CarMovement move = car.GetComponent<CarMovement>();
				if (move.movement == CarMovement.STOP) {
					move.movement = CarMovement.NORMAL;
					move.SetToOriginal ();
				}
			}
		}

		if (GetComponent<StopCommand> ())
			GetComponent<StopCommand> ().ResetCommand ();
		this.TransformCommand ();
	}

	IEnumerator UnfadeToNormal() {
		yield return new WaitForSeconds(2f);
		Glow glow = GetRoadFromAngle (transform.rotation.eulerAngles.y).GetComponent<Glow> ();
		glow.SetColor (glow.originalColor);
	}
	
	// Use this for initialization
	void Start () {
		commandColor = Color.green;
	}
	void Update() {
	//	if (Input.GetKeyDown (KeyCode.G))
	//		PerformCommand();
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GoCommand : Command {
	
	public override void PerformCommand() {
		
		base.PerformCommand ();

		GameObject currentRoad = GetRoadFromAngle (this.transform.rotation.eulerAngles.y);
		GetComponent<StopCommand>().Roads.Remove (currentRoad);

		if (Roads.Contains (currentRoad))
			return;
		Roads.Add (currentRoad);

		
		StartCoroutine (UnfadeToNormal (currentRoad));
		if (GetComponent<StopCommand> ())
			GetComponent<StopCommand> ().ResetCommand ();
		this.TransformCommand ();
	}

	IEnumerator UnfadeToNormal(GameObject road) {
		yield return new WaitForSeconds(2f);
		Glow glow = GetRoadFromAngle (transform.rotation.eulerAngles.y).GetComponent<Glow> ();
		if (glow.GetColor() == commandColor) {
			glow.SetColor (glow.originalColor);
			Roads.Remove (road);
		}
	}
	
	// Use this for initialization
	void Start () {
		commandColor = Color.green;
	}


	void Update() {
		DeliverCarMovement (CarMovement.GO);
	}
}

using UnityEngine;
using System.Collections;

public class Command : MonoBehaviour {
	protected Color commandColor = Color.white;
	public virtual void PerformCommand() {
		GameObject officer = GameObject.FindGameObjectWithTag ("GameController");
		GameObject currentRoad = GetRoadFromAngle (officer.transform.rotation.eulerAngles.y);
		currentRoad.GetComponent<Glow> ().setColor (commandColor);
	}
	public static GameObject GetRoadFromAngle(float angle) {
		while (angle < 0) angle += 360;
		GameObject[] roads = GameObject.FindGameObjectsWithTag ("road");
		int id = Mathf.RoundToInt (angle / 90) % 4;
		return roads [(5 - id) % 4];
	}
	void Update() {
	}
}

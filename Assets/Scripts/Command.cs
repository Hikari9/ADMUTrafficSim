using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Command : MonoBehaviour {
	protected Color commandColor = Color.white;

	[System.Serializable]
	public class Targetter {
		public GameObject gameobject;
		public Vector3 position;
		public Quaternion rotation;
		public Targetter(GameObject gameobject, Vector3 position, Quaternion rotation) {
			this.gameobject = gameobject;
			this.position = position;
			this.rotation = rotation;
		}
	}
	public List<Targetter> transformers = new List<Targetter>();
	//public List<KeyValuePair<GameObject, PositionRotation>> transformers = new List<KeyValuePair<GameObject, PositionRotation>>();

	public void ResetCommand() {
		foreach (Targetter target in transformers) {
			if (!target.gameobject.GetComponent<AnimatedTargetedTransform>()) {
				target.gameobject.AddComponent <AnimatedTargetedTransform>();
				target.gameobject.GetComponent<AnimatedTargetedTransform>().SetOriginal();
			}
			AnimatedTargetedTransform trans = target.gameobject.GetComponent<AnimatedTargetedTransform>();
			trans.Reset ();
		}
	}

	public void TransformCommand() {
		foreach (Targetter target in transformers) {
			if (!target.gameobject.GetComponent<AnimatedTargetedTransform>()) {
				target.gameobject.AddComponent <AnimatedTargetedTransform>();
				target.gameobject.GetComponent <AnimatedTargetedTransform>().SetOriginal();
			}
			AnimatedTargetedTransform trans = target.gameobject.GetComponent<AnimatedTargetedTransform>();
			trans.SetPosition (target.position);
			trans.SetRotation (target.rotation);
		}
	}

	public virtual void PerformCommand() {
		GameObject officer = GameObject.FindGameObjectWithTag ("GameController");
		GameObject currentRoad = GetRoadFromAngle (officer.transform.rotation.eulerAngles.y);
		currentRoad.GetComponent<Glow> ().SetColor (commandColor);
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

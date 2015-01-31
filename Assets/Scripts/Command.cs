using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Command : MonoBehaviour {
	protected Color commandColor = Color.white;
	private static CarSpawner carspawner = null;
	public static CarSpawner GetCarSpawner() {
		if (carspawner == null)
			carspawner = GameObject.FindGameObjectWithTag ("GameMaster").GetComponent<CarSpawner> ();
		// Debug.Log (carspawner);
		return carspawner;
	}

	/*
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
	*/

	public AnimationClip animationClip = null;

	public void ResetCommand() {
		GetComponent<Animator> ().Play ("idle");
		/*
		foreach (Targetter target in transformers) {
			if (!target.gameobject.GetComponent<AnimatedTargetedTransform>()) {
				target.gameobject.AddComponent <AnimatedTargetedTransform>();
				target.gameobject.GetComponent<AnimatedTargetedTransform>().SetOriginal();
			}
			AnimatedTargetedTransform trans = target.gameobject.GetComponent<AnimatedTargetedTransform>();
			trans.Reset ();
		}*/
	}

	public void TransformCommand() {
		if (animationClip) {
			GetComponent<Animator> ().Play (animationClip.name);
		}
		/*
		foreach (Targetter target in transformers) {
			if (!target.gameobject.GetComponent<AnimatedTargetedTransform>()) {
				target.gameobject.AddComponent <AnimatedTargetedTransform>();
				target.gameobject.GetComponent <AnimatedTargetedTransform>().SetOriginal();
			}
			AnimatedTargetedTransform trans = target.gameobject.GetComponent<AnimatedTargetedTransform>();
			trans.SetPosition (target.position);
			trans.SetRotation (target.rotation);
		}*/
	}

	public virtual void PerformCommand() {
		GameObject officer = GameObject.FindGameObjectWithTag ("GameController");
		GameObject currentRoad = GetRoadFromAngle (officer.transform.rotation.eulerAngles.y);
		currentRoad.GetComponent<Glow> ().SetColor (commandColor);
	}
	
	public HashSet<GameObject> Roads = new HashSet<GameObject>();
	protected HashSet<GameObject> current = new HashSet<GameObject>();

	public void DeliverCarMovement(int movement) {
		HashSet<GameObject> next = new HashSet<GameObject> ();
		HashSet<GameObject> change = new HashSet<GameObject> ();
		foreach (GameObject road in Roads) {
			GameObject head = GetCarSpawner().GetRoadHead (road);
			if (head != null && movement == CarMovement.STOP && head.transform.localPosition.z > 23)
				continue;
			if (head != null) {
				if (current.Remove (head))
					next.Add (head);
				else
					change.Add (head);
			}
			//head.GetComponent<CarMovement>().movement = CarMovement.GO;
		}
		foreach (var car in current) {
			if (car.gameObject == null) continue;
			car.GetComponent<CarMovement> ().movement = CarMovement.NORMAL;
		}

		foreach (var car in change) {
			if (car.gameObject == null) continue;
			car.GetComponent<CarMovement> ().movement = movement;
		}

		current = next;
		foreach (var car in change) {
			if (car.gameObject == null) continue;
			current.Add (car);
		}
	}

	public static GameObject GetRoadFromAngle(float angle) {
		while (angle < 0) angle += 360;
		GameObject[] roads = GameObject.FindGameObjectsWithTag ("road");
		int id = Mathf.RoundToInt (angle / 90) % 4;
		return roads [(5 - id) % 4];
	}

	public static void LoadLevel(string level) {
		Application.LoadLevel (level);
		foreach (GameObject o in GameObject.FindObjectsOfType<GameObject> ()) {
			Destroy (o.gameObject);
		}
	}

	void Update() {
	}

}

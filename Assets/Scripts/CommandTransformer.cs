using UnityEngine;
using System.Collections;

public class CommandTransformer : MonoBehaviour {

	public StopCommand stopCommand;
	public GameObject stopPrefab;

	public GoCommand goCommand;
	public GameObject goPrefab;

	// Use this for initialization
	void Start () {
		ListTransformers (stopCommand, stopPrefab);
		ListTransformers (goCommand, goPrefab);
	}

	// Update is called once per frame
	void Update () {
	
	}

	void dfs(Transform child, Command command, GameObject prefab) {
		Transform partner = prefab.transform.Find (child.name);
		if (!partner) return;
		command.transformers.Add (new Command.Targetter(child.gameObject, partner.localPosition, partner.localRotation));
		foreach (Transform grandchild in child) {
			dfs (grandchild, command, partner.gameObject);
		}
	}

	void ListTransformers(Command command, GameObject prefab) {
		if (command && prefab) {
			foreach (Transform child in transform) {
				dfs(child, command, prefab);
			}
		}
	}
}

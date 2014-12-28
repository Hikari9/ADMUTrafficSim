using UnityEngine;
using System.Collections;

public class GameLoader : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void LoadLevel(string level) {
		Debug.Log ("Loading level: " + level);
		Application.LoadLevel (level);
	}
}

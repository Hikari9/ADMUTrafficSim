using UnityEngine;
using System.Collections;

public class GameLoader : MonoBehaviour {

	public void LoadLevel(string level) {
		Debug.Log ("Loading level: " + level);
		Application.LoadLevel (level);
	}

	public void Quit() {
		Application.Quit ();
	}

	void Update() {
	}

}

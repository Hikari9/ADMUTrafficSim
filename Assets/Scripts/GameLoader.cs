using UnityEngine;
using System.Collections;

public class GameLoader : MonoBehaviour {

	public void LoadLevel(string level) {
		Command.LoadLevel (level);
	}



	public void Quit() {
		Application.Quit ();
	}

	void Update() {
	}

}

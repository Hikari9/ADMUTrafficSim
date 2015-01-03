using UnityEngine;
using System.Collections;

public class GamePauser : MonoBehaviour {

	protected bool Pausing = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space))
			Pausing ^= true;
		Time.timeScale = Pausing ? 0f : 1f;
	}

	void OnGUI() {
		if (Pausing) {
			GUIStyle style = new GUIStyle(GUI.skin.GetStyle ("label"));

			style.fontSize = 100;
			style.hover.textColor = style.normal.textColor = Color.white;

			GUI.Box (new Rect ((Screen.width)/2 -(Screen.width)/4,(Screen.height)/2-(Screen.height)/4,(Screen.width)/2,(Screen.height)/2), "GAME PAUSED", style);
		}
	}
}

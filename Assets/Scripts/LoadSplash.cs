using UnityEngine;
using System.Collections;

public class LoadSplash : MonoBehaviour {

	public Texture[] splash;
	int current = 0;
	// public int width;
	// public int height;
	private Rect r;
	
	// Use this for initialization
	void Start () {
		r = new Rect(0, 0, Screen.width, Screen.height);
	}
	
	// Update is called once per frame
	void Update() {
		if (Input.anyKeyDown)
			current++;
		if (current >= splash.Length)
			Command.LoadLevel ("Main");
	}
	void OnGUI () {
		try {
			GUI.DrawTexture(r, splash[current]);
		}
		catch { current++; }
	}
}

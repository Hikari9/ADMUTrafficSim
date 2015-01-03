using UnityEngine;
using System.Collections;

public class LoadSplash : MonoBehaviour {

	public Texture splash;
	// public int width;
	// public int height;
	private Rect r;
	
	// Use this for initialization
	void Start () {
		r = new Rect(0, 0, Screen.width, Screen.height);
	}
	
	// Update is called once per frame
	void OnGUI () {
		GUI.DrawTexture(r, splash);
		
		if(Input.anyKey) {
			Application.LoadLevel("Main");
		}
	}
}

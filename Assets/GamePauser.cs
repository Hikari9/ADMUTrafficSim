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
}

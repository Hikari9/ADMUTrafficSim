using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TutorialGUI : MonoBehaviour {

	internal delegate void Void();
	internal delegate bool Bool();

	int current = 0;
	List<TutorialScene> scenes = new List<TutorialScene>();
	TutorialScene CurrentScene {
		get {
			if (current >= scenes.Count)
				return null;
			return scenes[current];
		}
	}

	void OnGUI() {
		if (CurrentScene != null) {
			CurrentScene.OnGUI ();
		}
	}
	void Start () {
		scenes.Add (new TutorialScene("Welcome!"));
		scenes.Add (new TutorialScene("You'll be playing as a traffic enforcer.", 60, 650));
	}
	void Update () {
		if (CurrentScene == null) {
			// Done
			Command.LoadLevel("Main");
			return;
		}
		if (CurrentScene.Check())
			current++;
		else
			CurrentScene.Updater ();
	}

	
	class TutorialScene {
		
		public GUIStyle style = new GUIStyle("label");
		public string text;
		public float width, height;
		
		public TutorialScene(string text, int size = 100, float width = -1, float height = -1) {
			this.text = text;
			style.fontSize = size;
			style.alignment = TextAnchor.MiddleCenter;
			style.normal.textColor = Color.white;
			style.wordWrap = true;
			if (width == -1)
				width = Screen.width;
			if (height == -1)
				height = Screen.height;
			this.width = width;
			this.height = height;
		}
		
		// check if command is fulfilled before proceeding
		// override-able
		public Bool Check = AnyKeyCheck;
		public static GameObject GetController() {
			return GameObject.FindGameObjectWithTag ("GameController");
		}
		public static void SetControllerInputs(bool enabled) {
			GameObject controller = GetController ();
			controller.GetComponent<KeyLook>().enabled = enabled;
			controller.GetComponent<StopCommand>().enabled = enabled;
			controller.GetComponent<GoCommand>().enabled = enabled;
		}
		public static Bool AnyKeyCheck = delegate() {
			SetControllerInputs(false);
			if (Input.anyKeyDown) {
				return true;
			}
			return false;
		};

		public Void Updater = delegate() {

		};
		
		public void OnGUI() {
			DrawString(text);
		}

		
		public void DrawString(object label) {
			float x, y;
			x = (Screen.width - width) / 2;
			y = (Screen.height * 0.5f - height) / 2;
			Rect r = new Rect(x, y, width, height);
			GUI.Label (r, label.ToString (), style);
		}
	}


}

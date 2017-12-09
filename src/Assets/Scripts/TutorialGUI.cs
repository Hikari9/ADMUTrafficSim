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
		// highlight enforcer
		scenes.Add (new TutorialScene("You'll be playing as a traffic enforcer.", 60, 650));
		scenes.Add (new TutorialScene("Your objective is to make as many cars pass the intersection in three minutes.", 60, 900));
		scenes.Add (new TutorialScene("Avoid collisions as much as possible!", 60, 900));

		// highlight timer
		scenes.Add (new TutorialScene("Your timer below shows the remaining time.", 60, 650));
		// jeepneys approach from N and E
		scenes.Add (new TutorialScene(""));
		// pause after a couple of seconds
		// highlight cars
		scenes.Add (new TutorialScene("There are two cars approaching!", 60, 650));
		scenes.Add (new TutorialScene("Raise your right hand to make your car stop.", 60, 650));
		// pause
		// open STOP command
		// wait for stop signal
		scenes.Add (new TutorialScene("Good job!"));
		// close input commands
		// wait for score to update then highlight
		// pause
		scenes.Add (new TutorialScene("When you let cars pass, your score below updates.", 60, 900));
		scenes.Add (new TutorialScene("Now wave your right hand to make the car move.", 60, 800));
		// pause
		// open GO command
		// wait fot go signal
		scenes.Add (new TutorialScene("Good job!"));
		// close input commands
		// wait for score to update
		// spawn W and S
		// pause
		scenes.Add (new TutorialScene("Oh no! A car is approaching from the left!", 60, 900));
		// highlight minimap
		scenes.Add (new TutorialScene("The mini-map displays the cars around you and your current line of sight.", 60, 900));
		scenes.Add (new TutorialScene("Use the WASD or Arrow Keys to rotate!", 60, 900));
		// open LOOK LEFT command (only)
		// wait for rotation
		// play, wait for car to be near
		scenes.Add (new TutorialScene("Good! Now make the car stop to avoid a collision.", 60, 900));
		// close input commands
		// open STOP command
		// wait for stop signal
		// play
		scenes.Add (new TutorialScene("Good job! You now know the basics of the game.", 60, 900));
		scenes.Add (new TutorialScene("Be careful! If cars collide, your score will be deducted.", 60, 900));
		// highlight collisions
		scenes.Add (new TutorialScene("You can see the number of collisions below.", 60, 900));
		scenes.Add (new TutorialScene("Now you're ready to go! Good luck!", 80, 900));

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

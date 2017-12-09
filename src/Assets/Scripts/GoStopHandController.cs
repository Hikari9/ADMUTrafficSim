using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Leap;

/// <summary>
/// append this script with LeapHandController
/// checks for go and stop signals using leap motion sensor
/// </summary>

public class GoStopHandController : MonoBehaviour {
	
	// public members
	
	public bool debugMode = false;

	public int GO_TARGET_COUNT = 40;
	public int STOP_TARGET_COUNT = 10;
	public float SLEEP_VELOCITY = 5f;
	int goCount, stopCount;
	
	// private members
	
	Controller controller;

	// use this for initialization
	
	void Start() {
		goCount = stopCount = 0;
		controller = new Controller();
		LeapInputEx.HandUpdated += OnHandUpdated;
		ConfigureController();
	}
	
	// updates every frame
	
	void Update() {
	}
	
	// private methods

	private void ConfigureController() {
		controller.Config.SetFloat("Gesture.Swipe.MinLength", 10f);
		controller.Config.SetFloat("Gesture.Swipe.MinVelocity", 10f);
		controller.Config.Save();
	}
	
	private void OnDestroy() {
		LeapInputEx.HandUpdated -= OnHandUpdated;
	}
	
	private void OnHandUpdated(Hand h) {
		if (CheckStop ()) {
			ExecuteStop ();
			ResetCount ();
		}
		else if (CheckGo ()) {
			ExecuteGo ();
			ResetCount();
		}
	}

	private void ResetCount() {
		goCount = stopCount = 0;
	}
	
	private bool CheckStop() {
		if (controller.Frame ().Hands.Count > 0 && controller.Frame ().Hands.Rightmost.PalmVelocity.Magnitude < SLEEP_VELOCITY) {
			stopCount++;
			// Log (controller.Frame ().Hands.Rightmost.PalmNormal);
		}
		else
			stopCount = 0;
		return stopCount >= STOP_TARGET_COUNT;
	}
	
	private bool CheckGo() {
		foreach (Gesture g in controller.Frame ().Gestures ()) {
			if (g.Type == Gesture.GestureType.TYPESWIPE)
				goCount ++;
		}
		// Log (goCount);
		return goCount >= GO_TARGET_COUNT;
	}
	
	private void Log(object text) {
		if (debugMode)
			Debug.Log(text);
	}


	// public methods
	
	public void ExecuteGo() {
		// Place code here for when in GO
		Log ("Executing Go.");
		GameObject.FindGameObjectWithTag ("GameController").GetComponent<GoCommand> ().PerformCommand ();
	}
	
	public void ExecuteStop() {
		// Place code here for when in STOP
		Log ("Executing Stop.");
		GameObject.FindGameObjectWithTag ("GameController").GetComponent<StopCommand> ().PerformCommand ();
	}

	
}
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
	
	public static Gesture.GestureType SWIPE = Gesture.GestureType.TYPE_SWIPE;
	public static float AXIS_RANGE_FOR_GO = 5f;
	public static int GO_TARGET_COUNT = 3;
	public static float COUNT_ELAPSED_TIME_CAP = 2;
	
	// private members
	
	Controller controller;
	Dictionary<int, List<SwipeGesture>> Recent;
	long timeStart, timeElapsed, countStart, countElapsed, goCount, currentTime;
	
	// public methods
	
	public void ExecuteGo() {
		// Place code here for when in GO
	}
	
	public void ExecuteStop() {
		// Place code here for when in STOP
	}
	
	// use this for initialization
	
	void Start() {
		controller = new Controller();
		Recent = new Dictionary<int, List<SwipeGesture>>();
		timeStart = countStart = currentTime = -1;
		timeElapsed = countElapsed = goCount = 0;
		LeapInputEx.HandUpdated += OnHandUpdated;
		ConfigureController();
	}
	
	// updates every frame
	
	void Update() {
		if (timeStart >= 0) {
			Log ("Entered check.");
			timeElapsed = getCurrentTime() - timeStart;
			
			if (countStart >= 0)
				countElapsed = getCurrentTime() - countStart;
			
			if (timeElapsed > 0 && Recent.Count > 0) {
				if (CheckStop()) {
					Log ("Command Stop executed.");
					ExecuteStop();
				}
				
				else if (CheckGo()) {
					Log ("Command Go executed.");
					ExecuteGo();
				}
				
				ResetTimeFrame();
			}
			
			if (countElapsed > COUNT_ELAPSED_TIME_CAP)
				ResetConsecutive();
		}
		this.currentTime = -1;
	}
	
	// private methods
	
	private void Log(string text) {
		if (debugMode)
			Debug.Log(text);
	}
	
	private void ConfigureController() {
		controller.Config.SetFloat("Gesture.ScreenTap.MinForwardVelocity", 0.1f);
		controller.Config.SetFloat("Gesture.ScreenTap.HistorySeconds", 0.5f);
		controller.Config.SetFloat("Gesture.ScreenTap.MinDistance", 0.5f);
		controller.Config.SetFloat("Gesture.Swipe.MinLength", 10.0f);
		controller.Config.SetFloat("Gesture.Swipe.MinVelocity", 10f);
		controller.Config.Save();
	}
	
	private void OnDestroy() {
		// necessary to clean delegate assignment between scenes
		LeapInputEx.HandUpdated -= OnHandUpdated;
	}
	
	private long getCurrentTime() {
		if (currentTime < 0)
			currentTime = controller.Frame().Timestamp / 1000000;
		return currentTime;
	}
	
	private void ResetConsecutive() {
		goCount = 0;
		countStart = -1;
		countElapsed = 0;
	}
	
	private void ResetTimeFrame() {
		timeStart = -1;
		timeElapsed = 0;
		Recent.Clear();
	}
	
	private void OnHandUpdated(Hand h) {
		Frame frame = controller.Frame();
		GestureList gestures = frame.Gestures();
		foreach (Gesture gesture in gestures) {
			if (gesture.Type == SWIPE) {
				SwipeGesture swipe = new SwipeGesture(gesture);
				
				if (!Recent.ContainsKey(swipe.Id)) {
					Recent.Add(swipe.Id, new List<SwipeGesture>());
				}
				
				Recent[swipe.Id].Add(swipe);
			}
		}
		timeStart = getCurrentTime();
		currentTime = -1;
	}
	
	// public methods
	
	private bool CheckStop() {
		foreach (var list in Recent.Values) {			
			SwipeGesture a = list[0];
			SwipeGesture b = list[list.Count - 1];
			
			Vector delta = a.Position - b.Position;
			Vector pn = a.Hands.Rightmost.PalmNormal;
			
			if (delta.z > delta.y && delta.z > delta.x && pn.z < -0.5f && pn.x > -0.5f) {
				if (a.Direction.z > 0)
					continue;
				
				// DO STOP HERE
				ResetConsecutive();
				return true;
			}
		}
		return false;
	}
	
	private bool CheckGo() {
		foreach (var list in Recent.Values) {
			SwipeGesture a = list[0];
			SwipeGesture b = list[list.Count - 1];
			
			Vector delta = a.Position - b.Position;
			
			if (Math.Abs(delta.x) > AXIS_RANGE_FOR_GO) {
				if (a.Direction.x > 0)
					continue;
				
				countStart = getCurrentTime();
				goCount++;
				
				Log ("Command GO buffering at " + goCount + " of " + GO_TARGET_COUNT);
				if (goCount >= GO_TARGET_COUNT) {
					// DO GO HERE
					
					ResetConsecutive();
					return true;
				}
			}
			
			break; // idk what this break is for, but author nina put it so i just copied <hikari9>
		}
		return false;
	}
	
}
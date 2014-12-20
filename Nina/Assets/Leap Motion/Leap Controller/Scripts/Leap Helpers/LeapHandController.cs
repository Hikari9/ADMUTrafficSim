using UnityEngine;
using System.Collections;
using Leap;
using System;
using System.Collections.Generic;

/// <summary>
/// gets updates from leap api
/// assigns leap hand data to unity hand
/// </summary>
public class LeapHandController : MonoBehaviour
{
	public UnityHand[] unityHands;
	public UnityHandSettings handSettings;
	public Controller controller;
	
	private Dictionary<int, List<SwipeGesture> > Recent;
	private long tf_elapsed;
	private long tf_start = -1;
	private long c_start = -1;
	private long c_elapsed;
	private int c_count = 0;
	
	private float timeVisible = 0.2f;
					
	void Start ()
	{
		
		controller = new Controller();
		Recent = new Dictionary<int, List<SwipeGesture> >();
		tf_elapsed = 0;
		c_elapsed = 0;
		
		//attach controller methods to Leap's hand updates
		LeapInputEx.HandUpdated += OnHandUpdated;
		LeapInputEx.HandFound += OnHandFound;
		LeapInputEx.HandLost += OnHandLost;
		
		//enable gestures
		controller.EnableGesture(Gesture.GestureType.TYPE_SWIPE);
		
		unityHands[0].AssignSettings(handSettings);
		unityHands[1].AssignSettings(handSettings);
		// Debug.Log("init");
		
		controller.Config.SetFloat("Gesture.ScreenTap.MinForwardVelocity", 0.1f);
		controller.Config.SetFloat("Gesture.ScreenTap.HistorySeconds", 0.5f);
		controller.Config.SetFloat("Gesture.ScreenTap.MinDistance", 0.5f);
		controller.Config.SetFloat("Gesture.Swipe.MinLength", 10.0f);
		controller.Config.SetFloat("Gesture.Swipe.MinVelocity", 10f);
		controller.Config.Save();
	}

	void Update()
	{
		//process the Leap message pump
		LeapInputEx.Update();
		
		Frame frame = controller.Frame();
		
		if(tf_start!=-1) {
			tf_elapsed = frame.Timestamp/1000000 - tf_start;
			
			if(c_start!=-1)
				c_elapsed = frame.Timestamp/1000000 - c_start;
			
			if(tf_elapsed>0 && Recent.Count>0) {
				
				outer: foreach(KeyValuePair<int, List<SwipeGesture>> kvp in Recent) {
					List<SwipeGesture> lis = kvp.Value;
					
					int i = 0;
					SwipeGesture a = null;
					SwipeGesture b = null;
					// Debug.Log(lis.Count);
					foreach(SwipeGesture sg in lis) {
						if(i==0)
							a = sg;
						if(i==lis.Count-1)
							b = sg;
						i++;
					}
					
					float x_dist = Math.Abs(a.Position.x-b.Position.x);
					float y_dist = Math.Abs(a.Position.y-b.Position.y);
					float z_dist = Math.Abs(a.Position.z-b.Position.z);
					
					Vector pn = a.Hands.Rightmost.PalmNormal;
					// Debug.Log(pn);
					
					//"STOP" signal
					if(z_dist>y_dist && z_dist>x_dist && pn.z < -0.5f && pn.x > -0.5f) {
						if(a.Direction.z > 0)
							continue;
						
						//DO STOP HERE
						
						// Debug.Log("STOP! " + x_dist + " " + z_dist + " " + pn.x + " " + pn.z);
						ResetConsecutive();
						break;
					}
					//"GO" signal
					else if(Math.Abs(a.Position.x - b.Position.x)>5f) {
						if(a.Direction.x > 0)
							continue;
						
						c_start = frame.Timestamp/1000000;
						c_count++;
						
						// Debug.Log("valid " + c_count);
						if(c_count==3) {
							//DO GO HERE
							
							// Debug.Log("GO!");
							ResetConsecutive();
						}
						
						break;
					}
					
				}
				
				ResetTimeFrame();
			}
			
			if(c_elapsed>2f) {
				ResetConsecutive();
			}
		}
	}
	
	private void OnHandFound(Hand h) {
		Messenger.Broadcast<int>(SIG.HANDFOUND.ToString(), h.Id); //broadcast new hand ID to registered listeners
	}
 	
	private void OnHandUpdated(Hand h) {
		bool undeterminedHand = true;
		
		Frame frame = controller.Frame();
		GestureList gestures = frame.Gestures ();
		
		for (int i = 0; i < gestures.Count; i++) {
			Gesture gesture = gestures[i];
			
			if(gesture.Type==Gesture.GestureType.TYPE_SWIPE) {
				SwipeGesture swipe = new SwipeGesture (gesture);
				
				if(!Recent.ContainsKey(swipe.Id)) {
					Recent.Add(swipe.Id, new List<SwipeGesture>());
					tf_start = frame.Timestamp/1000000;
				}
				
				Recent[swipe.Id].Add(swipe);
			}
			
			if(i==gestures.Count-1)
				tf_start = frame.Timestamp/1000000;
		}
		
		for (int i = 0; i < 2; i++)
		{
			if ((unityHands[i]).hand != null && unityHands[i].hand.Id == h.Id && unityHands[i].isHandDetermined)
			{
				unityHands[i].hand = h; //update the state of unity hand
				undeterminedHand = false;
			}
		}

		if (undeterminedHand)
			AssignHands(h);																		
	}

	private void OnHandLost(int Id)
	{
		Messenger.Broadcast<int>(SIG.HANDLOST.ToString(), Id);	// broadcast lost hand ID to registered listeners e.g. game objects, controllers, etc.
		
		for (int i = 0; i < 2; i++)
		{
			if (unityHands[i].hand != null && unityHands[i].hand.Id == Id)
			{
				unityHands[i].HandLost();
			}
		}
	}

	/// <summary>
	/// determines if detected leap hand is left or right
	/// </summary>
	/// <param name="h">leap input hand</param>
	private void AssignHands(Hand h)
	{										
		// delay left/right analysis for better accuracy
		if (h.TimeVisible < timeVisible)
			return;

		//if fingers are displayed, determine left / right based on thumb
		for (int i = 0; i < h.Fingers.Count; i++)
		{
			Finger f = h.Fingers[i];

			if (!f.IsValid) 
				continue; 

			Vector3 palmRightAxis = Vector3.Cross(h.Direction.ToUnity(), h.PalmNormal.ToUnity());
			float fingerDot = Vector3.Dot((f.TipPosition - h.PalmPosition).ToUnity().normalized, palmRightAxis.normalized);

			// is finger tip where we would expect the thumb to be?
			if (Mathf.Abs(fingerDot) > 0.9f)
			{
				int index = fingerDot > 0 ? 0 : 1;

				if (unityHands[index].hand != null && unityHands[index].hand.Id != h.Id)
				{
					if (unityHands[index].isHandDetermined)
					{
						SolveAmbiguity(index, h);
						return;
					}

					Debug.Log("OVERRIDING OLD HAND");
					unityHands[1 - index].isHandDetermined = true;
					unityHands[1 - index].AssignHand(unityHands[index].hand);
					unityHands[index].isHandDetermined = true;
					unityHands[index].AssignHand(h);
					
					return;
				}

				unityHands[index].AssignHand(h);
				unityHands[index].isHandDetermined = true;

				// If this hand was previously the opposite hand, call HandLost on opposite hand
				if (unityHands[1 - index].hand != null && unityHands[index].hand.Id == unityHands[1 - index].hand.Id)
				{
					unityHands[1 - index].HandLost();
				}
				return;
			}
		}

		for (int i = 0; i < 2; i++)
		{
			if (unityHands[i].hand != null && unityHands[i].hand.Id == h.Id)
			{
				unityHands[i].AssignHand(h);
				return;
			}
		}

		//if finger analysis did not determine a hand, make an assumption based on position
		AssumeHand(h);
	}
	
	/// <summary>
	/// assume left / right hand based on absolute leap position
	/// </summary>
	/// <param name="h">hand to be solved</param>
	private void AssumeHand(Hand h)
	{
		int index = h.PalmPosition.x < 0 ? 0 : 1;

		if (unityHands[index].hand == null)
			unityHands[index].AssignHand(h);
		else
			SolveAmbiguity(index, h);
	}

	/// <summary>
	/// Solve Hand ambiguity (two right hands or two left hands)
	/// </summary>
	/// <param name="conflictIndex"> Index of conflict with new hand</param>
	/// <param name="h"></param>
	private void SolveAmbiguity(int conflictIndex, Hand h)
	{
		Hand leftMost = h.PalmPosition.x < unityHands[conflictIndex].hand.PalmPosition.x ? h : unityHands[conflictIndex].hand;
		Hand rightMost = leftMost.Id == h.Id ? unityHands[conflictIndex].hand : h;

		unityHands[0].hand = leftMost;
		unityHands[1].hand = rightMost;
	}
	
	private void ResetConsecutive() {
		c_count = 0;
		c_start = -1;
		c_elapsed = 0;
	}

	private void ResetTimeFrame() {
		tf_start = -1;
		tf_elapsed = 0;
		Recent.Clear();
	}
	
	private void OnDestroy()
	{
		//necessary to clean delegate assignment between scenes
		LeapInputEx.HandFound -= OnHandFound;
		LeapInputEx.HandUpdated -= OnHandUpdated;
		LeapInputEx.HandLost -= OnHandLost;
	}
}

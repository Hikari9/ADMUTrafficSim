using UnityEngine;
using System.Collections;

public class GameGUI : MonoBehaviour {
	protected bool Pausing = false, GameOver = false;
	protected System.DateTime time;
	public float gameSeconds = 60 * 3;
	
	public bool Pausable = true;
	
	int collisions = 0;
	bool IsColliding = false;
	
	public void AddCollision() {
		collisions++;
		StartCoroutine (Collide ());
	}
	
	IEnumerator Collide() {
		IsColliding = true;
		yield return new WaitForSeconds (3f);
		IsColliding = false;
	}
	
	// Use this for initialization
	void Start () {
		this.time = System.DateTime.Now;
	}
	
	// Update is called once per frame
	void Update () {
		if (!Pausing) {
			if (gameSeconds <= 0) {
				gameSeconds = 0;
				GameOver = true;
			}
			else {
				gameSeconds -= Time.deltaTime;
			}
		}
		
		if (!GameOver && Input.GetKeyDown (KeyCode.Space))
			Pausing ^= true;
		else if (GameOver && Input.anyKeyDown) {
			Application.Quit ();
			// Command.LoadLevel ("Main Menu");
		}
		
		Time.timeScale = (Pausing || GameOver) ? 0f : 1f;
	}
	
	void drawString(object label, float x, float y, int fontSize, Color fontColor) {
		GUIStyle style = new GUIStyle(GUI.skin.GetStyle("label"));
		style.fontSize = fontSize;
		style.onFocused.textColor =
			style.onActive.textColor =
				style.onHover.textColor =
				style.onNormal.textColor =
				style.normal.textColor = fontColor;
		GUI.Label (new Rect(x, y, Screen.width, Screen.height), label.ToString (), style);
	}
	
	public void ShowTimer() {
		int remaining = (int) gameSeconds;
		string label = string.Format ("{0,2}:{1,2}", (remaining / 60).ToString ("0#"), (remaining % 60).ToString("0#"));
		drawString (label, 0, 0, 40, Color.white);
	}
	
	public long GetScore() {
		long sum = 0, product = 1;
		foreach (GameObject road in GameObject.FindGameObjectsWithTag("road")) {
			int score = Command.GetCarSpawner ().GetRoadScore (road);
			sum += score;
			if (score > 0) product *= score;
		}
		
		long totalScore = sum + (product == 1 ? 0 : product);
		return totalScore / (collisions + 1);
	}
	
	void OnGUI() {
		ShowTimer();
		ShowScore();
		ShowCollisionCount ();
		if (IsColliding)
			ShowPopupCollision ();
		if (GameOver) {
			ShowGameOver();
		}
		else if (Pausing && Pausable) {
			ShowPaused();
		}
	}
	
	/// Show GUI codes
	
	public void ShowScore() {
		drawString ("Score: " + GetScore (), 0, 50, 40, Color.white);
	}
	
	public void ShowCollisionCount() {
		drawString ("Collisions: " + collisions, 0, 100, 40, Color.white);
	}
	
	public void ShowPopupCollision() {
		drawString ("Car Collision!!", 0, 150, 40, Color.white);
	}
	
	public void ShowPaused() {
		drawString ("GAME PAUSED", (Screen.width)/2 -(Screen.width)/4, (Screen.height)/2-(Screen.height)/4, 80, Color.white);
		drawString ("Press space to continue", (Screen.width)/2 -(Screen.width)/4, 100+(Screen.height)/2-(Screen.height)/4, 40, Color.white);
	}
	
	public void ShowGameOver() {
		drawString ("GAME OVER", (Screen.width)/2 -(Screen.width)/4, (Screen.height)/2-(Screen.height)/4, 80, Color.white);
		drawString ("\nYour score is: " + GetScore (), (Screen.width)/2 -(Screen.width)/4, 100+(Screen.height)/2-(Screen.height)/4, 40, Color.white);
	}
	
}
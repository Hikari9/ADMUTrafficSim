using UnityEngine;
using System.Collections;

public class Glow : MonoBehaviour {

	Color targetColor, originalColor;
	public float colorFadeSpeed = 7.5f;
	public bool testRedGreen = false;

	// Use this for initialization
	void Start () {
		originalColor = targetColor = renderer.sharedMaterial.color;
	}
	
	// Update is called once per frame
	void Update () {

		if (testRedGreen) {
			if (Input.GetKey ("g")) {
					setColor (Color.green);
			} else if (Input.GetKey ("r")) {
					setColor (Color.red);
			} else if (Input.GetKey (KeyCode.Space)) {
					setColor (originalColor);
			}
		}

		Color currentColor = renderer.sharedMaterial.color;
		// Color deltaColor = new Color (targetColor.r - currentColor.r, targetColor.g - currentColor.g, targetColor.b - currentColor.b, targetColor.a - currentColor.a);            targetColor.a - currentColor.a);
		Color deltaColor = targetColor - currentColor;
		renderer.sharedMaterial.color += deltaColor * Time.smoothDeltaTime * colorFadeSpeed;
	}

	public void setColor(Color color) {
		this.targetColor = color;
	}
}

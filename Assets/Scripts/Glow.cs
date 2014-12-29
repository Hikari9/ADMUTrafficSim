using UnityEngine;
using System.Collections;

public class Glow : MonoBehaviour {

	public Color targetColor, originalColor;
	public float colorFadeSpeed = 7.5f;
	public bool testRedGreen = false;

	// Use this for initialization
	void Start () {
		originalColor = targetColor = this.gameObject.renderer.material.color;
	}
	
	// Update is called once per frame
	void Update () {

		if (testRedGreen) {
			if (Input.GetKey ("g")) {
				SetColor (Color.green);
			} else if (Input.GetKey ("r")) {
				SetColor (Color.red);
			} else if (Input.GetKey (KeyCode.Space)) {
				SetColor (originalColor);
			}
		}

		Color currentColor = this.gameObject.renderer.material.color;
		// Color deltaColor = new Color (targetColor.r - currentColor.r, targetColor.g - currentColor.g, targetColor.b - currentColor.b, targetColor.a - currentColor.a);            targetColor.a - currentColor.a);
		Color need = targetColor - currentColor;
		Color addend = need * Mathf.Min (1f, Time.deltaTime * colorFadeSpeed);
		this.gameObject.renderer.material.color += addend;
	}

	public void SetColor(Color color) {
		this.targetColor = color;
	}
}

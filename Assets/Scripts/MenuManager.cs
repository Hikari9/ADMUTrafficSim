using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour 
{
	public Menu CurrentMenu = null;

	public void Start()
	{
		ShowMenu (CurrentMenu);
	}

	public void ShowMenu (Menu menu)
	{
		if (CurrentMenu != null) {
			CurrentMenu.IsOpen = false;
			CurrentMenu.gameObject.SetActive (false);
		}

		CurrentMenu = menu;
		CurrentMenu.gameObject.SetActive (true);
		CurrentMenu.IsOpen = true;

	}


}

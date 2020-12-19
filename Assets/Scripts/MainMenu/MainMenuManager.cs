using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
	public GameObject mainMenu;
	public GameObject singleplayerMenu;
	public GameObject multiplayerMenu;

	[Header("Game Mode Descriptions")]
	public GameObject classicDescription;
	public GameObject countdownDescription;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
	public void BackToMainMenu(GameObject menu)
	{
		menu.SetActive(false);
		mainMenu.SetActive(true);
	}

	public void OpenMenu(GameObject menu)
	{
		menu.SetActive(true);
		mainMenu.SetActive(false);
	}

	public void OpenSubMenu(GameObject menu)
	{
		menu.SetActive(true);
	}
	public void DisableSubMenu(GameObject menu)
	{
		menu.SetActive(false);
	}

	public void Exit()
	{
		Application.Quit();
	}

	#region singleplayer

	public void SinglePlayerSelectClassic()
	{
		GameModeManager.Instance.SetCurrentSinglePlayerMode(SinglePlayerModes.Classic);
		ChangeDescriptionMenu(SinglePlayerModes.Classic);
	}
	public void SinglePlayerSelectCountdown()
	{
		GameModeManager.Instance.SetCurrentSinglePlayerMode(SinglePlayerModes.Countdown);
		ChangeDescriptionMenu(SinglePlayerModes.Countdown);
	}

	private void ChangeDescriptionMenu(SinglePlayerModes mode)
	{
		switch (mode)
		{
			case SinglePlayerModes.Countdown:
				classicDescription.SetActive(false);
				countdownDescription.SetActive(true);
				break;
			default:
				classicDescription.SetActive(true);
				countdownDescription.SetActive(false);
				break;
		}
	}

	#endregion
}

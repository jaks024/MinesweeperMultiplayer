using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public enum SinglePlayerModes
{
	Classic,
	Countdown
}

public class GameModeManager : MonoBehaviour
{
	public static GameModeManager Instance;

	[SerializeField] private SinglePlayerModes currentSinglePlayerMode = SinglePlayerModes.Classic;

	private void Awake()
	{
		if (Instance != this && Instance != null)
			Destroy(this.gameObject);
		Instance = this;
	}

	public void LoadGameMode()
	{

		switch (currentSinglePlayerMode)
		{
			case SinglePlayerModes.Countdown:
				Debug.Log("in countdown");
				break;
			default:
				Debug.Log("in classic");
				SceneManager.LoadScene("Gameplay", LoadSceneMode.Single);
				break;
		}
	}

	public void SetCurrentSinglePlayerMode(SinglePlayerModes mode)
	{
		currentSinglePlayerMode = mode;
	}
}

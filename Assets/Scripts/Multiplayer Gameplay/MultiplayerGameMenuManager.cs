using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiplayerGameMenuManager : MonoBehaviour
{
	public static MultiplayerGameMenuManager instance;


	[SerializeField] private GameObject countdownPanel;
	[SerializeField] private GameObject resultPanel;
	[SerializeField] private GameObject statsPanel;
	[SerializeField] private Text countdownText;
	[SerializeField] private Text resultText;
	[SerializeField] private Text playerResultText;
	[SerializeField] private Text lobbyWinnerText;
	[SerializeField] private Text lobbyBigText;
	[SerializeField] private GameObject restartButton;

	[Header("Partial")]
	[SerializeField] private GameObject partialResultPanel;
	[SerializeField] private Text partialResultText;
	[SerializeField] private Text partialResultContentText;
	private void Awake()
	{
		if (instance != this && instance != null)
			Destroy(this.gameObject);
		instance = this;
	}

	public void CountdownScreen()
	{
		countdownPanel.SetActive(true);
		resultPanel.SetActive(false);
	}

	public void ResultScreen()
	{
		resultPanel.SetActive(true);
		statsPanel.SetActive(false);
		partialResultPanel.SetActive(false);
	}
	public void PartialResultScren(string content, bool won)
	{
		if (won)
			partialResultText.text = "Cleared";
		else
			partialResultText.text = "Failed";
		partialResultPanel.SetActive(true);
		partialResultContentText.text = content;
		statsPanel.SetActive(false);
	}
	public void SetLobbyWinner(string name)
	{
		lobbyBigText.text = "Winner";
		lobbyWinnerText.text = name;
	}
	public void SetLobbyAllLose()
	{
		lobbyBigText.text = "Failed";
		lobbyWinnerText.text = "";
	}

	public void GameplayScreen()
	{
		countdownPanel.SetActive(false);
		resultPanel.SetActive(false);
		statsPanel.SetActive(true);
	}

	public void SetCountdownText(string txt)
	{
		countdownText.text = txt;
	}

	public void SetPlayerResultText(bool won)
	{
		if (won)
			playerResultText.text = "Cleared";
		else
			playerResultText.text = "Failed";
	}

	public void SetLobbyResultText(string txt)
	{
		resultText.text = txt;
	}

	public void DisableRestartButton()
	{
		restartButton.SetActive(false);
	}
}

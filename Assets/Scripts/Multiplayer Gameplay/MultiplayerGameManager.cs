using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class MultiplayerGameManager : MonoBehaviourPunCallbacks
{
	public static MultiplayerGameManager instance;

	[SerializeField] private GridMaker gm;
	private float countdownTime = 3;
	private float currentCountdown = 3;
	private bool isCountdown;

	private int playerCleared = 0;
	private int playerCount = 0;
	private bool inResult;
	private List<MultiplayerPlayerResult> playerResults = new List<MultiplayerPlayerResult>();

	private void Awake()
	{
		if (instance != this && instance != null)
			Destroy(this.gameObject);
		instance = this;
	}

	private void Start()
	{
		playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
		if (PhotonNetwork.IsMasterClient)
			StartCountdown();
		else
			MultiplayerGameMenuManager.instance.DisableRestartButton();
		MultiplayerGameMenuManager.instance.CountdownScreen();
	}

	[PunRPC]
	private void SetPasserValue(int w, int h, int c)
	{
		SceneValuePasser.SetValues(w, h, c);
		gm.InitializeGameScreenMultiplayer();
	}

	[PunRPC]
	private void SetGridMines(string[] mines, int x, int y)
	{
		gm.AssignMinesAndValues(mines, x, y);
	}

	private void Countdown()
	{
		currentCountdown = Mathf.Clamp(currentCountdown - Time.deltaTime, 0, countdownTime);
		MultiplayerGameMenuManager.instance.SetCountdownText(currentCountdown.ToString("F2"));
		photonView.RPC("UpdateCountdown", RpcTarget.OthersBuffered, currentCountdown);
		if (currentCountdown == 0)
		{
			currentCountdown = 3;
			isCountdown = false;
			MultiplayerGameMenuManager.instance.GameplayScreen();
			InGameStateManager.Instance.SetGameStarted();
		}
	}

	[PunRPC]
	private void UpdateCountdown(float v)
	{
		currentCountdown = v;
		MultiplayerGameMenuManager.instance.SetCountdownText(currentCountdown.ToString("F2"));
		if (currentCountdown == 0)
		{
			MultiplayerGameMenuManager.instance.GameplayScreen();
			InGameStateManager.Instance.SetGameStarted();
		}
	}

	private void StartCountdown()
	{
		isCountdown = true;
		MultiplayerGameMenuManager.instance.CountdownScreen();
		Debug.Log("multiplayer " + IsMultiplayer.isMultiplayer);

		gm.InitializeGameScreenMultiplayerHost();
		photonView.RPC("SetPasserValue", RpcTarget.OthersBuffered, SceneValuePasser.gridWidth, SceneValuePasser.gridHeight, SceneValuePasser.mineCount);
		photonView.RPC("SetGridMines", RpcTarget.OthersBuffered, gm.minesCoordinate, gm.randXClick, gm.randYClick);
	}

	[PunRPC]
	private void LeaveGame()
	{
		StartCoroutine(Leave());
	}

	private IEnumerator Leave()
	{
		PhotonNetwork.LeaveRoom();
		PhotonNetwork.LeaveLobby();
		PhotonNetwork.Disconnect();
		Debug.Log("disconnected");
		IsMultiplayer.isMultiplayer = false;
		yield return new WaitForSeconds(0.5f);
		SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
	}

	public void LeaveGameOnClick()
	{
		photonView.RPC("LeaveGame", RpcTarget.All);
	}

	public void ResetGameOnClick()
	{
		gm.ResetGameMultiplayer();

		playerCleared = 0;
		playerResults.Clear();
		inResult = false;

		isCountdown = true;
		MultiplayerGameMenuManager.instance.CountdownScreen();

		photonView.RPC("ResetGameClient", RpcTarget.OthersBuffered);
		photonView.RPC("SetGridMines", RpcTarget.OthersBuffered, gm.minesCoordinate, gm.randXClick, gm.randYClick);
	}

	[PunRPC]
	private void ResetGameClient()
	{
		gm.ResetGameMultiplayerClient();

		playerCleared = 0;
		playerResults.Clear();
		inResult = false;

		isCountdown = true;
		MultiplayerGameMenuManager.instance.CountdownScreen();
	}

	public void ClearedResponse(float time)
	{
		photonView.RPC("PlayerCleared", RpcTarget.All, time, PhotonNetwork.LocalPlayer.NickName);
		MultiplayerGameMenuManager.instance.PartialResultScren("Time: " + time, true);
	}
	public void FailedResponse(float time, int count)
	{
		photonView.RPC("PlayerFailed", RpcTarget.All, time, PhotonNetwork.LocalPlayer.NickName, count);
		MultiplayerGameMenuManager.instance.PartialResultScren("Time: " + time + "\tMines Left: " + count, false);
	}
	
	[PunRPC]
	private void PlayerCleared(float time, string name)
	{
		playerCleared++;
		playerResults.Add(new MultiplayerPlayerResult(name, time, true));
	}
	[PunRPC]
	private void PlayerFailed(float time, string name, int count)
	{
		playerCleared++;
		playerResults.Add(new MultiplayerPlayerResult(name, time, false, count));
	}

	private void Update()
	{
		if(playerCleared == playerCount && !inResult)
		{
			MultiplayerGameMenuManager.instance.ResultScreen();
			inResult = true;
			string result = "";
			MultiplayerPlayerResult winner = playerResults[0];
			bool allFailed = true;
			for(int i = 0; i < playerResults.Count; i++)
			{
				result += playerResults[i].ToString() + "\n";
				if(playerResults[i].cleared)
				{
					allFailed = false;
					if(playerResults[i].time < winner.time)
					{
						winner = playerResults[i];
					}
				}
				
			}
			MultiplayerGameMenuManager.instance.SetLobbyResultText(result);
			if(!allFailed)
				MultiplayerGameMenuManager.instance.SetLobbyWinner(winner.name);
			else
				MultiplayerGameMenuManager.instance.SetLobbyAllLose();
		}

		if (!PhotonNetwork.IsMasterClient)
			return;

		if (isCountdown && currentCountdown > 0)
			Countdown();
	}

}

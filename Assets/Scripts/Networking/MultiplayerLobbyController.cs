using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
public class MultiplayerLobbyController : MonoBehaviourPunCallbacks
{
	private int roomSize = 4;
	[SerializeField] private string joinRoomName = "";
	[SerializeField] private string newRoomName = "";
	[SerializeField] private InputField nameInput;
	private void Start()
	{
		nameInput.text = "Player " + Random.Range(0, 1000000);
		PhotonNetwork.NickName = nameInput.text;
	}
	public override void OnConnectedToMaster()
	{
		Debug.Log("connected to server");
		PhotonNetwork.JoinLobby();
		IsMultiplayer.isMultiplayer = true;
	}
	public void ConnectToServer()
	{
		PhotonNetwork.AutomaticallySyncScene = true;
		PhotonNetwork.ConnectUsingSettings();
	}

	public override void OnJoinRoomFailed(short returnCode, string message)
	{
		Debug.Log("failed to join: " + message);
		//show message
	}

	public override void OnCreateRoomFailed(short returnCode, string message)
	{
		Debug.Log("failed to create room");
	}
	public override void OnCreatedRoom()
	{
		Debug.Log("room has been created");
	}

	public void SetRoomNameByInput(string input)
	{
		newRoomName = input;
	}
	
	public void SetJoinRoomNameByInput(string input)
	{
		joinRoomName = input;
	}

	public void CreateNewRoom()
	{
		if (!PhotonNetwork.IsConnectedAndReady || !PhotonNetwork.IsConnected || !IsMultiplayer.isMultiplayer)
			return;

		RoomOptions ops = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)roomSize};
		PhotonNetwork.CreateRoom(newRoomName, ops, TypedLobby.Default);
		Debug.Log("room " + newRoomName + " has been created");
	}

	public void JoinRoom()
	{
		if (!PhotonNetwork.IsConnectedAndReady || !PhotonNetwork.IsConnected || !IsMultiplayer.isMultiplayer)
			return;

		Debug.Log("joinning room " + joinRoomName);
		PhotonNetwork.JoinRoom(joinRoomName);
	}

	public void LeaveRoom()
	{
		Debug.Log("leaving room");
		PhotonNetwork.LeaveRoom();
	}

	public void LeaveMultiplayer()
	{
		
		if (!PhotonNetwork.IsConnectedAndReady || !PhotonNetwork.IsConnected || !IsMultiplayer.isMultiplayer)
			return;
		IsMultiplayer.isMultiplayer = false;
		PhotonNetwork.LeaveLobby();
		PhotonNetwork.Disconnect();
		Debug.Log("disconnected");
	}

	public void SetPlayerName(InputField input)
	{
		if (string.IsNullOrWhiteSpace(input.text))
		{
			input.text = "Player " + Random.Range(0, 1000000);
			PhotonNetwork.NickName = input.text;
		}
		else
			PhotonNetwork.NickName = input.text;
	}

}

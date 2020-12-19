using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
public class MultiplayerRoomController : MonoBehaviourPunCallbacks
{
	[SerializeField] private GameObject roomPage;
	[SerializeField] private GameObject lobbyPage;
	[SerializeField] private GameObject playerItemPrefab;
	[SerializeField] private Transform playerListParent;
	[SerializeField] private Text roomNameText;

	public override void OnEnable()
	{
		PhotonNetwork.AddCallbackTarget(this);
	}

	public override void OnDisable()
	{
		PhotonNetwork.RemoveCallbackTarget(this);
	}

	public override void OnJoinedRoom()
	{
		Debug.Log("joined a room");
		roomPage.SetActive(true);
		lobbyPage.SetActive(false);

		roomNameText.text = PhotonNetwork.CurrentRoom.Name;

		RefreshPlayerList();
	}

	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		RefreshPlayerList();
	}

	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		RefreshPlayerList();
	}

	public override void OnLeftRoom()
	{
		roomPage.SetActive(false);
		lobbyPage.SetActive(true);
	}

	private void RefreshPlayerList()
	{
		ClearListedPlayers();
		ListPlayers();
	}

	private void ClearListedPlayers()
	{
		for (int i = playerListParent.childCount - 1; i >= 0; i--)
			Destroy(playerListParent.GetChild(i).gameObject);
	}

	private void ListPlayers()
	{
		foreach(Player p in PhotonNetwork.PlayerList)
		{
			GameObject pic = Instantiate(playerItemPrefab, playerListParent);
			pic.GetComponent<PlayerItemController>().SetPlayerName(p.NickName, p.IsMasterClient);
		}
	}

	public void LeaveRoom()
	{
		PhotonNetwork.LeaveRoom();
		StartCoroutine(RejoinLobby());
	}

	private IEnumerator RejoinLobby()
	{
		yield return new WaitForSeconds(1);
		PhotonNetwork.JoinLobby();
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class MultiplayerRoomSettingController : MonoBehaviourPunCallbacks
{
	[SerializeField] private GameObject clientPanel;
	[SerializeField] private GameObject hostPanel;

	private int width = 8;
	private int height = 8;
	private int mine = 8;

	[Header("Clients")]
	[SerializeField] private Text gridDimensionText;
	[SerializeField] private Text mineCountText;
	private bool isReady = false;

	[Header("Host")]
	[SerializeField] private Text widthText;
	[SerializeField] private Text heightText;
	[SerializeField] private Text mineText;
	[SerializeField] private Slider mineSlider;
	[SerializeField] private GameObject startButton;
	private int readiedPlayers = 0;

	public override void OnJoinedRoom()
	{
		if (PhotonNetwork.IsMasterClient)
		{
			clientPanel.SetActive(false);
			hostPanel.SetActive(true);
			photonView.RPC("SetAllRoomValues", RpcTarget.AllBufferedViaServer, width, height, mine);
		}
		else
		{
			clientPanel.SetActive(true);
			hostPanel.SetActive(false);
			startButton.SetActive(false);
		}
	}

	private void UpdateMineSlider()
	{
		if (width * height - 9 == 0)
			mineSlider.maxValue = 1;
		else
			mineSlider.maxValue = width * height - 9;
	}

	public void ChangeWidthBySlider(float w)
	{
		width = (int)w;
		photonView.RPC("SetRoomWidthValues", RpcTarget.AllBufferedViaServer, width);
		widthText.text = "Grid Width: " + w;
		UpdateMineSlider();
	}
	public void ChangeHeightBySlider(float h)
	{
		height = (int)h;
		photonView.RPC("SetRoomHeightValues", RpcTarget.AllBufferedViaServer, height);
		heightText.text = "Grid Height: " + h;
		UpdateMineSlider();
	}
	public void ChangeMineCountBySlider(float c)
	{
		mine = (int)c;
		photonView.RPC("SetRoomMineValues", RpcTarget.AllBufferedViaServer, mine);
		mineText.text = "Mines: " + c;
	}

	[PunRPC]
	private void SetAllRoomValues(int w, int h, int c)
	{
		width = w;
		height = h;
		mine = c;
	}

	[PunRPC]
	private void SetRoomWidthValues(int w)
	{
		width = w;
		gridDimensionText.text = "Grid Size: " + width + " x " + height;
	}
	[PunRPC]
	private void SetRoomHeightValues(int h)
	{
		height = h;
		gridDimensionText.text = "Grid Size: " + width + " x " + height;
	}
	[PunRPC]
	private void SetRoomMineValues(int m)
	{
		mine = m;
		mineCountText.text = "Mines: " + mine;
	}

	public void StartGame()
	{
		SceneValuePasser.SetValues(width, height, mine);
		StartCoroutine(ChangeToGameScene());
	}

	private IEnumerator ChangeToGameScene()
	{
		PhotonNetwork.CurrentRoom.IsOpen = false;

		PhotonNetwork.IsMessageQueueRunning = false;
		PhotonNetwork.LoadLevel("GameplayMultiplayer");
		while (PhotonNetwork.LevelLoadingProgress < 1)
		{
			Debug.Log(PhotonNetwork.LevelLoadingProgress);
			yield return null;
		}
		PhotonNetwork.IsMessageQueueRunning = true;

		SceneManager.LoadScene("GameplayMultiplayer");
	}
}

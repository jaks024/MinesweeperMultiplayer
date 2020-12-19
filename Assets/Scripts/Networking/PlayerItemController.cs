using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerItemController : MonoBehaviour
{
	[SerializeField] private Text playerNameText;
	private string playerName;
	private bool isHost;

	public void SetPlayerName(string name, bool host)
	{
		playerName = name;
		isHost = host;
		playerNameText.text = playerName;

		if (isHost)
			playerNameText.color = new Color(255, 200, 0);
	}
}

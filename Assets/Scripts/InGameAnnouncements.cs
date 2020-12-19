using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameAnnouncements : MonoBehaviour
{
	public static InGameAnnouncements Instance;

	[SerializeField] private Text gameResultMessage;
    void Awake()
    {
		if (Instance != this && Instance != null)
			Destroy(this.gameObject);
		Instance = this;
    }

	public void SetClearedMessage()
	{
		gameResultMessage.text = "Cleared";
		gameResultMessage.gameObject.SetActive(true);
	}
	public void SetFailedMessage()
	{
		gameResultMessage.text = "Failed";
		gameResultMessage.gameObject.SetActive(true);
	}

	public void DisableMessages()
	{
		gameResultMessage.gameObject.SetActive(false);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InGameStateManager : MonoBehaviour
{
	public static InGameStateManager Instance;

	private float playTime = 0f;
	private int mineCount = 0;
	[SerializeField]private bool gameStarted = false;

	[SerializeField] private Text bombText;
	[SerializeField] private Text playTimeText;
    void Start()
    {
		if (Instance != this && Instance != null)
			Destroy(this);
		Instance = this;
    }

    void Update()
    {
		if(gameStarted)
			UpdatePlayTime();
    }

	public float GetPlayTime()
	{
		return playTime;
	}
	public int GetMineCount()
	{
		return mineCount;
	}
	public void SetGameStarted()
	{
		gameStarted = true;
		ResetPlayTime();
	}
	public void SetGameStopped()
	{
		gameStarted = false;
	}
	public bool GetGameStarted()
	{
		return gameStarted;
	}

	public void SetMineCount(int count)
	{
		mineCount = count;
		bombText.text = mineCount.ToString();
	}

	public void ResetGame()
	{
		SetGameStopped();
		ResetPlayTime();
	}

	private void UpdatePlayTime()
	{
		playTime += Time.deltaTime;
		SetPlayTimeText();
	}
	private void ResetPlayTime()
	{
		playTime = 0;
		SetPlayTimeText();
	}

	private void SetPlayTimeText()
	{
		playTimeText.text = Helper.SecondToHHMMSS(playTime);
	}
}

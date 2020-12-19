using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMaker : MonoBehaviour
{
	[SerializeField] private int gridWidth = 8;
	[SerializeField] private int gridHeight = 8;
	public float blockSize = 1.1f;
	[SerializeField] private int bombCount = 8;
	public BlockObject[,] grid;

	public string[] minesCoordinate;
	public int randXClick = 0;
	public int randYClick = 0;
	public bool isHost;

	public Transform gridParent;
	public GameObject blockPrefab;
	public bool firstClick;

	public int unmarkedBombCount = 0;
	public int unopenedBlockCount = 0;

	private void Awake()
	{
		
	}

	void Start()
    {
		if (!IsMultiplayer.isMultiplayer)
			InitializeGameScreen();

		//FindGridParent();
		//InitializeGrid();
		//if (gridWidth > 0 && gridHeight > 0)
		//{
		//	AssignMines();
		//	AssignValues();
		//}
	}

	#region multiplayer functions
	public void InitializeGameScreenMultiplayer()
	{
		gridWidth = SceneValuePasser.gridWidth;
		gridHeight = SceneValuePasser.gridHeight;
		bombCount = SceneValuePasser.mineCount;

		InitializeGrid();
		CenterGrid();
	}
	public void InitializeGameScreenMultiplayerHost()
	{
		gridWidth = SceneValuePasser.gridWidth;
		gridHeight = SceneValuePasser.gridHeight;
		bombCount = SceneValuePasser.mineCount;

		InitializeGrid();
		CenterGrid();

		Debug.Log("CALLED ON CLIENT");
		randXClick = Random.Range(0, grid.GetLength(0));
		randYClick = Random.Range(0, grid.GetLength(1));
		AssignMines(randXClick, randYClick);
		AssignValues();
		unopenedBlockCount = gridWidth * gridHeight - bombCount;
		unmarkedBombCount = bombCount;

		BlockLeftClickMultiplayerInitial(grid[randXClick, randYClick].GetBlock());
	}
	public void AssignMinesAndValues(string[] mc, int x, int y)
	{
		AssignMinesMultiplayer(mc);
		AssignValues();
		unopenedBlockCount = gridWidth * gridHeight - bombCount;
		unmarkedBombCount = bombCount;
		BlockLeftClickMultiplayerInitial(grid[x, y].GetBlock());
	}

	public void AssignMinesMultiplayer(string[] mc)
	{
		for(int i = 0; i < mc.Length; i++)
		{
			Debug.Log("i: " + i + " " + mc[i]);
			string[] x = mc[i].Split('.');
			grid[int.Parse(x[0]), int.Parse(x[1])].GetBlock().SetAsMine();
		}
	}

	public void BlockLeftClickMultiplayerInitial(Block b)
	{
		b.Clicked();
		if (b.GetValue() == 0)
		{
			int x = b.GetGridPos()[0];
			int y = b.GetGridPos()[1];
			StartCoroutine(OpenRelativeBlocks(x, y, grid[x, y]));
		}
		unopenedBlockCount = unopenedBlockCount > 0 ? unopenedBlockCount - 1 : 0;
		firstClick = true;

		InGameStateManager.Instance.SetMineCount(unmarkedBombCount);
	}

	private void VictoryResponseMultiplayer()
	{
		MultiplayerGameManager.instance.ClearedResponse(InGameStateManager.Instance.GetPlayTime());
		StartCoroutine(RevealAllBlocks());
		InGameStateManager.Instance.SetGameStopped();
		MultiplayerGameMenuManager.instance.SetPlayerResultText(true);
	}
	private void DefeatResponseMultiplayer()
	{
		MultiplayerGameManager.instance.FailedResponse(InGameStateManager.Instance.GetPlayTime(), InGameStateManager.Instance.GetMineCount());
		StartCoroutine(RevealAllBlocks());
		InGameStateManager.Instance.SetGameStopped();
		MultiplayerGameMenuManager.instance.SetPlayerResultText(false);
	}

	public void ResetGameMultiplayer()
	{
		Debug.Log("resetted");
		ResetAllBlock();
		InGameStateManager.Instance.SetMineCount(bombCount);
		InGameStateManager.Instance.ResetGame();

		randXClick = Random.Range(0, grid.GetLength(0));
		randYClick = Random.Range(0, grid.GetLength(1));
		AssignMines(randXClick, randYClick);
		AssignValues();
		unopenedBlockCount = gridWidth * gridHeight - bombCount;
		unmarkedBombCount = bombCount;

		BlockLeftClickMultiplayerInitial(grid[randXClick, randYClick].GetBlock());
	}

	public void ResetGameMultiplayerClient()
	{
		ResetAllBlock();
		InGameStateManager.Instance.SetMineCount(bombCount);
		InGameStateManager.Instance.ResetGame();
	}

	#endregion


	#region game functions
	private void InitializeValuesOnClick(int x, int y)
	{
		Debug.Log("Called on client click");
		AssignMines(x, y);
		AssignValues();
		unopenedBlockCount = gridWidth * gridHeight - bombCount;
		unmarkedBombCount = bombCount;
	}

	private void InitializeGrid()
	{
		grid = new BlockObject[gridWidth, gridHeight];
		for(int x = 0; x < grid.GetLength(0); x++)
		{
			for(int y = 0; y < grid.GetLength(1); y++)
			{
				Vector2 pos = new Vector2(gridParent.position.x + blockSize * x, gridParent.position.y + blockSize * y);
				GameObject go = Instantiate(blockPrefab, pos, Quaternion.identity, gridParent);
				go.name = x + ", " + y;
				Block b = go.GetComponent<Block>();
				b.SetGridPos(x, y);

				grid[x, y] = new BlockObject(go, b);
			}
		}
	}

	private void AssignMines(int nx = 0, int ny = 0)
	{
		Debug.Log("CALLED ");
		minesCoordinate = new string[bombCount];
		for (int i = 0; i < bombCount; i++)
		{
			int x = Random.Range(0, gridWidth);
			int y = Random.Range(0, gridHeight);
			if (!grid[x, y].GetBlock().GetIsMine())
			{
				bool skip = false;
				for (var z = nx - 1; z <= nx + 1; z++)
				{
					for (var j = ny - 1; j <= ny + 1; j++)
					{
						if (z == x && j == y)
							skip = true;
					}
				}
				if (skip)
				{
					i--;
					continue;
				}
				grid[x, y].GetBlock().SetAsMine();
				minesCoordinate[i] = x.ToString() + "." + y.ToString();
			}
			else
				i--;
		}
	}

	private void AssignValues()
	{
		for(int x = 0; x < grid.GetLength(0); x++)
		{
			for(int y = 0; y < grid.GetLength(1); y++)
			{
				if (!grid[x, y].GetBlock().GetIsMine())
					grid[x, y].GetBlock().SetValue(AdjacentMineCount(x, y));
				else
					grid[x, y].GetBlock().SetValue(-1);
			}
		}
	}

	private int AdjacentMineCount(int x, int y)
	{
		int count = 0;
		for (int i = x - 1; i <= x + 1; i++) {
			for (int j = y - 1; j <= y + 1; j++) {
				if (i >= 0 && i < grid.GetLength(0) && j >= 0 && j < grid.GetLength(1)) {
					if (grid[i, j].GetBlock().GetIsMine())
						count++;
				}
			}
		}
		return count;
	}

	private void CheckFirstClick(Block b)
	{
		if (firstClick)
			return;
		firstClick = true;
		InitializeValuesOnClick(b.GetGridPos()[0], b.GetGridPos()[1]);
		InGameStateManager.Instance.SetGameStarted();
		InGameStateManager.Instance.SetMineCount(unmarkedBombCount);
	}

	public void BlockLeftClickReceive(Block b)
	{
		CheckFirstClick(b);

		if (b.GetIsMarked())
			return;

		if (b.GetIsMine())
		{
			if (!IsMultiplayer.isMultiplayer)
				DefeatResponse();
			else
				DefeatResponseMultiplayer();
		}
		else
		{
			b.Clicked();
			if (b.GetValue() == 0)
			{
				int x = b.GetGridPos()[0];
				int y = b.GetGridPos()[1];
				StartCoroutine(OpenRelativeBlocks(x, y, grid[x, y]));
			}
			unopenedBlockCount = unopenedBlockCount > 0 ? unopenedBlockCount - 1 : 0;
		}
		
	}

	public void BlockRightClickReceive(Block b)
	{
		if (!firstClick || b.GetClicked())
			return;

		if (b.GetIsMarked())
		{
			b.SetMarked(false);
			unmarkedBombCount = unmarkedBombCount < bombCount ? unmarkedBombCount + 1 : bombCount;
		}
		else
		{
			b.SetMarked(true);
			unmarkedBombCount = unmarkedBombCount > 0 ? unmarkedBombCount - 1 : 0;
		}

		InGameStateManager.Instance.SetMineCount(unmarkedBombCount);
	}

	private IEnumerator OpenRelativeBlocks(int x, int y, BlockObject bo)
	{
		for (var i = x - 1; i <= x + 1; i++)
		{
			for (var j = y - 1; j <= y + 1; j++)
			{
				if (i >= 0 && i < grid.GetLength(0) && j >= 0 && j < grid.GetLength(1))
				{
					yield return new WaitForSeconds(0.005f);
					if (grid[i, j].GetBlock().GetValue() == 0 && !grid[i, j].GetBlock().GetClicked() && grid[i, j] != bo)
					{
						StartCoroutine(OpenRelativeBlocks(i, j, grid[i, j]));
					}
					if(!grid[i, j].GetBlock().GetClicked())
					{
						grid[i, j].GetBlock().Clicked();
						unopenedBlockCount--;
					}
					
				}
			}
		}
		
	}

	private IEnumerator RevealAllBlocks()
	{
		for(int x = 0; x < grid.GetLength(0); x++)
		{
			for (int y = 0; y < grid.GetLength(1); y++)
			{
				grid[x, y].GetBlock().Clicked();
			}
			yield return new WaitForSeconds(0.001f);
		}
		unopenedBlockCount = 0;
	}

	private void ResetAllBlock()
	{
		for (int x = 0; x < grid.GetLength(0); x++)
		{
			for (int y = 0; y < grid.GetLength(1); y++)
			{
				grid[x, y].GetBlock().ResetValues();
			}
		}
		firstClick = false;
	}

	private bool CheckAllBombsMarked()
	{
		int count = 0;
		for (int x = 0; x < grid.GetLength(0); x++)
		{
			for (int y = 0; y < grid.GetLength(1); y++)
			{
				if (grid[x, y].GetBlock().GetClicked())
					continue;
				if (grid[x, y].GetBlock().GetIsMine() && grid[x, y].GetBlock().GetIsMarked())
					count++;
			}
		}
		if (count == bombCount)
			return true;
		return false;
	}
    
	private void VictoryResponse()
	{
		StartCoroutine(RevealAllBlocks());
		InGameStateManager.Instance.SetGameStopped();
		InGameAnnouncements.Instance.SetClearedMessage();
	}
	private void DefeatResponse()
	{
		StartCoroutine(RevealAllBlocks());
		InGameStateManager.Instance.SetGameStopped();
		InGameAnnouncements.Instance.SetFailedMessage();
	}

	public void ResetGame()
	{
		Debug.Log("resetted");
		ResetAllBlock();
		InGameStateManager.Instance.SetMineCount(bombCount);
		InGameStateManager.Instance.ResetGame();
		InGameAnnouncements.Instance.DisableMessages();
	}

	#endregion
	public void InitializeGameScreen()
	{
		gridWidth = SceneValuePasser.gridWidth;
		gridHeight = SceneValuePasser.gridHeight;
		bombCount = SceneValuePasser.mineCount;

		InitializeGrid();
		CenterGrid();
	}

	private void CenterGrid()
	{
		Vector2 centered = new Vector2(-(gridWidth * Mathf.RoundToInt(blockSize) / 2), -(gridHeight * Mathf.RoundToInt(blockSize) / 2));
		gridParent.position = centered;
		Debug.Log("centered");
	}

	public int GetGridWidth()
	{
		return gridWidth;
	}
	public int GetGridHeight()
	{
		return gridHeight;
	}
	public void SetGridDimension(int x, int y)
	{
		gridWidth = x;
		gridHeight = y;
	}
	public void SetBombCount(int count)
	{
		bombCount = count;
	}

    void Update()
    {
		if (gridParent == null)
			return;

		if (Input.GetKeyDown(KeyCode.Space))
		{
			ResetGame();
		}

		if (InGameStateManager.Instance.GetGameStarted())
		{
			if (unopenedBlockCount == 0)
			{
				Debug.Log("called1");
				if (CheckAllBombsMarked())
				{
					Debug.Log("called2");
					if (!IsMultiplayer.isMultiplayer)
						VictoryResponse();
					else
						VictoryResponseMultiplayer();
				}
			}
		}
    }
}

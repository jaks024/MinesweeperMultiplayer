using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MainMenuValueSetter : MonoBehaviour
{
	[SerializeField] private Slider mineSlider;

	[SerializeField] private Text widthText;
	[SerializeField] private Text heightText;
	[SerializeField] private Text mineText;

	private int gridWidth = 8;
	private int gridHeight = 8;
	public int mineCount = 8;

	// Start is called before the first frame update
	void Start()
    {
		mineCount = (int)mineSlider.value;
	}

    // Update is called once per frame
    void Update()
    {
		CapBombSliderValue();
	}

	private void CapBombSliderValue()
	{
		mineText.text = "Mines: " + mineSlider.value;
		
		if (gridWidth * gridHeight - 9 == 0)
			mineSlider.maxValue = 1;
		else
			mineSlider.maxValue = gridWidth * gridHeight - 9;
	}

	public void ChangeBombCountBySlider(float value)
	{
		mineCount = (int)value;
	}
	public void ChangeWidthBySlider(float value)
	{
		gridWidth = (int)value;
		widthText.text = "Grid Width: " + gridWidth;
	}
	public void ChangeHeightBySlider(float value)
	{
		gridHeight = (int)value;
		heightText.text = "Grid Height: " + gridHeight;
	}

	public void PlayOnClick()
	{
		SceneValuePasser.gridWidth = gridWidth;
		SceneValuePasser.gridHeight = gridHeight;
		SceneValuePasser.mineCount = mineCount;

		GameModeManager.Instance.LoadGameMode();
	}
}

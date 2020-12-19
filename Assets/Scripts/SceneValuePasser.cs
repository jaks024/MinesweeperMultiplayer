using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneValuePasser
{
	public static int gridWidth = 8;
	public static int gridHeight = 8;
	public static int mineCount = 8;

	public static void SetValues(int w, int h, int c)
	{
		gridWidth = w;
		gridHeight = h;
		mineCount = c;
	}
}

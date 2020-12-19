using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerPlayerResult
{
	public string name;
	public float time;
	public bool cleared;
	public int mineCount;

	public MultiplayerPlayerResult() { }

	public MultiplayerPlayerResult(string n, float t, bool c)
	{
		name = n;
		time = t;
		cleared = c;
	}
	public MultiplayerPlayerResult(string n, float t, bool c, int mc)
	{
		name = n;
		time = t;
		cleared = c;
		mineCount = mc;;
	}

	public override string ToString()
	{
		if (cleared)
			return string.Format("{0}: {1} - {2}", name, "Cleared", time.ToString("F2"));
		return (string.Format("{0}: {1} - {2}, {3} Mines Left", name, "Failed", time.ToString("F2"), mineCount));
	}
}

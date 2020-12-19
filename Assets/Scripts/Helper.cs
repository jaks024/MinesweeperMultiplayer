using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper
{
    public static string SecondToHHMMSS(float value)
	{
		return System.TimeSpan.FromSeconds(value).ToString(@"hh\:mm\:ss\:fff");
	}
}

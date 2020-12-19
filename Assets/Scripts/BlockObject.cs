using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockObject
{
	public GameObject gameObject;
	public Block block;
	public BlockObject (GameObject go, Block b)
	{
		gameObject = go;
		block = b;
	}

	public Block GetBlock()
	{
		return block;
	}
}

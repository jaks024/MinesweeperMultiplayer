using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFactory : MonoBehaviour
{
	public static SpriteFactory Instance;

	[SerializeField] private List<Sprite> numberSprites = new List<Sprite>();
	[SerializeField] private List<Sprite> bombSprites = new List<Sprite>();
	[SerializeField] private List<Texture2D> cursorSprite = new List<Texture2D>();
	[SerializeField] private List<Sprite> flagSprite = new List<Sprite>();
	[SerializeField] private List<Sprite> coverSprite = new List<Sprite>();
	void Awake()
    {
		if (Instance != null && Instance != this)
			Destroy(this);
		Instance = this;

		object[] objs = Resources.LoadAll("Numbers", typeof(Sprite));
		foreach(Sprite s in objs)
		{
			numberSprites.Add(s);
		}
		objs = Resources.LoadAll("Bombs", typeof(Sprite));
		foreach(Sprite s in objs)
		{
			bombSprites.Add(s);
		}
		objs = Resources.LoadAll("Cursors", typeof(Texture2D));
		foreach(Texture2D s in objs)
		{
			cursorSprite.Add(s);
		}
		objs = Resources.LoadAll("Flags", typeof(Sprite));
		foreach(Sprite s in objs)
		{
			flagSprite.Add(s);
		}
		objs = Resources.LoadAll("Covers", typeof(Sprite));
		foreach (Sprite s in objs)
		{
			coverSprite.Add(s);
		}
		DontDestroyOnLoad(this.gameObject);
	}

	public Sprite GetNumber(int number)
	{
		if (number < numberSprites.Count && number > -1)
			return numberSprites[number];
		else
			return numberSprites[0];
	}

	public Sprite GetBomb(int index)
	{
		if (index < bombSprites.Count && index > -1)
			return bombSprites[index];
		else
			return bombSprites[0];
	}

	public Texture2D GetCursor(int index)
	{
		if (index < cursorSprite.Count && index > -1)
			return cursorSprite[index];
		else
			return cursorSprite[0];
	}

	public Sprite GetFlag(int index)
	{
		if (index < flagSprite.Count && index > -1)
			return flagSprite[index];
		else
			return flagSprite[0];
	}

	public Sprite GetCover(int index)
	{
		if (index < coverSprite.Count && index > -1)
			return coverSprite[index];
		else
			return coverSprite[0];
	}
}

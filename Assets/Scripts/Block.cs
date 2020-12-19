using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
	public int[] gridPos = new int[2];
	public int value;
	public bool isMine;
	public bool clicked;
	public bool marked;

	[SerializeField] private SpriteRenderer coverSprite;
	[SerializeField] private SpriteRenderer contentSprite;
	private void Start()
	{
		//coverSprite = GetComponentInChildren<SpriteRenderer>();
		//contentSprite = coverSprite.gameObject.GetComponentInChildren<SpriteRenderer>();
		//Debug.Log("called");
	}

	public void SetGridPos(int x, int y)
	{
		gridPos[0] = x;
		gridPos[1] = y;
	}
	public int[] GetGridPos()
	{
		return gridPos;
	}
	public void SetAsMine()
	{
		isMine = true;
		//ChangeSpriteColor(Color.red);
		contentSprite.sprite = SpriteFactory.Instance.GetBomb(0);
	}
	public void SetValue(int v)
	{
		value = v;
		if(!isMine)
			contentSprite.sprite = SpriteFactory.Instance.GetNumber(v);
	}
	public int GetValue()
	{
		return value;
	}
	public bool GetIsMine()
	{
		return isMine;
	}
	public bool GetClicked()
	{
		return clicked;
	}
	private void ChangeSpriteColor(Color32 c)
	{
		coverSprite.color = c;
	}

	public void SetMarked(bool s)
	{
		if (clicked)
			return;

		marked = s;
		if (s)
			coverSprite.sprite = SpriteFactory.Instance.GetFlag(0);
		else
			coverSprite.sprite = SpriteFactory.Instance.GetCover(0);
	}

	public bool GetIsMarked()
	{
		return marked;
	}

	public void ResetValues()
	{
		value = 0;
		isMine = false;
		if(marked)
			coverSprite.sprite = SpriteFactory.Instance.GetCover(0);
		marked = false;
		if (clicked)
			coverSprite.enabled = true;
		clicked = false;
	}
	public void Clicked()
	{
		coverSprite.enabled = false;
		clicked = true;
	}
}

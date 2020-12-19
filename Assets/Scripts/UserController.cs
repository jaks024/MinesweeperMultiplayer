using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserController : MonoBehaviour
{
	private GridMaker gm;
    void Start()
    {
		gm = GameObject.FindGameObjectWithTag("gridParent").GetComponent<GridMaker>();
    }

	// Update is called once per frame
	void Update()
	{
		if (!InGameStateManager.Instance.GetGameStarted() && IsMultiplayer.isMultiplayer)
			return;

		Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		//cursorSprite.transform.parent.position = mousePos;

		if (Input.GetMouseButtonUp(0))
		{
			RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 0f);
			if (hit.collider != null)
			{
				Block b = hit.collider.gameObject.GetComponent<Block>();
				gm.BlockLeftClickReceive(b);
			}
		} 
		else if (Input.GetMouseButtonUp(1))
		{
			RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 0f);
			if (hit.collider != null)
			{
				Block b = hit.collider.gameObject.GetComponent<Block>();
				gm.BlockRightClickReceive(b);
			}
		}
    }

}

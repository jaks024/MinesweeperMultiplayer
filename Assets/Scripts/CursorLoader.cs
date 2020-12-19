using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		Texture2D cursor = SpriteFactory.Instance.GetCursor(1);
		Cursor.SetCursor(cursor, new Vector2(cursor.width / 2, cursor.height / 2), CursorMode.ForceSoftware);
	}
}

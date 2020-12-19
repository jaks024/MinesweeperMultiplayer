using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAdjuster : MonoBehaviour
{
	// Start is called before the first frame update
	private Camera cam;
	[SerializeField] private int minCamSize = 2;
	[SerializeField] private int maxCamSize = 10;
	[SerializeField] private float smoothness = 10f;
	private float target = 0;
	private Vector3 targetPos;
	private GridMaker gm;
	private bool adjusted;
    void Start()
    {
		gm = GameObject.FindGameObjectWithTag("gridParent").GetComponent<GridMaker>();

		StartCoroutine(WaitToAdjust());
	}

	private IEnumerator WaitToAdjust()
	{
		yield return new WaitForSeconds(0.25f);
		cam = Camera.main;
		target = (gm.GetGridWidth() + gm.GetGridHeight()) / 3f;
		maxCamSize = (int)target * 2;
		targetPos = transform.position;
		adjusted = true;
	}

    // Update is called once per frame
    void Update()
    {
		if (cam == null)
			return;
		cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, target, smoothness * Time.deltaTime);
		if (cam.orthographicSize >= target - 0.1f)
		{
			adjusted = true;
		}
		if (!adjusted)
			return;

		ChangeCameraSize();
		if (Input.GetMouseButton(2))
		{
			targetPos = cam.ScreenToWorldPoint(Input.mousePosition).normalized + transform.position;
		}
		MoveCamera();
	}

	private void ChangeCameraSize()
	{
		if(Input.mouseScrollDelta.y != 0)
			target = Mathf.Clamp(cam.orthographicSize + -Input.mouseScrollDelta.y, minCamSize, maxCamSize);
		cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, target, smoothness * Time.deltaTime);
	}

	private void MoveCamera()
	{

		transform.position = Vector3.Lerp(transform.position, new Vector3(targetPos.x, targetPos.y, -10), smoothness * Time.deltaTime);
	}
}

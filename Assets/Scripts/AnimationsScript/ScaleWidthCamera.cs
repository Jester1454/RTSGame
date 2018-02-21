using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ScaleWidthCamera : MonoBehaviour
{

	public int targetWidth = 1024;
	public float pixelsToUnit = 100;
	
	void Awake()
	{
		int height = Mathf.RoundToInt(targetWidth / (float) Screen.width * Screen.height);

		Camera.main.orthographicSize = height / pixelsToUnit / 2;
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShotManager : MonoBehaviour {

	// Use this for initialization
	void Awake ()
	{
		DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.P))
		{
			Debug.LogError(Application.persistentDataPath);
			ScreenCapture.CaptureScreenshot("screen.png");
		}	
	}
}

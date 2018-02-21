using System.Collections;
using System.Collections.Generic;
using ObjectBehavior;
using UnityEngine;

public class InputManager : MonoBehaviour 
{

#if UNITY_EDITOR
	public CameraControlMouse cameraMouse;
#endif
	public CameraControlTouch cameraTouch;
    static public InputManager instance;
	
    private void Awake()
    {
		if (instance == null)
		{
			instance = this;
		}
		else
			if (instance != this)
		{
			Destroy(gameObject);
		}
#if UNITY_EDITOR
		cameraMouse = GetComponent<CameraControlMouse>();
		cameraTouch.Attach();
#endif
		cameraTouch = GetComponent<CameraControlTouch>();
        cameraTouch.Attach();
    }
}

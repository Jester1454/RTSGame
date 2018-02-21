using System;
using System.Collections;
using System.Collections.Generic;
using ObjectBehavior;
using UnityEngine;

// TODO Задокументировать
// TODO Обработка двойного клика
public class CameraControlMouse : AbstractCameraControlMouse {

	private float dragTreshold = 10f;
	public GameObject selectedObject;


	protected override void  Awake() 
	{
		Attach ();
	}

	#if UNITY_EDITOR
	void Update () 
	{
		if (attached)
		{
			if (Input.GetMouseButtonDown (0))
			{
				onMouseBtnDown ();
			}
			if (Input.GetMouseButton (0))
			{
				onMouseHold ();
				onMouseBtnHold ();
			}
			if (Input.GetMouseButtonUp (0)) 
			{
				onMouseBtnUp ();
				onMouseClick ();
			}
			if (ZoomEnabled && Input.mouseScrollDelta.y != 0) 
			{
				onMouseScroll ();
			}
		}
	}
	#endif

	void onMouseHold () {
		Vector2 clickPosition = Input.mousePosition;

		// Длинный жест пальцем - перетаскивание
		if ((clickPosition - initialClick).magnitude > dragTreshold) {
			//ActiveCamera.TranslateScreen(lastClickPosition, clickPosition);
            isInteractionStatic = false;
		}
	}

	protected override void onMouseBtnDown()
	{
		RaycastHit2D hit = ActiveCamera.Raycast2DScreen(Input.mousePosition);
		if (hit && selectedObject == null)
		{
			selectedObject = hit.transform.gameObject;
			if (selectedObject.GetComponent<Base>().side == Faction.Player)
			{
				InputLine.instance.StartDraw(selectedObject.transform.position);
			}
			else
			{
				selectedObject = null;
			}
			//Начать отрисовку стрелочки
		}
	}

	//void onMouseClick()
	//{
	//	if (isInteractionStatic)
	//	{
	//		RaycastHit2D hit = ActiveCamera.Raycast2DScreen (Input.mousePosition);
	//		if (hit) {
	//		  // TODO: Не факт, что костыль производительнее OnMouseDown
	//		             hit.transform.gameObject.SendMessage ("Click", hit.point, SendMessageOptions.DontRequireReceiver);
	//		             RaiseClickTap(hit.point);
	//		}
	//	}
	//}

	void onMouseClick () {
		if (isInteractionStatic)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaiseClickTap(ray.origin + (ray.direction));
		}
		
		if (selectedObject != null)
		{
			
			RaycastHit2D hit = ActiveCamera.Raycast2DScreen(Input.mousePosition);
			if (hit)
			{
				Base _base = hit.transform.gameObject.GetComponent<Base>();
				if (_base != null && _base.gameObject != selectedObject)
				{
					selectedObject.GetComponent<Base>().StartSpawnUnit(hit.transform);
					//RaiseClickTapOnBase(selectedObject.GetComponent<Base>(), _base);
				}
			}
			//переставать рисовать стрелочку
			InputLine.instance.EndDraw();
			selectedObject = null;
		}
		
	}

	void onMouseScroll () {
		ActiveCamera.ZoomScreen (Input.mousePosition, ZoomSpeed*Input.mouseScrollDelta.y);
	}
	
	void OnDrawGizmos() {
		if (selectedObject != null) {
			Gizmos.color = Color.blue;
			Gizmos.DrawLine(selectedObject.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
		}
	}
}

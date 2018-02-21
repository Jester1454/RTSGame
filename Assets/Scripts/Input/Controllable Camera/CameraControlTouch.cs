using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ObjectBehavior;
using UnityEngine;

// TODO Задокументировать
// TODO Обработка двойного клика
public class CameraControlTouch : AbstractCameraControlTouch {

	// Минимальная дистанция смещения пальца в пикселях, чтобы считалось дивжение, а не тык
	// TODO: не стоит ли перейти к относительным координатам?
	private float dragTreshold = 10f;
	
	public GameObject selectedObject;

	protected override void Awake() {
		Attach ();
	}

	void Update()
	{
		if (attached) {
			// Обновление точек касания при первом касании
			if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
				onTouchStart ();
			}

			// Одно касание - перетаскивание камеры или поиск столкновений
			if (Input.touchCount == 1)
			{
//				if (Input.GetTouch(0).phase == TouchPhase.Ended)
//				{
//					Debug.Log("touch with 1 ended");
//					onTouchEnded();
//				}
				onOneTouch ();
			}

			// Два касания - зум
			if (ZoomEnabled && Input.touchCount == 2)
			{
				onTwinTouch ();
			} else {
				zoomStarted = false;
			}
		}
	}

	void onTouchStart () {
		updateTouchPositions ();
		initialTouch = lastTouchPosition [0];
		isInteractionStatic = true;
		
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

	void onTouchEnded()
	{
		Touch touch = Input.GetTouch (0);
		Vector2 touchPosition = touch.position;
		if (selectedObject != null)
		{
			
			RaycastHit2D hit = ActiveCamera.Raycast2DScreen(Input.mousePosition);
			if (hit)
			{
				Base _base = hit.transform.gameObject.GetComponent<Base>();
				if (_base != null && _base.gameObject != selectedObject)
				{
					selectedObject.GetComponent<Base>().StartSpawnUnit(hit.transform);
				}
			}
			//переставать рисовать стрелочку
			InputLine.instance.EndDraw();
			selectedObject = null;
		}
	}

	void onOneTouch () {
		Touch touch = Input.GetTouch (0);
		Vector2 touchPosition = touch.position;

		if (DragEnabled)
		{
			// Длинный жест пальцем - перетаскивание
			if (touch.phase == TouchPhase.Moved && (touchPosition - initialTouch).magnitude > dragTreshold)
			{
				ActiveCamera.TranslateScreen(lastTouchPosition[0], touchPosition);
				lastTouchPosition[0] = touchPosition;
				isInteractionStatic = false;
			}
		}

		// Короткое прикосновение - поиск места тыка
		if (touch.phase == TouchPhase.Ended && isInteractionStatic)
		{
			if (isInteractionStatic)
			{
				Ray ray = Camera.main.ScreenPointToRay(touchPosition);
				RaiseClickTap(ray.origin + (ray.direction));
				//RaycastHit2D hit = ActiveCamera.Raycast2DScreen (touchPosition);
				//if (hit) {
				//	// TODO: Не факт, что костыль производительнее OnMouseDown
				//             hit.transform.gameObject.SendMessage ("Click", hit.point, SendMessageOptions.DontRequireReceiver);
				//}
			}
			onTouchEnded();
		}
	}

	void onTwinTouch () {
		if (!zoomStarted) {
			zoomStarted = true;
			lastZoomCenter = ActiveCamera.Camera.ScreenToWorldPoint ((Input.GetTouch (0).position + Input.GetTouch (1).position) / 2f);
		}
		if (lastTouchPosition.Length > 1) {
			float deltaScale = (lastTouchPosition [0] - lastTouchPosition [1]).magnitude - (Input.GetTouch (0).position - Input.GetTouch (1).position).magnitude;
			ActiveCamera.Zoom (
				lastZoomCenter, 
				ZoomSpeed * deltaScale / ActiveCamera.Camera.ScreenToWorldPoint (Vector3.one).magnitude
			);
			isInteractionStatic = false;
		}
		updateTouchPositions ();
	}

	void updateTouchPositions () {
		lastTouchPosition = (new List<Touch> (Input.touches)).FindAll (touchInProgress).Select(t => t.position).ToArray ();
	}

	private static bool touchInProgress (Touch touch) {
		return touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled;
	}
}

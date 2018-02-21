using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// TODO Задокументировать
public class CameraControlTouchNGUI : AbstractCameraControlTouch {

	private LayerMask eventLayer;

	protected override void Awake () {
        base.Awake();
		lastTouchPosition = new Vector2[0];
		eventLayer = ActiveCamera.RaycastLayers;
	}

	public override void Attach () {
		base.Attach ();
		if (DragEnabled || ZoomEnabled) {
			UICamera.onDrag += Drag;
		}
		if (DoubleClickEnabled) {
			UICamera.onDoubleClick += DoubleClick;
		}
	}

	public override void Detach () {
		base.Detach ();
		UICamera.onDrag -= Drag;
		UICamera.onDoubleClick -= DoubleClick;
	}

	void Update () {
		// Сброс состояний жестов
		if (attached) {
			if ((zoomStarted || dragStarted) && Input.touchCount == 0) {
				lastTouchPosition = new Vector2[0];
			}
			if (zoomStarted && Input.touchCount < 2) {
				zoomStarted = false;
			}
			if (dragStarted && Input.touchCount < 1) {
				dragStarted = false;
			}	
		}
	}

	void Drag (GameObject go, Vector2 direction) {
		// Для любого количества касаний
		if (isSignalCatched(go)) {
			if (DragEnabled && Input.touchCount == 1) {
				onOneTouch ();
			}
			if (ZoomEnabled && Input.touchCount == 2) {
				onTwinTouch ();
			}
		}
	}

	void onOneTouch () {
		if (!dragStarted) {
			updateTouchPositions ();
			initialTouch = lastTouchPosition [0];
			isInteractionStatic = true;
			dragStarted = true;
		}

		Touch touch = Input.GetTouch (0);
		if (touch.phase == TouchPhase.Moved) {
			ActiveCamera.TranslateScreen(lastTouchPosition[0], touch.position);
			lastTouchPosition[0] = touch.position;
			isInteractionStatic = false;
		}
	}

	void onTwinTouch () {
		
		if (!zoomStarted) {
			zoomStarted = true;
			lastZoomCenter = ActiveCamera.Camera.ScreenToWorldPoint ((Input.GetTouch (0).position + Input.GetTouch (1).position) / 2f);
		}
		if (lastTouchPosition.Length > 1) {
			float lastDelta = (lastTouchPosition [0] - lastTouchPosition [1]).magnitude;
			float currentDelta = (Input.GetTouch (0).position - Input.GetTouch (1).position).magnitude;
			float deltaScale = (lastDelta - currentDelta)/lastDelta;
			ActiveCamera.Zoom (
				lastZoomCenter, 
				ZoomSpeed * deltaScale / ActiveCamera.Camera.ScreenToWorldPoint (Vector3.right).magnitude
			);
			isInteractionStatic = false;
		}
		updateTouchPositions ();
	}

	void DoubleClick (GameObject go) {
		if (DoubleClickEnabled && isSignalCatched (go)) {
			RaiseDoubleClickTap (go);
		}
	}

	void updateTouchPositions () {
		lastTouchPosition = (new List<Touch> (Input.touches)).FindAll (touchInProgress).Select(t => t.position).ToArray ();
	}

	private static bool touchInProgress (Touch touch) {
		return touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled;
	}

	bool isSignalCatched (GameObject go) {
		return eventLayer == (eventLayer | (1 << go.layer));
	}
}

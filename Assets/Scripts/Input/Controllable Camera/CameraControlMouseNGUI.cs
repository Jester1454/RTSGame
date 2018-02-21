using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO Задокументировать
public class CameraControlMouseNGUI : AbstractCameraControlMouse {

	private LayerMask eventLayer;

	protected override void Awake () {
        base.Awake();
		eventLayer = ActiveCamera.RaycastLayers;
	}

	// Подписка на события
	public override void Attach () {
		base.Attach();
		UICamera.onPress += Press;
		if (DragEnabled) {
			UICamera.onDrag += Drag;
		}
		if (ZoomEnabled) {
			UICamera.onScroll += Scroll;
		}
		if (DoubleClickEnabled) {
			UICamera.onDoubleClick += DoubleClick;
		}
	}

	public override void Detach () {
		base.Detach ();
		UICamera.onPress -= Press;
		UICamera.onDrag -= Drag;
		UICamera.onScroll -= Scroll;
		UICamera.onDoubleClick -= DoubleClick;
	}

	void OnScroll (float deltaScroll) {
		Scroll (this.gameObject, deltaScroll);
	}

	void Scroll (GameObject go, float deltaScroll) {
		if (ZoomEnabled && isSignalCatched(go)) {
			Vector2 center = Input.mousePosition;
			lastZoomCenter = (lastZoomCenter != center) ? center : lastZoomCenter;
			ActiveCamera.ZoomScreen (lastZoomCenter, ZoomSpeed*deltaScroll);
		}
	}

	void Press (GameObject go, bool state) {
		if (isSignalCatched(go)) {
			if (state) {
				onMouseBtnDown ();
			} else {
				onMouseBtnUp ();
			}
		}
	}

	void Drag (GameObject go, Vector2 direction) {
		// Для любого количества касаний
		if (DragEnabled && isSignalCatched(go)) {
			Vector2 clickPosition = Input.mousePosition;
			ActiveCamera.TranslateScreen(lastClickPosition, clickPosition);
			onMouseBtnHold ();
		}
	}

	void DoubleClick (GameObject go) {
		if (DoubleClickEnabled && isSignalCatched (go)) {
			RaiseDoubleClickTap (go);
		}
	}

	bool isSignalCatched (GameObject go) {
		return eventLayer == (eventLayer | (1 << go.layer));
	}
}

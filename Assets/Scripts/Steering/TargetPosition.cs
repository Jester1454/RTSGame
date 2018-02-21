using UnityEngine;

public class TargetPosition : MonoBehaviour {

    Transform t;

	void Start () 
    {
        t = GetComponent<Transform>();
	}
	
	void Update () 
    {
		if (Input.GetMouseButton(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			t.position = ray.origin + (ray.direction);
		}
	}
}

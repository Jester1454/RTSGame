using UnityEngine;

public class InputLine : MonoBehaviour
{
    public static InputLine instance;
    private LineRenderer line;
    private bool isDraw = false;

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

        line = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (isDraw)
        {
            Vector3 endPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            endPosition.z = 0;
            line.SetPosition(1, endPosition); ;
        }
    }

    public void StartDraw(Vector2 position)
    {
        line.SetPosition(0, new Vector3(position.x, position.y, 0));

        line.SetPosition(1, Camera.main.ScreenToWorldPoint(Input.mousePosition));
        isDraw = true;
    }

    public void EndDraw()
    {
        line.SetPosition(1, line.GetPosition(0));
        isDraw = false;
    }
}
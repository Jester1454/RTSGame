using UnityEngine;


public class CannonBall : MonoBehaviour
{
    public Vector3 Finish;
    public float speed = 2.0f;

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, Finish, speed * Time.deltaTime);
        if (transform.position == Finish)
        {
            this.Recycle();
        }
    }

}
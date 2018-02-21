using ObjectBehavior;
using UnityEngine;

public abstract  class NaturalDisaster : CollisionObject
{
    public float Maxforce;
    public abstract  Vector2 CalculateForce(Vector2 position);
}

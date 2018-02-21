using ObjectBehavior;
using UnityEngine;

public class AI
{
    public Base Base; // база
    public bool BaseIsDead = false;
    public Transform CurrentTarget; // текущая цель

    public AI(Base _Base)
    {
        this.Base = _Base;
        this.CurrentTarget = this.Base.ObjectTransform;
        //this.Targets = new List<Vector2>(countTargets);
    }
}

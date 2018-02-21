using System.Collections;
using System.Collections.Generic;
using ObjectBehavior;
using UnityEngine;

public class  Maelstorm: NaturalDisaster
{  
    public bool constLife;
    public float LifeDuration;
    float angle =  1.0f; //угол направления засывания в водоворот (в радианах)
    public float SpeedRotation;
    
    private void Awake()
    {
        ObjectTransform = transform;

        if (!constLife)
            StartCoroutine(LifeCycle());
    }

    public override Vector2 CalculateForce(Vector2 position)
    {
        Vector2 desire = ((Vector2)position - (Vector2)ObjectTransform.position );
        desire.Normalize();
        desire *= Maxforce;
        Vector2 force = new Vector2( desire.x * Mathf.Cos(angle) - desire.y * Mathf.Sin(angle), desire.x * Mathf.Sin(angle) + desire.y * Mathf.Cos(angle));
        return force;
    }
    
    public void OnDrawGizmos()
    {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, SoftRadius);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, HardRadius);
    }
    
    IEnumerator LifeCycle()
    {
        float duration = 0.0f;
        while (duration<LifeDuration)
        {
            duration += Time.deltaTime;
            transform.Rotate(Vector3.back *Time.deltaTime* SpeedRotation);
            yield return null;
        }
        ColliderManager.instance.naturalDisasters.Remove(this);
        List<Unit> units = ColliderManager.instance.ReturnUnitInRadius(ObjectTransform.position, SoftRadius);
        for(int i=0; i<units.Count; i++)
            units[i].NatureDisasterEnd();
        Destroy(gameObject);
    }
}

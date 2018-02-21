using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour {

	public float maxSpeed = 4.0f;
	public float maxForce = 5.0f;
    public float sight = 3.0f;

    public Transform target;
	public Vector2 velocity = Vector2.zero;
    public Vector2 acceleration = Vector2.zero;

    public List<Agent> agentList;
    public  Transform pos;

    void Start () 
    {
        pos = GetComponent<Transform>();
	}
	
	void Update () 
    {
        seekInGroup(agentList);
		velocity += acceleration;
		velocity = Limited(velocity, maxSpeed);
		transform.position += (Vector3)velocity;
		acceleration *= 0;
	}

    public Vector2 Limited(Vector2 v, float magnitude)
    {
        return v.normalized * magnitude;
    }

	Vector2 Seek(Vector2 targetPosition)
	{
        Vector2 desire = ((Vector2)targetPosition - (Vector2)pos.position);
		desire.Normalize();
		desire *= maxSpeed;
		Vector2 steering = (desire - velocity);
		steering = Limited(steering, maxForce);
		return steering;
	}

	public Vector2 Separate(List<Agent> agentList)
	{
        float desireSeparation = sight * 2;
		Vector2 sum = new Vector2();
		int count = 0;
        foreach (Agent col in agentList)
		{
            if (col != this)
            {
                float d = Vector2.Distance(pos.position, col.pos.position);
                if (d > 0 && d < desireSeparation)
                {
                    Vector2 diff = pos.position - col.pos.position;
                    diff.Normalize();
                    diff *= 1 / d;
                    sum += diff;
                    count++;
                }
            }
		}
		if (count > 0)
		{
			sum *= 1 / count;
			sum.Normalize();
			sum *= maxSpeed;
			Vector2 steer = sum - velocity;
			steer = Limited(steer, maxForce);
			return (steer);
		}
		else
		{
			return Vector2.zero;
		}
	}

	Vector2 align(List<Agent> units)
	{
        float neigbours = sight * 2;
		Vector2 sum = Vector2.zero;
		int count = 0;

		foreach (Agent other in units)
		{
            if (other != this)
            {
                float d = Vector2.Distance(pos.position, other.pos.position);
                if (d > 0 && d < neigbours)
                {
                    sum += other.velocity;
                    count++;
                }
            }
		}
		if (count > 0)
		{
			sum *= 1 / units.Count;
			sum.Normalize();
			sum *= maxSpeed;
			Vector2 steer = sum - velocity;
			steer = Limited(steer, maxForce);
			return steer;

		}
		else
			return Vector2.zero;
	}

	Vector2 cohesion(List<Agent> units)
	{
		float neigbours = sight * 2;
		Vector2 sum = Vector2.zero;
		int count = 0;
        foreach (Agent other in units)
		{
            if (other != this)
            {
                float d = Vector2.Distance(pos.position, other.pos.position);
                if ((d > 0) && (d < neigbours))
                {
                    sum += ((Vector2)other.pos.position);
                    count++;
                }
            }
		}
		if (count > 0)
		{
			sum *= 1 / (count);
			return Seek(sum);
		}
		else
		{
			return new Vector2(0, 0);
		}
	}

	public void flock(List<Agent> units)
	{
		Vector2 sep = Separate(units);
		Vector2 coh = cohesion(units);
		Vector2 al = align(units);
        Vector2 seek = Seek(target.position);

		sep *= 1.5f;
		al *= 0.5f;
		coh *= 0.5f;

		applyForce(sep);
		applyForce(al);
		applyForce(coh);
        applyForce(seek);
	}

    public void seekInGroup(List<Agent> units)
    {
		Vector2 sep = Separate(units);
        Vector2 seek = Seek(target.position);
		sep *= 1.5f;
        seek *= 0.5f;
        applyForce(sep);
        applyForce(seek);
	}

	void Arrive(Vector2 targetPosition)
	{
		Vector2 desired = ((Vector2)targetPosition - (Vector2)pos.position);
		float d = desired.magnitude;
		desired.Normalize();

		if (d < 10)
		{
			float m = Remap(d, 0, 10, 0, maxSpeed);
			desired *= m;
		}
		else
		{
			desired *= maxSpeed;
		}

		Vector2 steer = desired - velocity;
		steer = Limited(steer, maxForce);
		applyForce(steer);
	}

	void applyForce(Vector2 force)
	{
		acceleration += force;
	}

	public float Remap(float val, float from1, float to1, float from2, float to2)
	{
		return (val - from1) / (to1 - from1) * (to2 - from2) + from2;
	}

	public void OnDrawGizmos()
	{
        if (gameObject.activeSelf)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(pos.position, sight);
        }
	}
}

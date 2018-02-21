using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ObjectBehavior
{
    public class Steering : ScriptableObject
    {

        public Vector2 Seek(Vector2 targetPosition, Vector2 objectPostion, float maxSpeed, Vector2 velocity, float maxForce)
        {
            Vector2 desire = ((Vector2)targetPosition - (Vector2)objectPostion);
            desire.Normalize();
            desire *= maxSpeed;
            Vector2 steering = (desire - velocity);
            steering = Limited(steering, maxForce);
			if (Demp(steering))
				return steering;
			else
				return Vector2.zero;
        }

        public Vector2 Arrive(Vector2 targetPosition, Vector2 objectPostion, float maxSpeed, Vector2 velocity, float maxForce)
        {
            Vector2 desired = ((Vector2)targetPosition - (Vector2)objectPostion);
            float d = desired.magnitude;
            desired.Normalize();
            if (d < 100)
            {
                float m = Remap(d, 0, 100, 0, maxSpeed);
                desired *= m;
            }
            else
            {
                desired *= maxSpeed;
            }

            Vector2 steer = desired - velocity;
            steer = Limited(steer, maxForce);
            if (Demp(steer))
                return steer;
            else
                return Vector2.zero;
        }

        public Vector2 Separate(List<CollisionObject> collisions, RTSObject unit, float maxSpeed, Vector2 velocity, float maxForce, ref int  count)  
        {
            Vector2 sum = Vector2.zero;
            count = 0;
            bool storm = false;
            foreach (CollisionObject col in collisions)
            {
                if (col.GetType() != typeof(Maelstorm))
                {
                    float SoftSeparation = unit.SoftRadius + col.SoftRadius;
                    float HardSeparation = unit.HardRadius + col.HardRadius;

                    float d = Vector2.Distance(unit.ObjectTransform.position, col.ObjectTransform.position);

                    if (d > 0 && d <= SoftSeparation) 
                    {
                        float force = 0.25f;
                        if (d <= HardSeparation)
                        {
                            unit.OnDead();
                            return Vector2.zero;
                        }
                        Vector2 diff = unit.ObjectTransform.position - col.ObjectTransform.position;
                        diff.Normalize();
                        diff /= d;
                        diff *= force;
                        sum += diff;
                        count++; 
                    }
                }
                else
                {
                    storm = true;
                }
            }
            if (count > 0 || storm)
            {
                sum /= count;
                sum.Normalize();
                sum *= maxSpeed;
                Vector2 steer = sum - velocity;
                steer = Limited(steer, maxForce);
                return steer;
            }
            else
            {
                return Vector2.zero;
            }
        }

        private bool CompareSideUnits(RTSObject obj1, RTSObject obj2)
        {
            if (obj1.side == obj2.side)
                return true;
            return false;
        }

   //     public Vector2 seekInGroup(List<CollisionObject> units, Vector2 target, Vector2 objectPostion, float maxSpeed, Vector2 velocity, float maxForce, float radius)
   //     {
   //         Vector2 arrive = Seek(target, objectPostion, maxSpeed, velocity,  maxForce);
			//Vector2 separate = Separate(units, objectPostion, maxSpeed, velocity, maxForce, radius);

			//separate *= 2.0f;
        //    arrive *= 0.5f;
        //    Vector2 result = separate + arrive;
        //    return result;
        //}

        public Vector2 Limited(Vector2 v, float magnitude)
        {
            return v.normalized * magnitude;
        }

        float Remap(float val, float from1, float to1, float from2, float to2)
        {
            return (val - from1) / (to1 - from1) * (to2 - from2) + from2;
        }

        bool Demp(Vector2 steer)
        {
            if (steer.magnitude < 0.01f)
                return false;
            else
                return true;
        }

        Vector2 getNormalPoint(Vector2 p, Vector2 a, Vector2 b)
        {
            Vector2 ap = p - a;
            Vector2 ab = b - a;

            ab.Normalize();

            ab *= Vector2.Dot(ap, ab);

            Vector2 normalPoint = a + ab;
            return normalPoint;
        }

        Vector2 followPath(Vector2[] path, Vector2 objectPostion, float maxSpeed, Vector2 velocity, float maxForce, float radius)
        {
            Vector2 predict = velocity;
            predict.Normalize();
            predict *= 50.0f;
            Vector2 predictPos = (Vector2)objectPostion + predict;

            Vector2 normal = Vector2.zero;
            Vector2 targetPos = Vector2.zero;

            float worldRecord = float.PositiveInfinity;

            for (int i = 0; i < path.Length - 1; i++)
            {
                Vector2 a = path[i];
                Vector2 b = path[i + 1];

                Vector2 normalPoint = getNormalPoint(predictPos, a, b);

                if (normalPoint.x < a.x || normalPoint.x > b.x)
                {
                    normalPoint = b;
                }

                float distance = Vector2.Distance(predictPos, normalPoint);
                if (distance < worldRecord)
                {
                    worldRecord = distance;
                    normal = normalPoint;

                    Vector2 dir = b - a;
                    dir.Normalize();
                    dir *= 10.0f;
                    targetPos = normalPoint;
                    targetPos += dir;
                }
            }

            if (worldRecord > radius)
            {
                return Seek(targetPos, objectPostion, maxSpeed, velocity, maxForce);
            }
            else
                return Vector2.zero;

        }
        
        public  Vector2 AttackBaseMovement(Vector2 position, float Maxforce, CollisionObject Base)
        {
            Vector2 desire = ((Vector2)position - (Vector2)Base.ObjectTransform.position);
            desire.Normalize();
            desire *= Maxforce;
            float angle = 1.57f;
            Vector2 force = new Vector2( (desire.x * Mathf.Cos(angle) - desire.y * Mathf.Sin(angle)), (desire.x * Mathf.Sin(angle) + desire.y * Mathf.Cos(angle)));
            return force;
        }

		    //Vector2 align(List<CollisionObject> units, Vector2 objectPostion, float maxSpeed, Vector2 velocity, float maxForce, float radius)
		    //{
		    //    float neigbours = radius * 2;
		    //    Vector2 sum = Vector2.zero;
		    //    int count = 0;
		    //    foreach (Unit other in units)
		    //    {
		    //        float d = Vector2.Distance(objectPostion, other.ObjectTransform.position);
		    //        if (d > 0 && d < neigbours)
		    //        {
		    //            sum += other.velocity;
		    //            count++;
		    //        }
		    //    }
		    //    if (count > 0)
		    //    {
		    //        sum *= 1 / units.Count;
		    //        sum.Normalize();
		    //        sum *= maxSpeed;
		    //        Vector2 steer = sum - velocity;
		    //        steer = Limited(steer, maxForce);
		    //        if (Demp(steer))
		    //            return steer;
		    //        else
		    //            return Vector2.zero;
		    //    }
		    //    else
		    //        return Vector2.zero;
		    //}

		public Vector2 cohesion(List<CollisionObject> collisions, Vector2 objectPostion, float maxSpeed, Vector2 velocity, float maxForce, float radius)
        {
            Vector2 sum = Vector2.zero;
            int count = 0;
            foreach (CollisionObject other in collisions)
            {
                float neigbours = radius + other.SoftRadius;

				float d = Vector2.Distance(objectPostion, other.ObjectTransform.position);
                if ((d > 0) && (d < neigbours))
                {
                    sum += ((Vector2)other.ObjectTransform.position);
                    count++;
                }
            }
            if (count > 0)
            {
                sum *= 1 / (count);
                return Seek(sum, objectPostion, maxSpeed, velocity, maxForce);
            }
            else
            {
                return new Vector2(0, 0);
            }
        }

        //public void flock(List<Unit> units, Vector2 target)
        //{
            //Vector2 sep = Separate(units);
            //Vector2 coh = cohesion(units);
            //Vector2 al = align(units);
            //Vector2 seek = Seek(target);

        //        sep *= 2.5f;
        //        al *= 1.5f;
        //        coh *= 1.5f;
        //        seek *= 0.5f;

        //        applyForce(sep);
        //        applyForce(al);
        //        applyForce(coh);
        //        applyForce(seek);
        //    }
    }
}

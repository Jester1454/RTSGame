using System;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectBehavior
{
    public enum MovementState
    {
        AttackEnemyBase,
        MoveInBase,
	    MoveForNatureDisaster,
        Stop
    }
	
	[Serializable]
	public class Movement : MonoBehaviour
	{
        private Unit rtsObject;
		
		private MovementState state;

		public MovementState State
        {
            get
            {
                return state;
            }
            set
            {
                state = value;
            }
        }

        public float maxSpeed;
		public float maxForce;
		public float rotateSpeed;

		private Vector2 velocity = Vector2.zero;
		private Vector2 acceleration = Vector2.zero;
		
		[NonSerialized]
        public Transform target;
        private Transform MoveTarget;

      //  [NonSerialized]
        public List<CollisionObject> Group;	
        private int countCollision=0;

        private Steering steer;

		void Start()
		{
            state = MovementState.MoveInBase;

			Group = new List<CollisionObject>();
			
            rtsObject = GetComponent<Unit>();
		
    	   if (rtsObject.side != Faction.Player) //затычка для ИИ
    	   {
		       MoveTarget = AIManager.instance.GetAITarget(rtsObject.side);
        	   AIManager.instance.changeTarget += RefreshPath; 
		       target = MoveTarget;
		       RefreshPath();
    	   }
			steer = ScriptableObject.CreateInstance<Steering>();
		}

        private void Update()
        {
	        if (target != null)
	        {
		        float DistanceToTarget = (target.position - rtsObject.ObjectTransform.position).magnitude;
		        if (DistanceToTarget < rtsObject.SoftRadius ||
		            (DistanceToTarget < rtsObject.SoftRadius * 3.0f && countCollision > 0) ||
		            (DistanceToTarget < rtsObject.SoftRadius * 6.0f && countCollision > 1))
		        {
			        if (state != MovementState.MoveForNatureDisaster && state != MovementState.Stop)
				        state = MovementState.Stop;
		        }

		        if (state != MovementState.Stop)
		        {
			        if (state != MovementState.MoveForNatureDisaster && !rtsObject.attack.IsAttack)
			        {
				        applyForce(0.5f * steer.Arrive(target.position, rtsObject.ObjectTransform.position,
					                   maxSpeed, velocity, maxForce));
			        }

			        applyForce(1.5f * steer.Separate(Group, rtsObject,
				                   maxSpeed, velocity, maxForce, ref countCollision));

			        if (rtsObject.attack.IsAttack && rtsObject.attack.Enemy.GetType() == typeof(Base))
			        {
				        applyForce(steer.AttackBaseMovement(rtsObject.ObjectTransform.position, maxForce, rtsObject.attack.Enemy));

			        }
			        Move();
			        Rotate();
		        }
	        }
        }

        void applyForce(Vector2 force)
        {
			acceleration += force;
        }

		public void applyNatureForce(Vector2 force)
		{
			if(state != MovementState.MoveForNatureDisaster)
				state = MovementState.MoveForNatureDisaster;
			acceleration += force;
		}
		
		void Move()
		{
			if (state != MovementState.Stop)
			{
				velocity += acceleration;
				velocity = steer.Limited(velocity, maxSpeed);
				transform.position += (Vector3) velocity * Time.deltaTime;
				acceleration *= 0;
			}
		}

		void Rotate()
		{
			if (velocity.sqrMagnitude > 0.5f)
			{
                float toRotation = (Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg) - 90.0f;
				float rotation = Mathf.LerpAngle(transform.rotation.eulerAngles.z, toRotation, Time.deltaTime * rotateSpeed);
				transform.rotation = Quaternion.Euler(0, 0, rotation);
			}
		}
		
		public void RefreshPath()
		{
			if (state != MovementState.AttackEnemyBase)
			{
				state = MovementState.MoveInBase;
				target = MoveTarget;
			}
		}
		
		public void RefreshPath(Transform Base)
		{
			state = MovementState.MoveInBase;
			MoveTarget = Base;
			target = MoveTarget;
		}

		public void StopObject()
		{
			velocity = Vector2.zero;
			acceleration = Vector2.zero;
			state = MovementState.Stop;
		}

	}
}

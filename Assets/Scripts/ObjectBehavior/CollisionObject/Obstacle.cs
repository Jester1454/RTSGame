using UnityEngine;

namespace ObjectBehavior
{

    public class Obstacle : CollisionObject
    {
        public bool DrawGizmos;
        void Start()
        {
            ObjectTransform = transform;
        }

        private void OnDrawGizmos()
        {
            if(DrawGizmos)
            {
				Gizmos.color = Color.magenta;
				Gizmos.DrawWireSphere(transform.position, SoftRadius);
				Gizmos.color = Color.green;
				Gizmos.DrawWireSphere(transform.position, HardRadius);
            }
        }
    }	
}
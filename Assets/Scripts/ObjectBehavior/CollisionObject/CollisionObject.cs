using System;
using UnityEngine;

namespace ObjectBehavior
{
    [Serializable]
    public abstract class CollisionObject : MonoBehaviour
    {
        [NonSerialized]
        public Transform ObjectTransform;

        public float HardRadius;
        public float SoftRadius;

    }
}

using System;

namespace ObjectBehavior
{
	[Serializable]
    public abstract class AttackObject : RTSObject
    {
	    [NonSerialized]
		public Attack attack;
	    	    
	    [NonSerialized]
	    public AttackObjectState state;

	    public override abstract void OnDead();

        public abstract void Attack(RTSObject enemy);

	    public abstract void EnemyOut();

    }
}

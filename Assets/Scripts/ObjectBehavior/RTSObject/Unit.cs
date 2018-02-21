using System;
using PrototypeScripts;
using UnityEngine;

namespace ObjectBehavior
{
	public enum AttackObjectState
	{
		isDead,
		isAlive,
	}
	
	[Serializable]
	public class Unit : AttackObject 
    {
	    
        [NonSerialized]
        public Movement move;
	    
	    public bool DrawGizmos;
	    
	    private BoomAnimation boom;

		void Awake()
        {
	        state = AttackObjectState.isAlive;
			attack = GetComponent<Attack>();
			ObjectTransform = transform;
            currentHP = maxHP;
            move = GetComponent<Movement>();
            boom = GetComponent<BoomAnimation>();
        }

	    private void Start()
	    {
		    CreateFromPrototype();
	    }
	    
	    private void CreateFromPrototype()
	    {
		    UnitPrototype proto = SaveDataManager.instance.GetUnitPrototype(side);
		    if (proto != null)
		    {
			    maxHP = proto.maxHP;
			    currentHP = maxHP;
			    attack.damage = proto.Damage;
			    attack.damageRate = proto.DamageRate;
			    attack.attackRadius = proto.AttackRadius;
			    move.maxSpeed = proto.maxSpeed;
		    }
	    }

	    public override void Attack(RTSObject enemy)
        {
            attack.AttackEnemy(enemy);
        }

	    public override void EnemyOut()
	    {
		    attack.Enemy = null;
	    }

		public override void OnDead()
		{
			if (state != AttackObjectState.isDead)
			{
				move.StopObject();
				state = AttackObjectState.isDead;
				if(AudioManager.instance!=null)
					AudioManager.instance.ExplosionPlay();
				if(gameObject.activeSelf)
					boom.Boom();
			}
		}

	    public void isNatureDisaster(NaturalDisaster naturalDis)
	    {
		    move.applyNatureForce(naturalDis.CalculateForce(ObjectTransform.position));
	    }

	    public void NatureDisasterEnd()
	    {
		    move.RefreshPath();
	    }
	    
		public void OnDrawGizmos()
		{
			if (DrawGizmos)
			{
				Gizmos.color = Color.magenta;
				Gizmos.DrawWireSphere(transform.position, SoftRadius);
				Gizmos.color = Color.green;
				Gizmos.DrawWireSphere(transform.position, HardRadius);

                if (attack != null)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawWireSphere(transform.position, attack.attackRadius);
                }
			}
		}

	    public void MoveInBase(Transform Base)
	    {
		    move.RefreshPath(Base);
	    }
    }
}
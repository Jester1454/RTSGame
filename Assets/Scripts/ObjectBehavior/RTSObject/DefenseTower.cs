using System;
using PrototypeScripts;
using UnityEngine;

namespace ObjectBehavior
{
	
	[Serializable]
    public class DefenseTower : AttackObject
    {
		
	    public bool DrawGizmos = true;
        private RTSObject Enemy;
        public float rotateSpeed;

        private void Awake()
        {
	        state = AttackObjectState.isAlive;
            attack = GetComponent<Attack>();
			ObjectTransform = transform;
            currentHP = maxHP;
		}

	    private void Start()
	    {
		    CreateFromPrototype();
	    }

	    private void CreateFromPrototype()
	    {
		    TowerPrototype proto = SaveDataManager.instance.GetTowerPrototype(side);
		    if (proto != null)
		    {
			    maxHP = proto.maxHP;
			    currentHP = maxHP;
			    attack.damage = proto.Damage;
			    attack.damageRate = proto.DamageRate;
			    attack.attackRadius = proto.AttackRadius;
			    rotateSpeed = proto.RotateSpeed;
		    }
	    }
	    
	    void Update()
        {
            if(Enemy!=null)
            {
                Rotate();
            }
        }

        public override void Attack(RTSObject enemy)
		{
            if (enemy != null && enemy.currentHP > 0 && !attack.IsAttack)
            {
				Enemy = enemy;
				attack.AttackEnemy(enemy);
            }
		}

	    public override void EnemyOut()
	    {
		    attack.Enemy = null;
	    }
	    
        private void Rotate()
        {
            Vector2 lookDir = Enemy.ObjectTransform.position - ObjectTransform.position;
            lookDir.Normalize();
            float toRotation = (Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg) - 90.0f;
			float rotation = Mathf.LerpAngle(ObjectTransform.rotation.eulerAngles.z, toRotation, Time.deltaTime * rotateSpeed);
	        ObjectTransform.rotation = Quaternion.Euler(0, 0, rotation);
		}

		public override void OnDead()
		{
			state = AttackObjectState.isDead;
            if(AudioManager.instance!=null)
			    AudioManager.instance.ExplosionPlay();
            gameObject.SetActive(false);
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
    }
}

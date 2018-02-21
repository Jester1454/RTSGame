using System;
using System.Collections;
using UnityEngine;

namespace ObjectBehavior
{
	public enum Faction
	{
		Player,
		Enemy1,
		Enemy2,
		Neutral
	}

	[Serializable]
    public abstract class RTSObject : CollisionObject
    {

		public float maxHP;
	    
		public Faction side;

        [NonSerialized]
        public float currentHP;
	    
		public delegate void EnemyAttack(float hp);
		public event EnemyAttack enemyAttack = delegate { };
	    private bool invul = false;

		public virtual void Attacked(float damage)
		{
			if (!invul)
			{
				currentHP -= damage;
				enemyAttack(currentHP);
				if (currentHP <= 0)
				{
					OnDead();
				}
			}
		}

	    public virtual void Heal(float healPoints)
	    {
			if (currentHP > 0)
			{
				if (currentHP + healPoints > maxHP)
				{
					currentHP = maxHP;
				}
				else
				{
					currentHP += healPoints;
				}
			}
	    }
	    
        public abstract void OnDead();

	    public void SetInvulnerability(float duration)
	    {
		    StartCoroutine(Invulnerability(duration));
	    }
	    
	    IEnumerator Invulnerability(float duration)
	    {
		    invul = true;
		    yield return new WaitForSeconds(duration);
		    invul = false;
	    }
    }
}

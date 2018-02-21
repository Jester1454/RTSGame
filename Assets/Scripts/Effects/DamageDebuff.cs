using System.Collections;
using System.Collections.Generic;
using ObjectBehavior;
using UnityEngine;

namespace Effects
{
	public class DamageDebuff : Effect
	{
		private Unit targetUnit;
		public float DebuffDamage;
		private float originalDamage;
		
		public override void StartEffect(float duration)
		{
			targetUnit = GetComponent<Unit>();
			base.StartEffect(duration);
		}

		protected override void ApplyEffect()
		{
			originalDamage = targetUnit.attack.damage;
	
			if (targetUnit.attack.damage - DebuffDamage < 0)
			{
				targetUnit.attack.damage = 0;
			}
			else
			{
				targetUnit.attack.damage -= DebuffDamage;
	
			}
		}

		protected override void ResetEffect()
		{
			targetUnit.attack.damage = originalDamage;
		}
	}
}
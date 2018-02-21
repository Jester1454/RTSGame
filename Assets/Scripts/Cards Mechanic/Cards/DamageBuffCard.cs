using System.Collections.Generic;
using Effects;
using ObjectBehavior;
using UnityEngine;

namespace Cards
{
	public class DamageBuffCard : TargetZoneCard
	{
		public float BuffDamage;
		public float DurationEffect;

		public override bool ApplyEffect(Vector2 effectPosition)
		{
			List<Unit> units = ColliderManager.instance.ReturnUnitInRadius(effectPosition, Radius);
			
			for (int i = 0; i < units.Count; i++)
			{
				if (units[i].side == Faction.Player)
				{
					DamageBuff buff = units[i].gameObject.AddComponent<DamageBuff>();
					buff.BuffDamage = BuffDamage;
					buff.StartEffect(DurationEffect);
				}
			}
			
			base.ApplyEffect(effectPosition);
			return true;
		}
	}
}
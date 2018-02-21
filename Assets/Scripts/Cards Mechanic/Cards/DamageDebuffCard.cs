using System.Collections.Generic;
using Effects;
using ObjectBehavior;
using UnityEngine;

namespace Cards
{
	public class DamageDebuffCard : TargetZoneCard
	{
		public float DebuffDamage;
		public float DurationEffect;

		public override bool ApplyEffect(Vector2 effectPosition)
		{
			List<Unit> units = ColliderManager.instance.ReturnUnitInRadius(effectPosition, Radius);
			
			for (int i = 0; i < units.Count; i++)
			{
				if (units[i].side != Faction.Player)
				{
					DamageDebuff debuff = units[i].gameObject.AddComponent<DamageDebuff>();
					debuff.DebuffDamage = DebuffDamage;
					debuff.StartEffect(DurationEffect);
				}
			}
			
			base.ApplyEffect(effectPosition);
			return true;
		}
	}
}
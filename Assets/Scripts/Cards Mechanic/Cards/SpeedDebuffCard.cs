using System.Collections.Generic;
using Effects;
using ObjectBehavior;
using UnityEngine;

namespace Cards
{
	public class SpeedDebuffCard : TargetZoneCard
	{
		public float DebuffSpeed;
		public float DurationEffect;

		public override bool  ApplyEffect(Vector2 effectPosition)
		{
			List<Unit> units = ColliderManager.instance.ReturnUnitInRadius(effectPosition, Radius);
			
			for (int i = 0; i < units.Count; i++)
			{
				if (units[i].side != Faction.Player)
				{
					SpeedDebuff debuff = units[i].gameObject.AddComponent<SpeedDebuff>();
					debuff.DebuffSpeed = DebuffSpeed;
					debuff.StartEffect(DurationEffect);
				}
			}
			
			return  base.ApplyEffect(effectPosition);
		}
	}
}
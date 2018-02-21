using System.Collections.Generic;
using Effects;
using ObjectBehavior;
using UnityEngine;

namespace Cards
{
	public class SpeedBuffCard : TargetZoneCard
	{
		public float BuffSpeed;
		public float DurationEffect;

		public override bool ApplyEffect(Vector2 effectPosition)
		{
			List<Unit> units = ColliderManager.instance.ReturnUnitInRadius(effectPosition, Radius);
			
			for (int i = 0; i < units.Count; i++)
			{
				if (units[i].side == Faction.Player)
				{
					SpeedBuff buff = units[i].gameObject.AddComponent<SpeedBuff>();
					buff.BuffSpeed = BuffSpeed;
					buff.StartEffect(DurationEffect);
				}
			}
			
			return base.ApplyEffect(effectPosition);
		}
	}
}
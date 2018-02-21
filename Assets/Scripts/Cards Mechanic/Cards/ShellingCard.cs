using UnityEngine;

namespace  Cards
{
	public class ShellingCard : TargetZoneCard
	{
		public float Damage;

		public override bool ApplyEffect(Vector2 effectPosition)
		{
			ColliderManager.instance.Shelling(effectPosition, Radius, Damage);
			return base.ApplyEffect(effectPosition);
		}
	}
}


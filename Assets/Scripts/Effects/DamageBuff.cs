using ObjectBehavior;

namespace Effects
{
	public class DamageBuff : Effect
	{
		private Unit targetUnit;
		public float BuffDamage;
		private float originalDamage;
		
		public override void StartEffect(float duration)
		{
			targetUnit = GetComponent<Unit>();
			base.StartEffect(duration);
		}

		protected override void ApplyEffect()
		{
			originalDamage = targetUnit.attack.damage;

			targetUnit.attack.damage += BuffDamage;
		}

		protected override void ResetEffect()
		{
			targetUnit.attack.damage = originalDamage;
		}
	}
}
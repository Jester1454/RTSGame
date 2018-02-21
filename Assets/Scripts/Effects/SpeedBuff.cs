using ObjectBehavior;

namespace Effects
{
	public class SpeedBuff : Effect
	{
		private Unit targetUnit;
		public float BuffSpeed;
		private float originalDamage;
		
		public override void StartEffect(float duration)
		{
			targetUnit = GetComponent<Unit>();
			base.StartEffect(duration);
		}

		protected override void ApplyEffect()
		{
			originalDamage = targetUnit.move.maxSpeed;

			targetUnit.move.maxSpeed += BuffSpeed;
		}

		protected override void ResetEffect()
		{
			targetUnit.move.maxSpeed = originalDamage;
		}
	}
}
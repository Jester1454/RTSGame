using ObjectBehavior;

namespace Effects
{
	public class SpeedDebuff : Effect
	{
		private Unit targetUnit;
		public float DebuffSpeed;
		private float originalSpeed;
		
		public override void StartEffect(float duration)
		{
			targetUnit = GetComponent<Unit>();
			base.StartEffect(duration);
		}

		protected override void ApplyEffect()
		{
			originalSpeed = targetUnit.move.maxSpeed;
			if (targetUnit.move.State != MovementState.MoveForNatureDisaster)
			{
				if (targetUnit.move.maxSpeed - DebuffSpeed < 0)
				{
					//лучше так не делать, а то иначе корабли будут просто стоять, даже тогда, 
					//когда на них налетит внезапная буря
					targetUnit.move.maxSpeed = 0;
				}
				else
				{
					targetUnit.move.maxSpeed -= DebuffSpeed;

				}
			}
		}

		protected override void ResetEffect()
		{
			targetUnit.move.maxSpeed = originalSpeed;
		}
	}
}
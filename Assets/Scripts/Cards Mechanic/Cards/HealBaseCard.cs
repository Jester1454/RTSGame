using ObjectBehavior;
using UnityEngine;

namespace Cards
{
	public class HealBaseCard : Card
	{		
		public override bool ApplyEffect(Vector2 effectPosition)
		{
			Base _base= ColliderManager.instance.ReturnBaseInRadius(effectPosition, 1.0f);//круг радиуса один
			if(_base != null)
			{
				if (_base.side == Faction.Player)
				{
					_base.Heal(_base.maxHP);
					return true;
				}
				else
				{
					return false;
				}
			}
			return true;
		}

	}
}
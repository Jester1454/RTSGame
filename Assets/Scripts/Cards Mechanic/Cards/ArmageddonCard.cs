using System;
using UnityEngine;

namespace Cards
{
	public class ArmageddonCard : Card {
		public override bool ApplyEffect(Vector2 effectPosition)
		{
			ColliderManager.instance.DestroyAllUnits();
			base.ApplyEffect(effectPosition);
			return true;
		}
	}
}

	
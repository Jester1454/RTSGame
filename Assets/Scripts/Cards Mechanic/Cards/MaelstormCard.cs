using UnityEngine;

namespace Cards
{
	public class MaelstormCard : Card
	{
		public NaturalDisaster Maelstorm;

		public override bool ApplyEffect(Vector2 effectPosition)
		{
			ColliderManager.instance.AddnatureDisaster(Instantiate(Maelstorm.gameObject, effectPosition, Quaternion.identity).GetComponent<Maelstorm>());
			Maelstorm.transform.position = effectPosition;
			return base.ApplyEffect(effectPosition);
		}
	}
}
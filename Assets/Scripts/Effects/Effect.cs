using System.Collections;
using UnityEngine;

namespace Effects
{
	public abstract class Effect : MonoBehaviour
	{

		public float DurationEffect;

		protected abstract void ApplyEffect();

		protected abstract void ResetEffect();

		public virtual void StartEffect(float duration)
		{
			DurationEffect = duration;
			StartCoroutine(StartEffect());
		}

		IEnumerator StartEffect()
		{
			ApplyEffect();
			yield return new WaitForSeconds(DurationEffect);
			ResetEffect();
			DestroyObject(this);
		}
	}
}
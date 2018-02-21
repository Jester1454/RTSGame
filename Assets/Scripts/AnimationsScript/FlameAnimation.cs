using System.Collections;
using System.Collections.Generic;
using ObjectBehavior;
using UnityEngine;

public class FlameAnimation : MonoBehaviour {

	public Sprite[] Animation;
	private SpriteRenderer mesh;
	private Unit ownableRtsObject;
	public float ChangeSpriteRate=0.1f;
	
	void Start () 
	{
		mesh = GetComponent<SpriteRenderer>();
		ownableRtsObject = GetComponentInParent<Unit>();
		StartCoroutine(AnimationClip());
	}


	IEnumerator AnimationClip()
	{
		int i = 0;
		while (ownableRtsObject.state == AttackObjectState.isAlive)
		{
			mesh.sprite = Animation[i];
			yield return new WaitForSeconds(ChangeSpriteRate);
			i++;
			if (i >= Animation.Length)
				i = 0;
		}
		mesh.sprite = null;
	}
	
}

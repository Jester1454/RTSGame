using System.Collections;
using System.Collections.Generic;
using ObjectBehavior;
using UnityEngine;

public class BaseAnimator : MonoBehaviour
{
	public Sprite PlayerBase;
	public Sprite EnemyBase;
	public Sprite NeutralBase;
	
	public Color PlayerColor;
	public Color EnemyColor;
	public Color NeutralColor;

	private UISprite HPSprite;

	public UISprite HpSprite
	{
		get { return HPSprite; }
		set { HPSprite = value; }
	}

	private UILabel countText;

	public UILabel CountText
	{
		get { return countText; }
		set { countText = value; }
	}

	private SpriteRenderer CurrentSprite;
	private Base Base;

	private void Awake()
	{
		CurrentSprite = GetComponent<SpriteRenderer>();
		Base = GetComponent<Base>();
	}

	public void CaptureAnimation()
	{
		//Debug.Log("CaptureBase");
		if (Base.side == Faction.Enemy1)
		{
			CurrentSprite.sprite = EnemyBase;
			HPSprite.color = EnemyColor;
		}
		if (Base.side == Faction.Player)
		{
			CurrentSprite.sprite = PlayerBase;
			HPSprite.color = PlayerColor;
		}
		if (Base.side == Faction.Neutral)
		{
			CurrentSprite.sprite = NeutralBase;
			HPSprite.color = NeutralColor;
		}
	}
	
	public void UpdateCountText()
	{
		//хз что за баг
		int a = Base.CurrentCountUnit;
			CountText.text = a.ToString();
	}
	
	public void LevelUpAnimation()
	{
		//Debug.Log("LevelUpBase");
	}

	public void DestroyAnimation()
	{
		//Debug.Log("DestroyBase");
	}
}

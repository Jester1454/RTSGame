using System.Collections;
using System.Collections.Generic;
using  ObjectBehavior;
using UnityEngine;

public class CircularHPBar : MonoBehaviour {

	
	private UISprite HPBar;

	public UISprite HpBar
	{
		get { return HPBar; }
		set { HPBar = value; }
	}

	private RTSObject rtsObject;
	
	void Start () 
	{
		rtsObject = GetComponent<RTSObject>();
		rtsObject.enemyAttack += OnHPCahnged;
	}

	public void OnHPCahnged(float HP)
	{
		if(rtsObject!=null)
			HPBar.fillAmount = rtsObject.currentHP/rtsObject.maxHP;
	}
}

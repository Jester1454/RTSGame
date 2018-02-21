using UnityEngine;
using System.Collections;
using ObjectBehavior;

public class DoubleClick : MonoBehaviour
{
	public float delayBetween2Clicks ; // Change value in editor
	private float lastClickTime = 0 ;
	public Base Base;
	
	public void OnClickCallBack()
	{
		if( Time.time - lastClickTime  < delayBetween2Clicks )
		{
			if (Base.side == Faction.Player)
			{
				Base.LevelUpBase();
			}
			Debug.Log("Double Click");
		}
		else
		{    
			StartCoroutine( OnClickCoroutine() ) ;    
		}
		lastClickTime = Time.time ;
	}
 
	IEnumerator OnClickCoroutine()
	{    
		yield return new WaitForSeconds( delayBetween2Clicks ) ;
     
		if( Time.time - lastClickTime  < delayBetween2Clicks )
		{
			yield break ;
		}
		Debug.Log( "Simple click" );
	}
 
}
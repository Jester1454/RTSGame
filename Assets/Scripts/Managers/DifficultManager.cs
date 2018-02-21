using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultManager : MonoBehaviour 
{
    public GameObject[] Prefabs;
    public static DifficultManager instance;

    private void Awake()
    {
		if (instance == null)
		{

			instance = this;
		}
		else
			if (instance != this)
		{
			Destroy(gameObject);
		}
    }

//    public void SetEasy()
//	{
//		Unit Player = Prefabs[0].GetComponent<Unit>();
//		//Unit Enemy1 = Prefabs[1].GetComponent<Unit>();
//		//Unit Enemy2 = Prefabs[2].GetComponent<Unit>();

//		Player.maxSpeed = 6.0f;
//		Player.damage = 10.0f;
//		Player.HP = 15.0f;
//    }

//    public void SetMedium()
//    {
//		Unit Player = Prefabs[0].GetComponent<Unit>();
////		Unit Enemy1 = Prefabs[1].GetComponent<Unit>();
//		//Unit Enemy2 = Prefabs[2].GetComponent<Unit>();

//        Player.maxSpeed = 4.0f;
//		Player.damage = 5.0f;
//		Player.HP = 10.0f;
//    }

//    public void SetHard()
//    {
//		Unit Player = Prefabs[0].GetComponent<Unit>();
////		Unit Enemy1 = Prefabs[1].GetComponent<Unit>();
////		Unit Enemy2 = Prefabs[2].GetComponent<Unit>();

		//Player.maxSpeed = 3.0f;
		//Player.damage = 3.0f;
		//Player.HP = 10.0f;
    //}
}

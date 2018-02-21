using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour {

    public static ObjectPoolManager instance;
    public GameObject[] Prefabs;
    public int SizePool = 100; 
    private ObjectPool Pool;

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
        for (int i = 0; i < Prefabs.Length; i++)
        {
            Prefabs[i].CreatePool(SizePool);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using ObjectBehavior;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
	public static TargetManager instance;

	public Base[] Bases;
	public Transform PlayerTarget;

	public float UpdateTargetRate;

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != this)
		{
			Destroy(gameObject);
		}
	}

}

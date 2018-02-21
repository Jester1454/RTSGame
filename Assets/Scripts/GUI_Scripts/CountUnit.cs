using System.Collections;
using System.Collections.Generic;
using ObjectBehavior;
using UnityEngine;

public class CountUnit : MonoBehaviour
{

	public UILabel Count;
	private Base Base;

	private void Awake()
	{
		Base = GetComponent<Base>();
		UpdateCount();
	}

	public void UpdateCount()
	{
		Count.text = Base.ToString();
	}
}

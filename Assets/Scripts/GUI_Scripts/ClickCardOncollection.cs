using System.Collections;
using System.Collections.Generic;
using Cards;
using UnityEngine;

public class ClickCardOncollection : MonoBehaviour
{

	private Card card;
	
	void Start ()
	{
		card = GetComponent<Card>();
	}

	void OnClick()
	{
		if(CollectionManager.instance!=null)
			CollectionManager.instance.ClickOnCard(card);
	}
}

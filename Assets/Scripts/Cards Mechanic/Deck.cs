using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Cards
{
	[Serializable]
	public class Deck: MonoBehaviour	
	{
		public int Id;
		public int DeckSize;
		public List<Card> Cards = new List<Card>();

		public void AddCard(Card card)
		{
			if (Cards.Count < DeckSize)
			{
				card.PutInDeck();
				Cards.Add(card);
			}
		}

		public void RemoveCard(Card card)
		{
			card.RemoveFromDeck();
			for (int i = 0; i < Cards.Count; i++)
			{
				if (card.Id == Cards[i].Id)
				{
					Cards[i].RemoveFromDeck();
					Cards.RemoveAt(i);
				}
			}
		}
		
		public void ShuffleDeck()
		{
			for (int i = 0; i < Cards.Count; i++)
			{
				int j = Random.Range(0, i);
				Card t = Cards[j];
				Cards[j] = Cards[i];
				Cards[i] = t;
			}
		}

		public Card PutInHand()
		{
			if (Cards.Count > 0)
			{
				Card topCard= Cards[0];
				topCard.PutInHand();
				Cards.RemoveAt(0);
				return topCard;
			}
			else
			{
				return null;
			}
		}

		public List<Card> StartHand(int HandSize)
		{
			List<Card> hand = Cards.GetRange(0, HandSize);
			
			foreach (var card in hand)
			{
				card.PutInHand();
			}
			
			Cards.RemoveRange(0,HandSize);
			return hand;	
		}
		public void Clear()
		{
			Cards.Clear();
			DeckSize = 0;
		}
	}	
}

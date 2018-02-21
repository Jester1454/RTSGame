using System.Collections.Generic;
using Cards;
using PrototypeScripts;
using UnityEngine;

public class CardManager : MonoBehaviour
{
	public Deck Deck;
	public GameObject NextCardHolder;
	public int HandSize;
	
	public CircularTimer timer;
	public float TimeNextCard;
	
	private List<Card> Hand;
	static public CardManager instance;
	private List<Card> Graveyard;

	public GameObject[] CardHolders;
	public List<Card> AllCards;
	
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
		timer.maxTime = TimeNextCard;
	}


	private void CreateDeckFromConfig()
	{
		DeckConfig deckConfig = SaveDeckManager.instance.GetConfig();
		if (deckConfig != null)
		{
			Deck.Clear();
			Deck.DeckSize = deckConfig.DeckSize;
			for (int i = 0; i < deckConfig.DeckSize; i++)
			{
				Deck.AddCard(FindCardInCollection(deckConfig.Cards[i].Id));
			}
		}
	}

	
	void Start () 
	{
		//CreateDeckFromConfig();
		Deck = GetComponent<Deck>();
		Deck.ShuffleDeck();
		Hand = Deck.StartHand(HandSize);
		DrawHand();
		DrawNextCard();
		Graveyard = new List<Card>();
		timer.stop += NextCard;
	}

	private void DrawHand()
	{
		for(int i=0; i<Hand.Count;i++)
		{
			NGUITools.AddChild(CardHolders[i], Hand[i].gameObject);
		}	
	}

	private void DrawNextCard()
	{
		if (Deck.Cards.Count > 0)
		{
			Deck.Cards[0].PutNext();
			NGUITools.AddChild(NextCardHolder, Deck.Cards[0].gameObject);
		}
	}
	
	private  void RedrawHand()
	{
		if (Hand.Count >=0)
		{
			for(int i= 0; i<CardHolders.Length;i++)
				NGUITools.DestroyChildren(CardHolders[i].transform);
			DrawHand();
		}
	}
	
	private void RedrawNextCard()
	{
		if (Deck.Cards.Count >=0)
		{	
			NGUITools.DestroyChildren(NextCardHolder.transform);
			DrawNextCard();
		}
	}
	
	public void RemoveCardFromHand(Card card)
	{
		timer.StartTurnTimer();
		
		RemoveCard(card);
		RedrawHand();
	}

	private void NextCard()
	{
		Hand.Add(Deck.PutInHand());	
		RedrawNextCard();
		DrawLastCard();
		//RedrawHand();
		
		if (Deck.Cards.Count == 1)
		{
			Deck.Cards.AddRange(Graveyard);
			Graveyard.Clear();
		}
		if(Hand.Count<HandSize)
			timer.StartTurnTimer();
	}
	
	private void RemoveCard(Card card)
	{
		
		for(int i=0;i<Hand.Count;i++)
		{
			if (Hand[i].Id == card.Id)
			{
				Card newCard = Hand[i];
				Graveyard.Add(newCard);
				Hand.RemoveAt(i);
				break;
			}
		}
	}

	private Card FindCardInCollection(int id)
	{
		for (int i = 0; i < AllCards.Count; i++)
		{
			if (id == AllCards[i].Id)
			{
				return AllCards[i];
			}
		}
		return null;
	}
	
	
	private void DrawLastCard()
	{
		NGUITools.AddChild(CardHolders[Hand.Count-1], Hand[Hand.Count-1].gameObject);
	}
}

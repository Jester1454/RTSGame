using System.Collections.Generic;
using Cards;
using PrototypeScripts;
using UnityEngine;

public class CollectionManager : MonoBehaviour
{
	public UIGrid UICollection;	
	public UIGrid UIDeck;
	public UIScrollView DeckScrollView;
	private Deck Deck;
	public List<Card> Collection;
	public GameObject ErrorMenu;
	
	public static CollectionManager instance;

	void Awake()
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
		Deck = GetComponent<Deck>();
		Collection = new List<Card>();
		Collection.AddRange(UICollection.GetComponentsInChildren<Card>());
		CreateDeckFromConfig();
	}

	public void ClickOnCard(Card card)
	{		
		if(card.state == CardState.InCollection)
		{
			if (Deck.Cards.Count < Deck.DeckSize)
			{
				if (AudioManager.instance != null)
					AudioManager.instance.CardClickPlay();
				Deck.AddCard(card);
				GameObject temp = NGUITools.AddChild(UIDeck.gameObject, card.gameObject);
				temp.GetComponent<Card>().PutInDeck();
				temp.GetComponent<UIDragScrollView>().scrollView = DeckScrollView;
				UIDeck.Reposition();
				RemoveCardFromCollection(card);
			}
		}
		else
		{
			if (AudioManager.instance != null)
				AudioManager.instance.CardClickPlay();
			Deck.RemoveCard(card);
			AddCardCollection(card);
			Destroy(card.gameObject);
			UIDeck.Reposition();
		}
	}

	private void DrawDeck()
	{
		for (int i = 0; i < Deck.DeckSize; i++)
		{
			GameObject temp = NGUITools.AddChild(UIDeck.gameObject, Deck.Cards[i].gameObject);
			temp.GetComponent<Card>().PutInDeck();
			temp.GetComponent<UIDragScrollView>().scrollView = DeckScrollView;
			UIDeck.Reposition();
			RemoveCardFromCollection(Deck.Cards[i]);
		}
	}
	
	private void RemoveCardFromCollection(Card card)
	{
		for (int i = 0; i < Collection.Count; i++)
		{
			if (card.Id == Collection[i].Id)
			{
				Collection[i].PutInDeck();
			}
		}
		card.GetComponent<UIButton>().isEnabled = false;
	}

	private void AddCardCollection(Card card)
	{
		for (int i = 0; i < Collection.Count; i++)
		{
			if (card.Id == Collection[i].Id)
			{
				Collection[i].RemoveFromDeck();
				Collection[i].GetComponent<UIButton>().isEnabled = true;
			}
		}
	}

	public void CollectionClose()
	{
		if (Deck.Cards.Count < Deck.DeckSize)
		{
			UIManager.instance.CollectionsMenu.SetActive(false);
			ErrorMenu.SetActive(true);
		}
		else
		{
			if (Deck.Cards.Count == Deck.DeckSize)
			{
				SaveDeck();
				UIManager.instance.CollectionsMenu.SetActive(false);
				UIManager.instance.MainMenuButtons.SetActive(true);
			}
		}
	}

	public void CloseError()
	{
		ErrorMenu.SetActive(false);
		UIManager.instance.CollectionsMenu.SetActive(true);
	}

	public void SaveDeck()
	{
		SaveDeckManager.instance.SaveDeck(Deck);
	}
	
	private Card FindCardInCollection(int id)
	{
		for (int i = 0; i < Collection.Count; i++)
		{
			if (id == Collection[i].Id)
			{
				return Collection[i];
			}
		}
		return null;
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
			DrawDeck();
		}
	}
}

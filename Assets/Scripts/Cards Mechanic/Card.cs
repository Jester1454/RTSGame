using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Cards
{
	public enum CardState
	{
		InCollection,
		InHand,
		InDeck,
		InField,
		IsDead
	}
	
	public enum CardType
	{
		TargetCard,
		NoneTargetCard,
		TargetZoneCard
	}
	
	public class Card : MonoBehaviour, IHaveZoneApplication
	{
		public ZoneApplication ZoneApplication 
		{
			get { return zone; }	
			set { zone = value; }
		}
		
		public ZoneApplication zone;
		public int Id;
		public CardType type;
		
		private CardState State = CardState.InCollection;

		
		public CardState state
		{
			get { return State; }
			set { State = value; }
		}

		public void PutInDeck()
		{
			State = CardState.InDeck;
		}

		public void RemoveFromDeck()
		{
			State = CardState.InCollection;
		}
		
		public virtual bool ApplyEffect(Vector2 effectPosition)
		{
			State = CardState.IsDead;
			return true;
		}

		public void PutInHand()
		{
			State = CardState.InHand;
			
			DragDropCard dragDropCard = GetComponent<DragDropCard>();
			if (dragDropCard != null) dragDropCard.enabled = true;
		}

		public void PutNext()
		{
			State = CardState.InDeck;
			DragDropCard dragDropCard = GetComponent<DragDropCard>();
			if (dragDropCard != null) dragDropCard.enabled = false;
		}

		public virtual void CardInField()
		{
			State = CardState.InField;
		}

	}
}

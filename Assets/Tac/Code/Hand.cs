// TAC
// Strategy Game Architecture
// Copyright (c) 2020 Ted Brown

using Jambox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tac
{
	/// <summary>
	/// Manages cards that an entity can play. Different than the Deck.
	/// </summary>
	public class Hand 
	{
		private List<Sequence> _cards;

		public Hand ()
		{
			_cards = new List<Sequence>();
		}

		public void AddCard (Sequence card, int index = -1)
		{
			if (index >= 0 && index < _cards.Count)
			{
				_cards.Insert(index, card);
			}
			else
			{
				_cards.Add(card);
			}
		}

		public bool AddCard (string cardId, int index = -1)
		{
			if (CardManager.IsValidCardId(cardId))
			{
				AddCard(CardManager.CreateCard(cardId), index);
				return true;
			}
			UnityEngine.Debug.LogError(string.Format("Hand.AddCard: Card id '{0}' is not recognized by Card Manager. Ignoring", cardId));
			return false;
		}

		public bool Draw (Deck targetDeck)
		{
			if (targetDeck.RemainingCards > 0)
			{
				AddCard(targetDeck.Draw());
				return true;
			}
			return false;
		}

		public Sequence[] GetCards ()
		{
			return _cards.ToArray();
		}

		public void RemoveCard (Sequence card)
		{
			_cards.Remove(card);
		} 
	}
}

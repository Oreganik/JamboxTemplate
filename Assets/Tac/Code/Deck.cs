// TAC
// Strategy Game Architecture
// Copyright (c) 2020 Ted Brown

using Jambox;
using System;
using System.Collections.Generic;
using System.IO;

namespace Tac
{
	/// <summary>
	/// A collection of card instances.
	/// </summary>
    public class Deck
    {
		private const int COLUMN_COUNT = 2;

		public bool ErrorOnLoad
		{
			get { return _errorOnLoad; }
		}

		public int RemainingCards
		{
			get { return _cards.Count; }
		}

		private bool _errorOnLoad;
		private List<Sequence> _cards;

		public Deck ()
		{
			_cards = new List<Sequence>();
		}

		public Deck(List<string> cardIds)
		{
			_cards = new List<Sequence>();
			AddCards(cardIds);
		}

		public void AddCards (List<string> cardIds)
		{
			foreach (string cardId in cardIds)
			{
				AddCardToBottom(CardManager.CreateCard(cardId));
			}
		}

		public Sequence[] GetCards ()
		{
			return _cards.ToArray();
		}

		/// <summary>
		/// Takes a text file, runs it through DataReader, and builds a deck.
		/// </summary>
		public bool LoadFromFile (string dataPath)
		{
			// Read data from the file into a list of string arrays. Return if an error is encountered.
			List<string[]> data;
			_errorOnLoad = DataReader.GetDataAsStringArray(dataPath, COLUMN_COUNT, out data);

			if (_errorOnLoad)
			{
				UnityEngine.Debug.LogError(string.Format("{0} encountered an error processing data file '{1}'", this.GetType().ToString(), dataPath));
				return false;
			}

			return LoadFromStringData(data);
		}

		/// <summary>
		/// Takes a Unity text asset, runs it through DataReader, and builds a deck.
		/// </summary>
		public bool LoadFromTextAsset (UnityEngine.TextAsset textAsset)
		{
			// Read data from the file into a list of string arrays. Return if an error is encountered.
			List<string[]> data;
			_errorOnLoad = DataReader.GetDataAsStringArray(textAsset, COLUMN_COUNT, out data);

			if (_errorOnLoad)
			{
				UnityEngine.Debug.LogError(string.Format("{0} encountered an error processing text asset '{1}'", this.GetType().ToString(), textAsset.name));
				return false;
			}

			return LoadFromStringData(data);
		}

		/// <summary>
		/// Takes a list of string arrays and builds a deck.
		/// </summary>
		public bool LoadFromStringData (List<string[]> data)
		{
			int count;
			string cardId = string.Empty;
			string rawCount = string.Empty;
			List<string> cardIds = new List<string>();

			// Process every data entry.
			foreach (string[] entry in data)
			{
				bool errorForThisEntry = false;

				// Get the data for this entry
				// Schema: Card Id [0], Count [1]
				cardId = entry[0];
				rawCount = entry[1];

				// Does this card actually exist?
				if (CardManager.IsValidCardId(cardId) == false)
				{
					UnityEngine.Debug.LogError(string.Format("Deck: Card id '{0}' is not recognized by Card Manager. Ignoring", cardId));
					errorForThisEntry = true;
				}

				if (int.TryParse(rawCount, out count) == false)
				{
					UnityEngine.Debug.LogError(string.Format("Deck: Card count '{0}' for card id '{1}' could not be parsed as an int.", rawCount, cardId));
					errorForThisEntry = true;
				}

				if (errorForThisEntry == false)
				{
					// Add a new entry for each card ID * count
					for (int i = 0; i < count; i++)
					{
						cardIds.Add(cardId);
					}
				}

				if (_errorOnLoad == false)
				{
					_errorOnLoad = errorForThisEntry;
				}
			}

			AddCards(cardIds);

			return (_errorOnLoad == false);
		}

		/// <summary>
		/// Adds a card to the bottom of the deck.
		/// </summary>
		public void AddCardToBottom (Sequence card)
		{
			_cards.Add(card);
		}

		/// <summary>
		/// Adds a card to the top of the deck.
		/// </summary>
		public void AddCardToTop (Sequence card)
		{
			_cards.Insert(0, card);
		}

		/// <summary>
		/// Draws the "top" card of the deck and returns a card instance.
		/// If no cards are available, returns a null.
		/// </summary>
		public Sequence Draw ()
		{
			int count = _cards.Count;

			if (count == 0)
			{
				return null;
			}

			int id = count - 1;
			Sequence card = _cards[id];
			_cards.RemoveAt(id);

			return card;
		}

		/// <summary>
		/// Randomly re-orders the cards in the deck.
		/// </summary>
		public void Shuffle ()
		{
			// Variant of the Fisher-Yates-Shuffle.
			// Also known as Sattolo's algorithm.
			int count = _cards.Count;
			Sequence temp;
			var rand = new Random();

			for (int i = count - 1; i > 0; i--) 
			{
				int j = (int) Math.Floor(rand.NextDouble() * i);
				temp = _cards[i];
				_cards[i] = _cards[j];
				_cards[j] = temp;
		    }
		}
	}
}

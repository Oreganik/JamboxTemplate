// TAC
// Strategy Game Architecture
// Copyright (c) 2020 Ted Brown

using Jambox;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Tac
{
	/// <summary>
	/// Processes a data file containing card definitions.
	/// Constructs a unique instance of every card used in the game,
	/// and makes them available by id to other modules.
	/// </summary>
    public class CardManager
    {
		const char EFFECT_ARRAY_SEPARATOR = ',';
		const char EFFECT_VALUE_SEPARATOR = ':';
		const int COLUMN_COUNT = 3;
	
		public static bool ErrorOnLoad
		{
			get { return s_errorOnLoad; }
		}

		private static bool s_errorOnLoad;
		private static Dictionary<string, Card> s_cards;

		// Takes a text file, runs it through DataReader, and builds a card dictionary.
		public static bool Initialize (string dataPath)
		{
			// Read data from the file into a list of string arrays. Return if an error is encountered.
			List<string[]> data;
			s_errorOnLoad = DataReader.GetDataAsStringArray(dataPath, COLUMN_COUNT, out data);
			if (s_errorOnLoad)
			{
				UnityEngine.Debug.LogError(string.Format("CardManager: Encountered an error processing data file '{0}'", dataPath));
				return false;
			}

			return Initialize(data);
		}

		// Takes a Unity text asset, runs it through DataReader, and builds a card dictionary.
		public static bool Initialize (UnityEngine.TextAsset textAsset)
		{
			// Read data from the file into a list of string arrays. Return if an error is encountered.
			List<string[]> data;
			s_errorOnLoad = DataReader.GetDataAsStringArray(textAsset, COLUMN_COUNT, out data);
			if (s_errorOnLoad)
			{
				UnityEngine.Debug.LogError(string.Format("CardManager: Encountered an error processing text asset '{0}'", textAsset.name));
				return false;
			}

			return Initialize(data);
		}

		// Takes list of string arrays (usually from DataReader) and builds a card dictionary.
		public static bool Initialize (List<string[]> data)
		{
		 	s_cards = new Dictionary<string, Card>();

			int cost = -1;
			string id = string.Empty;
			string rawCost = string.Empty;
			string rawEffects = string.Empty;
			string[] effectsArray = null;
			string[] effectValues = null;

			// Process every data entry.
			foreach (string[] entry in data)
			{
				// Get the data for this card
				// Schema: Id [0], Effects [1], Cost [2]
				id = entry[0];
				rawEffects = entry[1];
				rawCost = entry[2];

				// NOTE: For the rest of this phase, if an error is thrown, the program will keep going.
				// This helps raise as many data errors as possible.

				// Determine the cost as an int
				if (int.TryParse(rawCost, out cost) == false)
				{
					UnityEngine.Debug.LogError(string.Format("CardManager: {0}: Could not parse cost '{1}' as int", id, rawCost));
					s_errorOnLoad = true;
				}

				// Determine the effect(s) for this card
				List<Effect> effects = new List<Effect>();
				effectsArray = rawEffects.Split(EFFECT_ARRAY_SEPARATOR);

				foreach (string effectString in effectsArray)
				{
					// Effects are defined like this: effectType:modifier
					effectValues = effectString.Split(EFFECT_VALUE_SEPARATOR);

					EffectType effectType;

					// Schema: EffectType [0], Value [1]
					// Value is not always an int, so don't cast it.
					try
					{
						effectType = (EffectType) Enum.Parse(typeof(EffectType), effectValues[0]);
					}
					catch
					{
						UnityEngine.Debug.LogError(string.Format("CardManager: {0}: Could not parse '{1}' as EffectType", id, effectValues[0]));
						s_errorOnLoad = true;
						continue;
					}

					// Instantiate an effect class and add it to our list
					bool errorCreatingEffect = false;
					Effect newEffect = EffectManager.InstantiateEffect(effectType, effectValues[1], out errorCreatingEffect);

					if (errorCreatingEffect)
					{
						UnityEngine.Debug.LogError(string.Format("CardManager: {0}: Encountered error creating effect '{1}'", id, effectString));
						s_errorOnLoad = true;
						continue;
					}

					effects.Add(newEffect);
				}

				// A card without any effects is useless: skip it.
				if (effects.Count == 0)
				{
					UnityEngine.Debug.LogError(string.Format("CardManager: {0}: Has no valid effects. Skipping", id));
					s_errorOnLoad = true;
					continue;
				}

				// Make sure this id doesn't already exist in our database
				if (s_cards.ContainsKey(id))
				{
					UnityEngine.Debug.LogError(string.Format("CardManager: A card with id '{0}' has already been added to the database. Skipping", id));
					s_errorOnLoad = true;
					continue;
				}

				// Finally! Let's add the card to the database.
				s_cards.Add(id, new Card(id, cost, effects.ToArray()));
			}

			return s_errorOnLoad;
		}

		/// <summary>
		/// Returns a card instance. Will return null if id can't be found.
		/// </summary>
		/// <param name="id">The id of the card as defined in the card database</param>
		public static Card GetCard (string id)
		{
			Card card = null;
			if (s_cards.TryGetValue(id, out card) == false)
			{
				UnityEngine.Debug.LogError(string.Format("CardManager.GetCard could not find a card of id '{0}'", id));
			}
			return card;
		}

		/// <summary>
		/// Verifies if a card id is found in our database.
		/// </summary>
		/// <param name="id">The id of the card as defined in the card database</param>
		public static bool IsValidCardId (string id)
		{
			return s_cards.ContainsKey(id);
		}

		/// <summary>
		/// Outputs all cards in the database to the console
		/// </summary>
		public static void PrintCards ()
		{
			foreach (KeyValuePair<string, Card> entry in s_cards)
			{
				Console.Write(entry.Value.ToString());
			}
		}
    }
}

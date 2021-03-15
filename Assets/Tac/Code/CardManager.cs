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
		const int COLUMN_COUNT = 4;
		const char EFFECT_ARRAY_SEPARATOR = ',';
	
		public static bool ErrorOnLoad
		{
			get { return s_errorOnLoad; }
		}

		private static bool s_errorOnLoad;
		private static Dictionary<string, Sequence> s_cards;

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
		 	s_cards = new Dictionary<string, Sequence>();

			int cost = -1;
			string id = string.Empty;
			string rawFaction = string.Empty;
			string rawCost = string.Empty;
			string rawEffects = string.Empty;
			string[] effectsArray = null;
			FactionType factionTarget;

			// Process every data entry.
			foreach (string[] entry in data)
			{
				bool errorForThisEntry = false;

				// Get the data for this card
				// Schema: Id [0], Faction [1], Effects [2], Cost [3]
				id = entry[0];
				rawFaction = entry[1];
				rawEffects = entry[2];
				rawCost = entry[3];

				// NOTE: For the rest of this phase, if an error is thrown, the program will keep going.
				// This helps raise as many data errors as possible.

				// Make sure this id doesn't already exist in our database
				if (s_cards.ContainsKey(id))
				{
					UnityEngine.Debug.LogError(string.Format("CardManager: A card with id '{0}' has already been added to the database", id));
					errorForThisEntry = true;
				}

				// Determine the faction type
				if (Enum.TryParse(rawFaction, ignoreCase: true, out factionTarget) == false)
				{
					UnityEngine.Debug.LogError(string.Format("CardManager: {0}: Could not parse '{1}' as FactionType", id, rawFaction));
					errorForThisEntry = true;
				}

				// Determine the cost as an int
				if (int.TryParse(rawCost, out cost) == false)
				{
					UnityEngine.Debug.LogError(string.Format("CardManager: {0}: Could not parse cost '{1}' as int", id, rawCost));
					errorForThisEntry = true;
				}

				// Determine the effect(s) for this card
				List<Effect> effects = new List<Effect>();
				effectsArray = rawEffects.Split(EFFECT_ARRAY_SEPARATOR);

				foreach (string rawEffect in effectsArray)
				{
					Effect effect = new Effect(id, rawEffect);

					if (effect.ErrorOnLoad)
					{
					 	UnityEngine.Debug.LogError(string.Format("CardManager: {0}: Encountered error creating effect '{1}'", id, rawEffect));
						errorForThisEntry = true;
					}
					else
					{
						effects.Add(effect);
					}
				}

				// A card without any effects is useless
				if (effects.Count == 0)
				{
					UnityEngine.Debug.LogError(string.Format("CardManager: {0}: Has no valid effects. Skipping.", id));
					errorForThisEntry = true;
				}

				if (errorForThisEntry)
				{
					s_errorOnLoad = true;
				}
				else
				{
					// Finally! Let's add the card to the database.
					s_cards.Add(id, new Sequence(id, cost, factionTarget, effects.ToArray()));
				}
			}

			return s_errorOnLoad;
		}

		/// <summary>
		/// Returns a card instance. Will return null if id can't be found.
		/// </summary>
		/// <param name="id">The id of the card as defined in the card database</param>
		public static Sequence CreateCard (string id)
		{
			Sequence card = null;
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
			foreach (KeyValuePair<string, Sequence> entry in s_cards)
			{
				Console.Write(entry.Value.ToString());
			}
		}
    }
}

﻿// TAC
// Strategy Game Architecture
// Copyright (c) 2020 Ted Brown

using Jambox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tac
{
	/// <summary>
	/// Processes a database containing base entity definitions.
	/// Spawns entity classes.
	/// </summary>
	public class EntityManager : MonoBehaviour 
	{
		const int COLUMN_COUNT = 3;
		const char VALUE_SEPARATOR = ',';

		public static bool ErrorOnLoad
		{
			get { return s_errorOnLoad; }
		}

		private static bool s_errorOnLoad;
		private static Dictionary<string, EntityBase> s_baseEntities;

		// Takes a text file, runs it through DataReader, and builds a card dictionary.
		public static bool Initialize (string dataPath)
		{
			// Read data from the file into a list of string arrays. Return if an error is encountered.
			List<string[]> data;
			s_errorOnLoad = DataReader.GetDataAsStringArray(dataPath, COLUMN_COUNT, out data);
			if (s_errorOnLoad)
			{
				UnityEngine.Debug.LogError(string.Format("EntityManager: Encountered an error processing data file '{0}'", dataPath));
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
				UnityEngine.Debug.LogError(string.Format("EntityManager: Encountered an error processing text asset '{0}'", textAsset.name));
				return false;
			}

			return Initialize(data);
		}

		// Takes list of string arrays (usually from DataReader) and builds a dictionary of BaseEntities
		public static bool Initialize (List<string[]> data)
		{
			int attributeCount = System.Enum.GetValues(typeof(AttributeKey)).Length;

			s_baseEntities = new Dictionary<string, EntityBase>();

			// These variables are used when processing data entries
			int[] baseAttributes = new int[attributeCount];
			string[] rawAttributes = new string[attributeCount];
			string id;
			List<string> cardIds = new List<string>(4);

			// Process every data entry.
			foreach (string[] entry in data)
			{
				// If an error is thrown, the program will keep going.
				// This helps raise as many data errors as possible.
				bool errorForThisEntry = false;

				// Get the data for this card
				// Schema: Id [0], Attributes [1], Cards [2]
				id = entry[0];
				rawAttributes = entry[1].Split(VALUE_SEPARATOR);
				cardIds.Clear();
				cardIds.AddRange(entry[2].Split(VALUE_SEPARATOR));

				// See if this id exists in our database
				if (s_baseEntities.ContainsKey(id))
				{
					UnityEngine.Debug.LogError(string.Format("EntityManager: '{0}': This id already exists in the database.", id));
					errorForThisEntry = true;
				}

				// Parse attributes as an integers
				for (int i = 0; i < attributeCount; i++)
				{
					AttributeKey key = (AttributeKey) i;
					int value = 0;

					if (int.TryParse(rawAttributes[i], out value))
					{
						baseAttributes[i] = value;
					}
					else
					{
						UnityEngine.Debug.LogError(string.Format("EntityManager: '{0}': Could not parse attribute '{1}' value '{2}' as int.", id, key, rawAttributes[i]));
						errorForThisEntry = true;
					}
				}

				// Parse card ids
				foreach (string cardId in cardIds)
				{
					if (CardManager.IsValidCardId(cardId) == false)
					{
						UnityEngine.Debug.LogError(string.Format("EntityManager: {0}: Card '{1}' is not recognized by CardManager.", id, cardId));
						errorForThisEntry = true;
					}
				}

				// Finally! Let's add the entry to the database.
				if (errorForThisEntry == false)
				{
					s_baseEntities.Add(id, new EntityBase(id, baseAttributes, cardIds));
				}

				if (!s_errorOnLoad)
				{
					s_errorOnLoad = errorForThisEntry;
				}
			}

			return s_errorOnLoad;
		}

		public static Entity Spawn (string id, Entity creator)
		{
			// See if this id exists in our database
			if (s_baseEntities.ContainsKey(id) == false)
			{
				UnityEngine.Debug.LogError(string.Format("EntityManager: Could not spawn id '{0}'. Does not exist in the database.", id));
				return null;
			}
			return s_baseEntities[id].Spawn(creator);
		}
	}
}

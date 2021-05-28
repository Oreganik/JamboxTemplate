// TAC
// Strategy Game Architecture
// Copyright (c) 2020 Ted Brown

using Chess;
using Jambox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tac
{
	/// <summary>
	/// </summary>
	public class Entity 
	{
		public bool ErrorOnLoad
		{
			get; private set;
		}

		/// <summary>The Entity that currently controls this instance.</summary>
		private Entity _controller;

		/// <summary>The Entity that owns this instance.</summary>
		private Entity _owner;

		/// <summary>Current values for all attributes: base + effects. All entities have all attributes, which keeps memory cleaner.</summary>
		private int[] _attributeValues;

		private List<Ability> _abilities;

		/* String definition is a series of COLUMNS that are tab-delimited
		 * Each column has FEATURES that are pipe delimited e.g. x|y
		 * Each feature has VALUES that are comma delimited, e.g. move,1,2
		 * EntityName
		 * AttributeKey,Value
		 * AbilityKey,Parameters
			"Rook	MoveDistance,8	" +
			"SlideMove,1,0|SlideMove,-1,0|SlideMove,0,1|SlideMove,0,-1|" +
			"SlideCapture,1,0|SlideCapture,-1,0|SlideCapture,0,1|SlideCapture,0,-1\n" +
		*/

		public Entity (Entity owner)
		{
			// Initialize attribute array and ability list
			_attributeValues = new int[System.Enum.GetValues(typeof(AttributeKey)).Length];
			_abilities = new List<Ability>();

			// Set owner/controller
			_owner = owner;
			_controller = owner;
		}

		public Entity (string definition, Entity owner)
		{
			// Initialize attribute array and ability list
			_attributeValues = new int[System.Enum.GetValues(typeof(AttributeKey)).Length];
			_abilities = new List<Ability>();

			// Set owner/controller
			_owner = owner;
			_controller = owner;

			// Remove spaces
			definition = definition.Replace(" ", String.Empty);

			// Split into columns
			string[] columns = definition.Split(ChessData.ColumnDelimiter, System.StringSplitOptions.None);
			
			Debug.Log(definition);
			Debug.Log("Columns: " + columns.Length);

			// Evaluate the EntityType by parsing string
			ChessData.EntityType entityType = ChessData.EntityType.Invalid;
			if (System.Enum.TryParse(columns[0], out entityType) == false)
			{
				Debug.LogErrorFormat("Could not parse {0} as ChessData.EntityType", columns[0]);
				ErrorOnLoad = true;
			}

			// Set EntityType attribute
			Set(AttributeKey.EntityType, (int) entityType);

			// Evaluate and set default attribute values
			string[] attributeValuePairs = columns[1].Split(ChessData.FeatureDelimiter, System.StringSplitOptions.RemoveEmptyEntries);
			string[] attributeEntry;
			foreach (string attribute in attributeValuePairs)
			{
				// Split into type and value
				attributeEntry = attribute.Split(ChessData.ValueDelimiter, System.StringSplitOptions.RemoveEmptyEntries);

				// Parse attribute enum
				AttributeKey attributeKey = AttributeKey.Invalid;
				if (System.Enum.TryParse(attributeEntry[0], out attributeKey) == false)
				{
					Debug.LogErrorFormat("Could not parse {0} as AttributeKey", attributeEntry[0]);
					ErrorOnLoad = true;
					continue;
				}

				// Parse value enum
				int attributeValue = 0;
				if (int.TryParse(attributeEntry[1], out attributeValue) == false)
				{
					Debug.LogErrorFormat("Could not parse {0} as int", attributeEntry[1]);
					ErrorOnLoad = true;
					continue;
				}
				
				// Set default value
				Set(attributeKey, attributeValue);
			}

			// Evaluate and initialize abilities. 
			string[] abilityFeatures = columns[2].Split(ChessData.FeatureDelimiter, StringSplitOptions.RemoveEmptyEntries);
			string[] entry;
			foreach (string abilityData in abilityFeatures)
			{
				// Split into type and values. Split once: the rest of the data is sent to the ability constructor.
				entry = abilityData.Split(ChessData.ValueDelimiter, 2);
				//Debug.LogFormat("Split [ {0} ] with {1} into {")

				// Parse ability enum
				Chess.ChessData.AbilityType key = Chess.ChessData.AbilityType.Invalid;
				if (System.Enum.TryParse(entry[0], out key) == false)
				{
					Debug.LogErrorFormat("Could not parse {0} as Chess.ChessData.AbilityType", entry[0]);
					ErrorOnLoad = true;
					continue;
				}

				// Get the ability instance
				Ability ability = Main.AbilityManager.GetAbilityInstance(key, entry[1]);

				// Make sure it's not null due to an error
				if (ability == null)
				{
					Debug.LogErrorFormat("Could not initialize ability {0} with parameters [ {1} ]", entry[0], entry[1]);
					ErrorOnLoad = true;
					continue;
				}

				_abilities.Add(ability);
			}

			Debug.LogFormat("{0} has {1} abilities", entityType, _abilities.Count);
		}

		public int Get (AttributeKey key)
		{
			return _attributeValues[(int)key];
		}

		public bool Matches (params AttrQuery[] queries)
		{
			foreach (AttrQuery query in queries)
			{
				if (_attributeValues[query.AttributeId] != query.Value)
				{
					return false;
				}
			}
			return true;
		}

		public void Set (AttributeKey key, int value)
		{
			_attributeValues[(int)key] = value;
		}
	}
}

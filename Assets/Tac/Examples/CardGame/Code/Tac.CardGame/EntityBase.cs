// TAC
// Strategy Game Architecture
// Copyright (c) 2020 Ted Brown

using Jambox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tac.CardGame
{
	/// <summary>
	/// Stores entity information and creates instances.
	/// </summary>
	public class EntityBase
	{
		/// <summary>Base values of its attributes</summary>
		private int[] _baseValues;

		/// <summary>Unique identifier that matches database entry</summary>
		private string _id;
		private string[] _cards;

		public EntityBase (string id, int[] values, List<string> cardIds)
		{
			_id = id;
			_baseValues = new int[values.Length];
			Array.Copy(values, _baseValues, values.Length);
			_cards = cardIds.ToArray();
		}

		public void SetBaseValues (int[] values)
		{
			if (values == null)
			{
				values = new int[_baseValues.Length];
				Array.Copy(_baseValues, values, values.Length);
				return;
			}

			if (values.Length != _baseValues.Length)
			{
				UnityEngine.Debug.LogError(string.Format("BaseEntity '{0}' can't set base values because parameter has {1} elements and needs {2}", _id, values.Length, _baseValues.Length));
				return;
			}

			Array.Copy(_baseValues, values, values.Length);
		}

		public Entity Spawn (Entity creator)
		{
			return new Entity(this, creator);
		}
	}
}

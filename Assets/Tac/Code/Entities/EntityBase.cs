// TAC
// Strategy Game Architecture
// Copyright (c) 2020 Ted Brown

using Jambox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tac
{
	/// <summary>
	/// Stores entity information and creates instances.
	/// </summary>
	public class EntityBase
	{
		/// <summary>Base values of its attributes</summary>
		private int[] _baseValues;

		/// <summary>All ability IDs available to this entity</summary>
		//private List<> _Ids;

		/// <summary>Unique identifier that matches database entry</summary>
		private string _id;

		public EntityBase (string id, int[] values)
		{
			_id = id;
			_baseValues = new int[values.Length];
			Array.Copy(values, _baseValues, values.Length);
	//		_cardIds = new List<string>();
		}

		public EntityBase (string id, int[] values, List<string> cardIds)
		{
			_id = id;
			_baseValues = new int[values.Length];
			Array.Copy(values, _baseValues, values.Length);
	//		_cardIds = new List<string>(cardIds);
		}

		public void GetBaseValues (ref int[] values)
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
			//return new Entity(this, creator, _cardIds);
			return null;
		}
	}
}

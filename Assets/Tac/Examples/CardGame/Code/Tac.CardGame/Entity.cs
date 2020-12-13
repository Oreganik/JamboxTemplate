// TAC
// Strategy Game Architecture
// Copyright (c) 2020 Ted Brown

using Jambox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tac.CardGame
{
	/// <summary>
	/// Plays Cards and can be targeted by Effects.
	/// </summary>
	public class Entity : MonoBehaviour 
	{
		/// <summary>A reference to its base class.</summary>
		private EntityBase _baseEntity;

		/// <summary>The Entity that currently controls this instance.</summary>
		private Entity _controller;

		/// <summary>The Entity that created this instance.</summary>
		private Entity _creator;

		/// <summary>Current values for all attributes: base + effects.</summary>
		private int[] _attributeValues;

		/// <summary>All effects that are active on this instance.</summary>
		private List<Effect> _effects;

		public Entity (EntityBase baseEntity, Entity creator)
		{
			_baseEntity = baseEntity;
			_creator = creator;
			ResetAttributes();
		}

		public int GetValue (AttributeKey key)
		{
			return (_attributeValues[(int)key]);
		}

		private void ResetAttributes ()
		{
			_baseEntity.SetBaseValues(_attributeValues);
		}
	}
}

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
	/// Plays Cards and can be targeted by Effects.
	/// </summary>
	[System.Serializable]
	public class Entity 
	{
		public Action<Sequence> OnDrawCard;

		public Deck Deck
		{
			get { return _deck; }
		}

		public Hand Hand
		{
			get { return _hand; }
		}
		
		private Deck _deck;

		/// <summary>A reference to its base class.</summary>
		private EntityBase _baseEntity;

		/// <summary>The Entity that currently controls this instance.</summary>
		private Entity _controller;

		/// <summary>The Entity that created this instance.</summary>
		private Entity _creator;

		private Hand _hand;

		/// <summary>Current values for all attributes: base + effects.</summary>
		private int[] _attributeValues;

		/// <summary>All effects that are active on this instance.</summary>
		private List<Effect> _effects;

		public Entity (EntityBase baseEntity, Entity creator, List<string> cardIds)
		{
			_baseEntity = baseEntity;
			_creator = creator;
			ResetAttributes();
			_deck = new Deck(cardIds);
			_hand = new Hand();
		}

		public bool Draw ()
		{
			Sequence card = _deck.Draw();

			if (card == null)
			{
				return false;
			}

			_hand.AddCard(card);

			if (OnDrawCard != null)
			{
				OnDrawCard(card);
			}

			return true;
		}

		public int GetValue (AttributeKey key)
		{
			return (_attributeValues[(int)key]);
		}

		private void ResetAttributes ()
		{
			_baseEntity.GetBaseValues(ref _attributeValues);
		}
	}
}

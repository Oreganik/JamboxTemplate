// TAC
// Strategy Game Architecture
// Copyright (c) 2020 Ted Brown

using Jambox;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Tac
{
	/// <summary>
	/// A container for one or more effects. Played by Entities.
	/// </summary>
	public class Sequence 
	{
		public Effect[] Effects
		{
			get { return _effects; }
		}
		
		// NOTE: This is a simplistic value intended as an example.
		public int Cost
		{
			get { return _cost; }
		}

		public FactionType FactionTarget
		{
			get { return _factionTarget; }
		}

		public string Id
		{
			get { return _id; }
		}

		public string LocalizedName
		{
			get { return _id; }
			//get { return Program.LocalizedText.GetText(_id); }
		}
		
		private Effect[] _effects;
		private FactionType _factionTarget;
		private int _cost;
		private string _id;

		public Sequence (string id, int cost, FactionType factionTarget, Effect[] effects)
		{
			_id = id;
			_cost = cost;
			_factionTarget = factionTarget;
			_effects = new Effect[effects.Length];
			Array.Copy(effects, _effects, effects.Length);
		}

		/// <summary>
		/// Activate all effects on this card.
		/// </summary>
		/// <param name="owner">The entity taking action</param>
		/// <param name="targets">One or more affected entities</param>
		public void Play (Entity owner, params Entity[] targets)
		{
			//Display.PrintCardEffect(string.Format("{0} played {1}", owner.Name, LocalizedName));
			foreach (Effect effect in _effects)
			{
				effect.Activate(owner, targets);
			}
		}

		/// <summary>
		/// Prints the card information in a single line.
		/// </summary>
		public override string ToString ()
		{
			StringBuilder text = new StringBuilder();
			text.AppendFormat("{0} ({1} Cost): ", LocalizedName, _cost);
			bool firstEffect = true;
			foreach (Effect effect in _effects)
			{
				if (firstEffect)
				{
					text.Append(effect.ToString());
					firstEffect = false;
				}
				else
				{
					text.AppendFormat(", {0}", effect.ToString());
				}
			}
			return text.ToString();
		}
	}
}

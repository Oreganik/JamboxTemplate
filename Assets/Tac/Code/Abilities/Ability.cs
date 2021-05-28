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
	/// A hardcoded list of effects that generates sequences.
	/// </summary>
	public class Ability 
	{
		public List<Effect> Costs;
		public List<Effect> Effects;
		public readonly string Id;

		protected static string[] s_constructorValues;

		public string Name
		{
			get { return Id; }
			//get { return Program.LocalizedText.GetText(_id); }
		}

		public Ability ()
		{
			Costs = new List<Effect>();
			Effects = new List<Effect>();
		}

		public Ability (string parameters)
		{
		}

		public Ability (string id, Effect[] effects, Effect[] costs = null)
		{
			Id = id;
			Costs = new List<Effect>();
			Effects = new List<Effect>();
		}

		public void AddCost (Effect cost, params int[] values)
		{
			// Costs.Add(cost);
			// Costs = new Effect[costs.Length];
			// Array.Copy(costs, Costs, costs.Length);
		}

		public void AddEffects (Effect[] effects, int[] values)
		{
			// Effects = new Effect[effects.Length];
			// Array.Copy(effects, Effects, effects.Length);
		}

		/// <summary>
		/// Activate all effects on this card.
		/// </summary>
		/// <param name="owner">The entity taking action</param>
		/// <param name="targets">One or more affected entities</param>
		// public void Play (Entity owner, params Entity[] targets)
		// {
		// 	//Display.PrintCardEffect(string.Format("{0} played {1}", owner.Name, LocalizedName));
		// 	foreach (Effect effect in _effects)
		// 	{
		// 		effect.Activate(owner, targets);
		// 	}
		// }

		/// <summary>
		/// Prints the card information in a single line.
		/// </summary>
		public override string ToString ()
		{
			StringBuilder text = new StringBuilder();
			text.AppendFormat("{0}: ", Name);
			bool firstEffect = true;
			foreach (Effect effect in Effects)
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

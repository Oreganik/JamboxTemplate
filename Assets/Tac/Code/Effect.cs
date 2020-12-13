// TAC
// Strategy Game Architecture
// Copyright (c) 2020 Ted Brown

using System;

namespace Tac
{
	/// <summary>
	/// Modifies players or the game. Attached to Cards.
	/// </summary>
    public abstract class Effect
    {
		protected string _effectType;
		protected string _rawModifier;

		/// <summary>
		/// Immediately activates the effect.
		/// </summary>
		/// <param name="owner">The entity taking action</param>
		/// <param name="targets">One or more affected entities</param>
		public abstract void Activate (Entity owner, params Entity[] targets);

		/// <summary>
		/// Sets the "raw" string value of the modifier and lets the subclass do its thing.
		/// </summary>
		/// <param name="effectType">A string (usually based on an enum) that identifies the effect type</param>
		/// <param name="rawModifier">A string describing the 'amount' of the effect, e.g. "1" damage</param>
		public bool Initialize (string effectType, string rawModifier)
		{
			_effectType = effectType;
			_rawModifier = rawModifier;
			return OnInitializeEnd();
		}

		/// <summary>
		/// Returns a string describing the effect.
		/// </summary>
		public override string ToString ()
		{
			return string.Format("{0} {1}", _effectType, _rawModifier);
		}

		/// <summary>
		/// Called after the card is initialized. Often used to parse rawModifiers as int.
		/// </summary>
		protected abstract bool OnInitializeEnd ();

    }
}

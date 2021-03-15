// TAC
// Strategy Game Architecture
// Copyright (c) 2020 Ted Brown

using System;

namespace Tac
{
	/// <summary>
	/// Modifies players or the game. Attached to Cards.
	/// </summary>
    public class Effect
    {
		public bool ErrorOnLoad
		{
			get { return _errorOnLoad; }
		}

		protected AttributeKey _attributeKey;
		protected EffectOperation _operation;
		protected bool _errorOnLoad;
		protected string _rawModifier;

		/// <summary>
		/// Immediately activates the effect.
		/// </summary>
		/// <param name="owner">The entity taking action</param>
		/// <param name="targets">One or more affected entities</param>
		public virtual void Activate (Entity owner, params Entity[] targets)
		{

		}

		/// <summary>
		/// Sets the "raw" string value of the modifier and lets the subclass do its thing.
		/// </summary>
		/// <param name="cardId">The card creating the effect</param>
		/// <param name="rawData">A colon-delimited string containing data that defines the effect</param>
		public Effect (string cardId, string rawData)
		{
			const char EFFECT_VALUE_SEPARATOR = ':';
			const int EFFECT_COLUMNS = 3;

			// Effects are defined like this: operation:attributekey:modifier
			string[] data = rawData.Split(EFFECT_VALUE_SEPARATOR);

			if (data.Length != EFFECT_COLUMNS)
			{
				UnityEngine.Debug.LogError(string.Format("Effect: {0}: Effect entry '{1}' does not have {2} columns", cardId, rawData, EFFECT_COLUMNS));
				return;
			}

			// Schema: Operation [0], AttributeKey[1], Modifier [2]
			// Value is not always an int, so don't cast it.

			// Determine the operation
			if (Enum.TryParse(data[0], ignoreCase: true, out _operation) == false)
			{
				UnityEngine.Debug.LogError(string.Format("Effect: {0}: Could not parse '{1}' as EffectOperation", cardId, data[0]));
				_errorOnLoad = true;
			}

			// Determine the attribute key
			if (Enum.TryParse(data[1], ignoreCase: true, out _attributeKey) == false)
			{
				UnityEngine.Debug.LogError(string.Format("Effect: {0}: Could not parse '{1}' as AttributeKey", cardId, data[1]));
				_errorOnLoad = true;
			}

			_rawModifier = data[2];
		}

		/// <summary>
		/// Modifies a given value using a specified operation and amount.
		/// </summary>
		/// <param name="value">The starting value.</param>
		/// <param name="operation">The EffectOperation to apply.</param>
		/// <param name="modification">The amount used for the operation.</param>
		public static int ModifyValue (int value, EffectOperation operation, int modification)
		{
			switch (operation)
			{
				case EffectOperation.Add:
					value += modification;
					break;

				case EffectOperation.BitwiseAnd:
					value = (value & modification);
					break;

				case EffectOperation.BitwiseOr:
					value = (value | modification);
					break;

				case EffectOperation.BitwiseXor:
					value = (value ^ modification);
					break;

				case EffectOperation.Multiply:
					value *= modification;
					break;

				case EffectOperation.Set:
					value = modification;
					break;
				
				case EffectOperation.Subtract:
					value -= modification;
					break;

				default: // this will catch EffectOperation.Invalid
					UnityEngine.Debug.LogError("Error: Unsupported EffectOperation." + operation);
					break;
			}

			return value;
		}
		
		/// <summary>
		/// Returns a string describing the effect.
		/// </summary>
		public override string ToString ()
		{
			return string.Format("{0} {1} : {2}", _operation, _attributeKey, _rawModifier);
		}
    }
}

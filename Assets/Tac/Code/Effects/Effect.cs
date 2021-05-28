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
		protected Entity _source;
		protected Entity _target;
		protected bool _errorOnLoad;
		protected string _rawModifier;
		protected int _modifier;

		const string EFFECT_STRING = "{0}:{1}:{2}";

		public Effect (Entity source, EffectOperation operation, AttributeKey attribute, int value, Entity target)
		{
			_source = source;
			_operation = operation;
			_attributeKey = attribute;
			_modifier = value;
			_target = target;
		}

		public static string GetEffectString (EffectOperation operation, AttributeKey attribute, int value, bool humanReadable = true)
		{
			if (humanReadable)
			{
				return string.Format(EFFECT_STRING, operation.ToString(), attribute.ToString(), value.ToString());
			}
			return string.Format(EFFECT_STRING, (int)operation, (int)attribute, value);
		}

		/// <summary>
		/// Immediately activates the effect.
		/// </summary>
		/// <param name="owner">The entity taking action</param>
		/// <param name="targets">One or more affected entities</param>
		public virtual void Activate (Entity owner, params Entity[] targets)
		{

		}

		public bool IsValid ()
		{
			if (_attributeKey == AttributeKey.Invalid) return false;
			if (_operation == EffectOperation.Invalid) return false;
			UnityEngine.Debug.Log(this.GetType() + " is valid");
			return true;
		}

		/// <summary>
		/// Sets the "raw" string value of the modifier and lets the subclass do its thing.
		/// </summary>
		/// <param name="rawData">A colon-delimited string containing data that defines the effect - operation:attributekey:modifier</param>
		public void SetEffect (string rawData)
		{
			const char EFFECT_VALUE_SEPARATOR = ':';
			const int EFFECT_COLUMNS = 3;

			// Effects are defined like this: operation:attributekey:modifier
			string[] data = rawData.Split(EFFECT_VALUE_SEPARATOR);

			if (data.Length != EFFECT_COLUMNS)
			{
				UnityEngine.Debug.LogError(string.Format("Effect: Effect entry '{0}' does not have {1} columns", rawData, EFFECT_COLUMNS));
				return;
			}

			// Schema: Operation [0], AttributeKey[1], Modifier [2]

			// Determine the operation
			EffectOperation operation = EffectOperation.Invalid;
			if (Enum.TryParse(data[0], ignoreCase: true, out operation) == false)
			{
				UnityEngine.Debug.LogError(string.Format("Effect: Could not parse '{0}' as EffectOperation", data[0]));
				_errorOnLoad = true;
			}

			// Determine the attribute key
			AttributeKey attributeKey = AttributeKey.Invalid;
			if (Enum.TryParse(data[1], ignoreCase: true, out attributeKey) == false)
			{
				UnityEngine.Debug.LogError(string.Format("Effect: Could not parse '{0}' as AttributeKey", data[1]));
				_errorOnLoad = true;
			}

			// Determine the modifier
			int value = 0;
			if (int.TryParse(data[2], out value) == false)
			{
				UnityEngine.Debug.LogError(string.Format("Effect: Could not parse '{0}' as int", data[2]));
				_errorOnLoad = true;
			}

			//Set(operation, attributeKey, value);
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

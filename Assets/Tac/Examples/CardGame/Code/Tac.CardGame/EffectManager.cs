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
	/// Instantiates effects
	/// </summary>
	public class EffectManager 
	{
		/// <summary>
		/// Returns a new effect instance based on the provided values.
		/// Sets 'error' to true if the effect has not been defined.
		/// </summary>
		public static Effect InstantiateEffect (EffectType effectType, string rawModifier, out bool error)
		{
			Effect effect = null;
			error = false;
			
			switch (effectType)
			{
				case EffectType.Hurt:
					effect = new HurtEffect();
					break;

				default:
					UnityEngine.Debug.LogError("Effect did not recognize EffectType." + effectType);
					break;
			}

			if (effect == null)
			{
				error = true;
			}
			else
			{
				// If the effect does not initialize properly, return a null and force a bad operation
				if (effect.Initialize(effectType.ToString(), rawModifier) == false)
				{
					error = true;
				}
			}

			return effect;
		}
	}
}

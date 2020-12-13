// TAC
// Strategy Game Architecture
// Copyright (c) 2020 Ted Brown

using System;

#if false // THIS IS JUST AN EXAMPLE
namespace Tac
{
	/// <summary>
	/// The owning entity deals X damage to one or more entities.
	/// </summary>
    public class HurtEffect : Effect
    {
		const string EFFECT_TEXT = "{0} hit {1} for {2} damage!";
		private int _modifier;

		public override void Activate (Entity owner, params Entity[] targets)
		{
			foreach (Entity entity in targets)
			{
				// Display.PrintCardEffect(string.Format(EFFECT_TEXT, owner.Name, opponent.Name, _modifier));
				// entity.TakeDamage(_modifier);
			}
		}

		protected override bool OnInitializeEnd ()
		{
			if (int.TryParse(_rawModifier, out _modifier) == false)
			{
				UnityEngine.Debug.LogError(string.Format("{0} could not parse '{1}' as int", this.GetType().ToString(), _rawModifier));
				return false;
			}
			return true;
		}
    }
}
#endif
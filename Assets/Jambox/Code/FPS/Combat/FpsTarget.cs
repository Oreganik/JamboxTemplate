// JAMBOX
// General purpose game code for Unity
// Copyright (c) 2021 Ted Brown

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jambox
{
	/// <summary>
	/// An object must have an FpsTarget to have their stats (like health) affected by projectiles, explosions, etc.
	/// </summary>
	public class FpsTarget : MonoBehaviour 
	{
		public Action<string, int> OnDamage;
		public Action<string, int> OnHeal;

		public FpsStats Stats
		{
			get { return _fpsStats; }
		}

		private FpsStats _fpsStats;

		public void Damage (string stat, int amount)
		{
			if (amount > 0) amount *= -1;
			_fpsStats.Adjust(stat, amount);
			if (OnDamage != null)
			{
				OnDamage(stat, amount);
			}
		}

		public void Heal (string stat, int amount)
		{
			if (amount < 0) amount *= -1;
			_fpsStats.Adjust(stat, amount);
		}

		protected void Awake ()
		{
			_fpsStats = GetComponent<FpsStats>();
		}
	}
}

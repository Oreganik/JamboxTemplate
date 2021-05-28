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
	/// 
	/// </summary>
	public class FpsBreakable : MonoBehaviour 
	{
		private FpsTarget _fpsTarget;

		private void HandleDamage (string id, int value)
		{
			//Debug.Log("Damage: " + value + " => " + _fpsTarget.Stats.Get("health"));
			if (_fpsTarget.Stats.Get("health") <= 0)
			{
				Destroy(gameObject);
			}
		}
		protected void Awake ()
		{
			_fpsTarget = GetComponentInChildren<FpsTarget>();
			_fpsTarget.OnDamage += HandleDamage;
		}
	}
}

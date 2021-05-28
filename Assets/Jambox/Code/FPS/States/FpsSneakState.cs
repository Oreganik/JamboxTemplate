// GAME JAM PROJECT
// Prototype Application
// Copyright (c) 2020 Ted Brown

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jambox
{
	/// <summary>
	/// 
	/// </summary>
	public class FpsSneakState : FpsHeroFiniteState 
	{
		public override Enum Name
		{
			get { return FpsHeroState.Sneak; }
		}

		public float _moveSpeed = 3;

		protected override void OnInitializeEnd ()
		{
		}

		protected override void RegisterTransitions ()
		{
			// AddTransition (Enum trigger, Enum nextState)
		}

		protected override void OnEnterEnd ()
		{
			_hero.Move.SetSpeed(_moveSpeed);
		}

		protected override void OnProcessEnd (float deltaTime)
		{
			if (FpsControl.WasPressedThisFrame(FpsInput.Jump))
			{
				_hero.GoToState(FpsHeroState.Jump);
			}
			else if (FpsControl.WasReleasedThisFrame(FpsInput.Sneak))
			{
				_hero.GoToState(FpsHeroState.Normal);
			}
		}

		protected override void OnExitBegin ()
		{
		}
	}
}

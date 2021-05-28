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
	public class FpsNormalState : FpsHeroFiniteState 
	{
		public override Enum Name
		{
			get { return FpsHeroState.Normal; }
		}

		public override bool IsInitialState
		{
			get { return true; }
		}

		public float _moveSpeed = 5;

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
			_hero.Look.LerpToHeight(FpsLook.BASE_HEIGHT, 0.1f);

			if (FpsControl.WasPressedThisFrame(FpsInput.Crouch))
			{
				_hero.GoToState(FpsHeroState.Crouch);
			}
			else if (FpsControl.WasPressedThisFrame(FpsInput.Jump))
			{
				_hero.GoToState(FpsHeroState.Jump);
			}
			else if (FpsControl.WasPressedThisFrame(FpsInput.Run))
			{
				_hero.GoToState(FpsHeroState.Run);
			}
			else if (FpsControl.WasPressedThisFrame(FpsInput.Sneak))
			{
				_hero.GoToState(FpsHeroState.Sneak);
			}
		}

		protected override void OnExitBegin ()
		{
		}
	}
}

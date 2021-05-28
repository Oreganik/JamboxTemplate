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
	public class FpsCrouchState : FpsHeroFiniteState 
	{
		public override Enum Name
		{
			get { return FpsHeroState.Crouch; }
		}

		public float _moveSpeed = 2;
		public float _cameraHeight = 1;

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
			_hero.Look.LerpToHeight(_cameraHeight, 0.1f);

			if (FpsControl.WasPressedThisFrame(FpsInput.Jump))
			{
				_hero.GoToState(FpsHeroState.Jump);
			}
			else if (FpsControl.WasReleasedThisFrame(FpsInput.Crouch))
			{
				_hero.GoToState(FpsHeroState.Normal);
			}
		}

		protected override void OnExitBegin ()
		{
		}
	}
}

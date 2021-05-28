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
	public class FpsJumpState : FpsHeroFiniteState 
	{
		public override Enum Name
		{
			get { return FpsHeroState.Jump; }
		}

		private bool _wasGravityEnabled;
		private bool _hasJumped;
		private RigidbodyConstraints _baseConstraints;

		protected override void OnInitializeEnd ()
		{
			Physics.gravity = Physics.gravity * 2;
			_baseConstraints = _hero.Rigidbody.constraints;
		}

		protected override void RegisterTransitions ()
		{
			// AddTransition (Enum trigger, Enum nextState)
		}

		protected override void OnEnterEnd ()
		{
			_wasGravityEnabled = _hero.Rigidbody.useGravity;
			_hero.Rigidbody.useGravity = true;
			// Unset FreezePositionY
			_hero.Rigidbody.constraints = _baseConstraints & ~RigidbodyConstraints.FreezePositionY;
			_hasJumped = false;
		}

		protected override void OnProcessEnd (float deltaTime)
		{
			_hero.Look.LerpToHeight(FpsLook.BASE_HEIGHT, 0.3f);
			Vector3 position = transform.position;
			if (_hasJumped && position.y <= 0.01f)
			{
				position.y = 0;
				transform.position = position;
				_hero.GoToState(FpsHeroState.Normal);
			}
		}

		protected override void OnExitBegin ()
		{
			_hero.Rigidbody.useGravity = _wasGravityEnabled;
			// Set FreezePositionY
			_hero.Rigidbody.constraints = _baseConstraints;
		}

		protected void FixedUpdate ()
		{
			// If a jump hasn't been triggered AND the "FreezePositionY" constraint has been lifted, jump
			if (_hasJumped == false && (_hero.Rigidbody.constraints & RigidbodyConstraints.FreezePositionY) == 0)
			{
				_hero.Rigidbody.velocity += -Physics.gravity * 0.4f;
				_hasJumped = true;
			}

		}
	}
}

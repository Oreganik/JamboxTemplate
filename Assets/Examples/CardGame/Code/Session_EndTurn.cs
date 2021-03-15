// GAME JAM PROJECT
// Prototype Application
// Copyright (c) 2020 Ted Brown

using Jambox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tac
{
	/// <summary>
	/// 
	/// </summary>
	public class Session_EndTurn : FState 
	{
		public override Enum Name
		{
			get { return TacState.EndTurn; }
		}

		protected override void OnInitializeEnd ()
		{
		}

		protected override void RegisterTransitions ()
		{
			// AddTransition (Enum trigger, Enum nextState)
		}

		protected override void OnEnterEnd ()
		{
		}

		protected override void OnProcessEnd (float deltaTime)
		{
		}

		protected override void OnExitBegin ()
		{
		}
	}
}

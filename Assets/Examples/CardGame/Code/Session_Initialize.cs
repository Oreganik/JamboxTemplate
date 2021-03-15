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
	public class Session_Initialize : FState 
	{
		public override Enum Name
		{
			get { return TacState.Initialize; }
		}

		public override bool IsInitialState
		{
			get { return true; }
		}

		protected override void OnInitializeEnd ()
		{
		}

		protected override void RegisterTransitions ()
		{
			AddTransition (TacEvent.InitializeSuccess, TacState.PreGame);
		}

		protected override void OnEnterEnd ()
		{
			if (Session.Instance.Initialize())
			{
				_fsm.RaiseEvent(TacEvent.InitializeSuccess);
			}
		}

		protected override void OnProcessEnd (float deltaTime)
		{
		}

		protected override void OnExitBegin ()
		{
		}
	}
}

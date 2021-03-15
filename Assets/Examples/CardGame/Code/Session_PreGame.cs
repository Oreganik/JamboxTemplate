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
	public class Session_PreGame : FState 
	{
		public override Enum Name
		{
			get { return TacState.PreGame; }
		}

		private int remainingCards;

		protected override void OnInitializeEnd ()
		{
		}

		protected override void RegisterTransitions ()
		{
			// AddTransition (Enum trigger, Enum nextState)
		}

		protected override void OnEnterEnd ()
		{
			remainingCards = 5;
		}

		protected override void OnProcessEnd (float deltaTime)
		{
			if (remainingCards <= 0) return;

			if (Hud.Instance.IsBusy == false)
			{
				remainingCards--;
				Session.Instance.ActivePlayer.Draw();
				Session.Instance.OtherPlayer.Draw();
			}
		}

		protected override void OnExitBegin ()
		{
		}
	}
}

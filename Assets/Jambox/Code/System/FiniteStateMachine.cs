using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jambox
{
	public class FiniteStateMachine : MonoBehaviour 
	{
		private FiniteState currentFiniteState;
		private FiniteState[] finiteStates;

		public void GoToState (FiniteState newFiniteState)
		{
			if (currentFiniteState != null)
			{
				currentFiniteState.Exit();
			}

			if (newFiniteState != null)
			{
				newFiniteState.Enter();
			}

			currentFiniteState = newFiniteState;
		}

		// as long as this is not called multiple times a frame, this tradeoff of efficiency for flexibility has proven to be worth it (ted)
		public void GoToState (string stateName)
		{
			stateName = stateName.ToLower();
			FiniteState targetState = null;

			foreach (FiniteState finiteState in finiteStates)
			{
				if (finiteState.GetType().ToString().ToLower().Equals(stateName))
				{
					targetState = finiteState;
					break;
				}
			}

			if (targetState != null)
			{
				GoToState(targetState);
			}
			else
			{
				Debug.LogError("FiniteStateController could not find state named '" + stateName + "'");
			}
		}

		public void Initialize ()
		{
			finiteStates = transform.GetComponentsInChildren<FiniteState>();
			foreach (FiniteState finiteState in finiteStates)
			{
				finiteState.Initialize(this);
			}
		}

		public void Process () 
		{
			if (currentFiniteState != null)
			{
				currentFiniteState.Process();
			}
		}
	}
}

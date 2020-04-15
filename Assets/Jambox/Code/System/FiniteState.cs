using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jambox 
{
	public class FiniteState : MonoBehaviour 
	{
		[Tooltip("This will probably be null if the state has two or more possible next states.")]
		public FiniteState nextState;

		protected FiniteStateMachine fsm;

		public void Initialize (FiniteStateMachine fsm)
		{
			this.fsm = fsm;
			gameObject.SetActive(false);
			OnInitialize();
		}

		public void Enter ()
		{
			gameObject.SetActive(true);
			OnEnter();
		}

		public void Process ()
		{
			OnProcess();
		}

		public void Exit ()
		{
			OnExit();
			gameObject.SetActive(false);
		}

		protected virtual void OnInitialize () {}
		protected virtual void OnEnter () {}
		protected virtual void OnProcess () {}
		protected virtual void OnExit () {}
	}
}

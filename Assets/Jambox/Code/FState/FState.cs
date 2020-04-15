// JAMBOX
// General purpose game code for Unity
// Copyright 2020 Ted Brown

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NetStack
{
    public abstract class FState : MonoBehaviour 
    {
		public abstract Enum Name
		{
			get;
		}

		// Should only be overridden by the initial state for the system you are creating
		public virtual bool IsInitialState
		{
			get { return false; }
		}

		public List<FTransition> Transitions
		{
			get { return _transitions; }
		}

		protected bool _isStopped;
		protected FMachine _cMachine;
		protected List<FTransition> _transitions;

		public void Initialize (FMachine cMachine)
		{
			_cMachine = cMachine;
			_transitions = new List<FTransition>();
			RegisterTransitions();
			gameObject.SetActive(false);
		}

		public void Enter ()
		{
			Debug.Log(this.GetType().ToString() + ".Enter", gameObject);
			gameObject.SetActive(true);
			_isStopped = false;
			OnEnterEnd();
		}

		public void Process (float deltaTime)
		{
			if (_isStopped == false)
			{
				OnProcessEnd(deltaTime);
			}
		}

		public void Exit ()
		{
			Debug.Log(this.GetType().ToString() + ".Exit", gameObject);
			OnExitBegin();
			gameObject.SetActive(false);
		}

		protected virtual void AddTransition (Enum trigger, Enum nextState)
		{
			foreach (FTransition transition in _transitions)
			{
				// important: == will always return false in this situation
				if (transition.Trigger.Equals(trigger))
				{
					Debug.LogWarning(this.GetType().ToString() + ".AddTransition(" + trigger + ", " + nextState + ") ignored. That trigger is already in use.", gameObject);
					return;
				}
			}
			_transitions.Add(new FTransition(trigger, this.Name, nextState));
		}

		protected abstract void RegisterTransitions ();

		protected virtual void OnEnterEnd ()
		{
		}

		protected virtual void OnProcessEnd (float deltaTime)
		{
		}

		protected virtual void OnExitBegin ()
		{
		}
    }
}

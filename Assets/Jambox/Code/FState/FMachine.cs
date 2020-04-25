// JAMBOX
// General purpose game code for Unity
// Copyright 2020 Ted Brown

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jambox
{
    public class FMachine : MonoBehaviour 
    {
		public FState ActiveState
		{
			get { return _activeState; }
		}

		public FState PreviousState
		{
			get { return _previousState; }
		}

		public FTransition PreviousTransition
		{
			get { return _previousTransition; }
		}

		public bool PrintDebug = false;
		public bool OperateEvenIfPaused;

		private FState _activeState;
		private FState _previousState;
		private Dictionary<Enum, FState> _finiteStates;
		private FTransition _previousTransition;

		public void RaiseEvent (Enum trigger)
		{
			// Verify the enum is the accepted type
			// if (eventType.GetType() != typeof(Enum))
			// {
			// 	Debug.Log("Invalid type " + eventType.GetType());
			// 	return;
			// }

			if (_activeState == null)
			{
				Debug.LogError(this.GetType().ToString() + ".RaiseEvent(" + trigger + ") was called with no active state", gameObject);
				return;
			}

			FTransition transition = null;

			foreach (FTransition t in _activeState.Transitions)
			{
				// important: == will always return false in this situation
				if (t.Trigger.Equals(trigger))
				{
					transition = t;
					break;
				}
			}

			if (transition != null)
			{
				if (PrintDebug)
				{
					Debug.Log(this.GetType().ToString() + ".Transition: " + transition.ToString());
				}
				_previousTransition = transition;
				GoToState(transition.NextState);
			}
			else
			{
				Debug.LogError(this.GetType().ToString() + ".RaiseEvent(" + trigger + ") was called on active state " + _activeState.Name + " but no matching transition was found.", gameObject);
			}
		}

		protected void GoToState (Enum stateName)
		{
			if (_activeState != null)
			{
				_previousState = _activeState;
				_activeState.Exit();
			}

			if (_finiteStates.TryGetValue(stateName, out _activeState))
			{
				_activeState.Enter();
			}
			else
			{
				Debug.LogError(this.GetType().ToString() + ".GoToState(" + stateName + "): Unknown state!", gameObject);
				// Enum.GetName(stateName.GetType(), stateName) // what was this for?
			}
		}

		// Add and initialize all states
		protected void Awake ()
		{
			FState[] states = GetComponentsInChildren<FState>();
			_finiteStates = new Dictionary<Enum, FState>();

			foreach (FState state in states)
			{
				if (state.Name == null)
				{
					Debug.LogError(this.GetType().ToString() + ".Awake: Ignored null state name on " + state.gameObject.name, gameObject);
					continue;
				}

				if (_finiteStates.ContainsKey(state.Name) == false)
				{
					_finiteStates.Add(state.Name, state);
					state.Initialize(this);
				}
				else
				{
					Debug.LogWarning(this.GetType().ToString() + ".Awake: Ignored duplicate state " + state.Name + " on " + state.gameObject.name, gameObject);
				}
			}
		}

		protected void OnDestroy ()
		{
			if (_activeState)
			{
				_activeState.Exit();
			}
		}

		// Find the initial state and enter it.
		// Do this on Start so singletons and other objects with the same Execution Order have time to initialize.
		protected void Start ()
		{
			Enum initialState = null;
			List<Enum> keys = new List<Enum>(_finiteStates.Keys);

			// Find the initial state and enter it
			foreach (Enum key in keys)
			{
				if (_finiteStates[key].IsInitialState)
				{
					if (initialState == null)
					{
						initialState = _finiteStates[key].Name;
					}
					else
					{
						Debug.LogWarning(this.GetType().ToString() + ".Start: Multiple states flagged as initial state. It's currently " + initialState + ". Should it be " + _finiteStates[key].Name + "?", gameObject);
					}
				}
			}

			if (initialState != null)
			{
				GoToState(initialState);
			}
			else
			{
				Debug.LogError(this.GetType().ToString() + ".Start: No initial state!!!", gameObject);
			}
		}

		protected void Update ()
		{
			if (OperateEvenIfPaused == false && App.Instance && App.Instance.IsPaused)
			{
				return;
			}
			
			if (_activeState)
			{
				_activeState.Process(Time.deltaTime);
			}
		}
    }
}

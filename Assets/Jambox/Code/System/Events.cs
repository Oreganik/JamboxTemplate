// JAMBOX
// General purpose game code for Unity
// Copyright 2019 Ted Brown

using System;
using System.Collections.Generic;
using UnityEngine;

// https://stackoverflow.com/questions/25021078/create-dictionary-for-delegates-and-events

namespace Jambox
{
	// A general-purpose event broadcaster that uses an enum called EventType to manage
	// subscribing to, unsubscribing from, and triggering of Actions with a generic object parameter.
	public class Events : MonoBehaviour 
	{
		public static Events Instance
		{
			get 
			{ 
				if (_instance == null)
				{
					GameObject go = new GameObject("Events");
					_instance = go.AddComponent<Events>();
				}
				return _instance; 
			}
		}
		private static Events _instance;

		public delegate void EventTriggered (EventType eventType, object obj = null);

		/// <summary>
		/// This will be called when any event is broadcast.
		/// </summary>
		public EventTriggered OnEventTriggered;

		#pragma warning disable 0649
		[SerializeField] private bool _debug;
		#pragma warning restore 0649

		/* For every event in EventType, define an action like this:
		private Action<object> OnFinishRace;
		*/

		private static Dictionary<EventType, Action<object>> _eventActions;

		public void BroadcastEvent (EventType eventType, object obj = null)
		{
			Action<object> eventAction = null;

			if (_eventActions.TryGetValue(eventType, out eventAction))
			{
				if (_eventActions[eventType] != null)
				{
					if (obj == null)
					{
						_eventActions[eventType](obj);
					}
					else
					{
						_eventActions[eventType](obj);
					}
				}
			}
			else if (_debug)
			{
				Debug.LogWarning("Found no events for " + eventType.ToString());
			}

			if (OnEventTriggered != null)
			{
				OnEventTriggered(eventType, obj);
			}
		}

		public void SubscribeEvent (EventType eventType, Action<object> actionName)
		{
			Action<object> eventAction = null;

			if (_eventActions.TryGetValue(eventType, out eventAction))
			{
				_eventActions[eventType] += actionName;
			}
			else
			{
				Debug.LogError("Subscribe FAIL: No action defined for EventType." + eventType.ToString());
			}
		}

		public void UnsubscribeEvent (EventType eventType, Action<object> actionName)
		{
			Action<object> eventAction = null;

			if (_eventActions.TryGetValue(eventType, out eventAction))
			{
				_eventActions[eventType] -= actionName;
			}
		}

		private void Awake ()
		{
			_eventActions = new Dictionary<EventType, Action<object>>()
			{
				/* For every event in EventType, add its enum and action like this:
				{ EventType.FinishRace, OnFinishRace },
				*/
			};
		}

		private void OnDestroy ()
		{
			Action<object> eventAction = null;
			int count = System.Enum.GetValues(typeof(EventType)).Length;
			for (int i = 0; i < count; i++)
			{
				EventType eventType = (EventType) i;
				if (_eventActions.TryGetValue(eventType, out eventAction))
				{
					_eventActions[eventType] = null;
				}
			}
		}
	}
}

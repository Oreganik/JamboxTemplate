///////////////////////////////////////////////////////////////////////////////
//
//	App.cs
//	Part of the JamBox Plugin for Unity
//
//	Copyright (c) 2015, Oreganik LLC
//	All rights reserved.
//
///////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

namespace Jambox
{
	public enum AppState { Title, Gameplay }

	public class App : MonoBehaviour 
	{
		public static App Instance
		{
			get
			{
				if (_instance == null)
				{
					GameObject go = new GameObject("App");
					_instance = go.AddComponent<App>();
				}
				return _instance;
			}
		}

		public static float deltaGameTime
		{
			get
			{
				if (_instance.IsPaused) return 0;
				else return Time.deltaTime * Time.timeScale;
			}
		}

		private static App _instance;

		public bool IsPaused
		{
			get { return _isPaused; }
		}

		private bool _isPaused;

		public void Pause ()
		{
			_isPaused = true;
			Time.timeScale = 0;
			Events.Instance.BroadcastEvent(EventType.Pause);
		}

		public void TogglePaused ()
		{
			if (_isPaused)
			{
				Pause();
			}
			else
			{
				Unpause();
			}
		}

		public void Unpause ()
		{
			_isPaused = false;
			Time.timeScale = 1;
			Events.Instance.BroadcastEvent(EventType.Unpause);
		}


		private void Update ()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				if (Application.isEditor)
				{
					Debug.Break();
				}
				else
				{
//					Application.Quit();
				}
			}

			if (Input.GetKeyDown(KeyCode.P))
			{
				TogglePaused();
			}
		}

	}
}

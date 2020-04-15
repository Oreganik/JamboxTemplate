// JAMBOX
// General purpose game code for Unity
// Copyright 2019 Ted Brown

using System;
using UnityEngine;

public class AppTime : MonoBehaviour
{
	public static Action<bool> OnPause;

	public static bool IsPaused
	{
		get { return s_paused; }
	}

	public static float deltaGameTime
	{
		get { return s_paused ? 0 : Time.deltaTime; }
	}

	private static bool s_paused;

	public static void Pause ()
	{
		s_paused = true;
		if (OnPause != null)
		{
			OnPause(s_paused);
		}
	}

	public static void TogglePause ()
	{
		if (s_paused) Unpause();
		else Pause();
	}

	public static void Unpause ()
	{
		s_paused = false;
		if (OnPause != null)
		{
			OnPause(s_paused);
		}
	}
}

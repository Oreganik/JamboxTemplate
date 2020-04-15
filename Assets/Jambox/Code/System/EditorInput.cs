// JAMBOX
// General purpose game code for Unity
// Copyright (c) 2020 Ted Brown

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jambox
{
	// Debug Inputs that only work if the game is being played in the Editor
	public class EditorInput 
	{
		public static bool GetKey (KeyCode key)
		{
			#if UNITY_EDITOR
			return Input.GetKey(key);
			#else
			return false;
			#endif
		}

		public static bool GetKeyDown (KeyCode key)
		{
			#if UNITY_EDITOR
			return Input.GetKeyDown(key);
			#else
			return false;
			#endif
		}

		public static bool GetKeyUp (KeyCode key)
		{
			#if UNITY_EDITOR
			return Input.GetKeyUp(key);
			#else
			return false;
			#endif
		}
	}
}

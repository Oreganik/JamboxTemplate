// JAMBOX
// General purpose game code for Unity
// Copyright (c) 2020 Ted Brown

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jambox
{
	// Debug Inputs that only work if the game is being played in DEBUG mode (editor or exported with Developer Mode)
	public class DebugInput 
	{
		public static bool GetKey (KeyCode key, bool editorOnly = false)
		{
			#if DEBUG
			if (editorOnly && Application.isEditor == false) return false;
			return Input.GetKey(key);
			#else
			return false;
			#endif
		}

		public static bool GetKeyDown (KeyCode key, bool editorOnly = false)
		{
			#if DEBUG
			if (editorOnly && Application.isEditor == false) return false;
			return Input.GetKeyDown(key);
			#else
			return false;
			#endif
		}

		public static bool GetKeyUp (KeyCode key, bool editorOnly = false)
		{
			#if DEBUG
			if (editorOnly && Application.isEditor == false) return false;
			return Input.GetKeyUp(key);
			#else
			return false;
			#endif
		}
	}
}

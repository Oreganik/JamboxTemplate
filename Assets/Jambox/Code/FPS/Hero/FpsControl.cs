// JAMBOX
// General purpose game code for Unity
// Copyright (c) 2021 Ted Brown

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jambox
{
	/// <summary>
	/// 
	/// </summary>
	public class FpsControl
	{
		private static Dictionary<FpsInput, HashSet<KeyCode>> s_keyboardInputs;
		private static Dictionary<FpsInput, int> s_mouseInputs;
		private static HashSet<KeyCode> s_keysToCheck;

		public static void Initialize (bool shiftRunNotSneak)
		{
			s_keyboardInputs = new Dictionary<FpsInput, HashSet<KeyCode>>();
			s_mouseInputs = new Dictionary<FpsInput, int>();

			// Move
			AddKeyboardInput(FpsInput.Crouch, KeyCode.LeftControl);
			AddKeyboardInput(FpsInput.Crouch, KeyCode.C);
			AddKeyboardInput(FpsInput.Jump, KeyCode.Space);
			if (shiftRunNotSneak)
			{
				AddKeyboardInput(FpsInput.Run, KeyCode.LeftShift);
			}
			else
			{
				AddKeyboardInput(FpsInput.Sneak, KeyCode.LeftShift);
			}

			// Do Stuff
			AddKeyboardInput(FpsInput.Interact, KeyCode.E);
			AddKeyboardInput(FpsInput.Reload, KeyCode.R);
			AddMouseInput(FpsInput.Aim, 1);
			AddMouseInput(FpsInput.Shoot, 0);

			// UI
			AddKeyboardInput(FpsInput.HeroMenu, KeyCode.Tab);
			AddKeyboardInput(FpsInput.Screenshot, KeyCode.F12);
			AddKeyboardInput(FpsInput.Screenshot, KeyCode.Print);
			AddKeyboardInput(FpsInput.SystemMenu, KeyCode.Escape);

			// Menus
			AddKeyboardInput(FpsInput.Cancel, KeyCode.Escape);
			AddKeyboardInput(FpsInput.Confirm, KeyCode.Return);
			AddKeyboardInput(FpsInput.Confirm, KeyCode.KeypadEnter);
			AddKeyboardInput(FpsInput.MenuDown, KeyCode.DownArrow);
			AddKeyboardInput(FpsInput.MenuLeft, KeyCode.LeftArrow);
			AddKeyboardInput(FpsInput.MenuRight, KeyCode.RightArrow);
			AddKeyboardInput(FpsInput.MenuUp, KeyCode.UpArrow);
			AddKeyboardInput(FpsInput.MenuNext, KeyCode.RightBracket);
			AddKeyboardInput(FpsInput.MenuPrevious, KeyCode.LeftBracket);
			AddMouseInput(FpsInput.Cancel, 1);
			AddMouseInput(FpsInput.Confirm, 0);
		}

		public static void AddKeyboardInput (FpsInput input, KeyCode key)
		{
			if (s_keyboardInputs.TryGetValue(input, out s_keysToCheck) == false)
			{
				s_keyboardInputs.Add(input, new HashSet<KeyCode>() { key } );
			}
			else
			{
				s_keysToCheck.Add(key);
			}
		}

		public static void AddMouseInput (FpsInput input, int mouseButton)
		{
			if (s_mouseInputs.ContainsKey(input))
			{
				s_mouseInputs[input] = mouseButton;
			}
			else
			{
				s_mouseInputs.Add(input, mouseButton);
			}
		}

		public static KeyCode GetKeyboardInput (FpsInput input)
		{
			if (s_keyboardInputs.TryGetValue(input, out s_keysToCheck))
			{
				KeyCode value = KeyCode.Question;
				// get the first thing we find and exit the loop
				foreach (KeyCode code in s_keysToCheck)
				{
					value = code;
					break;
				}
				return value;
			}
			Debug.LogWarningFormat("FpsControl.GetKeyboardInput: No entry found for FpsInput.{0}", input.ToString());
			return KeyCode.Question;
		}

		public static bool IsPressed (FpsInput input)
		{
			int mouseButton = 0;
			if (s_mouseInputs.TryGetValue(input, out mouseButton) && Input.GetMouseButton(mouseButton))
			{
				return true;
			}
			if (s_keyboardInputs.TryGetValue(input, out s_keysToCheck))
			{
				foreach (KeyCode key in s_keysToCheck)
				{
					if (Input.GetKey(key))
					{
						return true;
					}
				}
			}
			return false;
		}

		public static bool WasPressedThisFrame (FpsInput input)
		{
			int mouseButton = 0;
			if (s_mouseInputs.TryGetValue(input, out mouseButton) && Input.GetMouseButtonDown(mouseButton))
			{
				return true;
			}
			if (s_keyboardInputs.TryGetValue(input, out s_keysToCheck))
			{
				foreach (KeyCode key in s_keysToCheck)
				{
					if (Input.GetKeyDown(key))
					{
						return true;
					}
				}
			}
			return false;
		}

		public static bool WasReleasedThisFrame (FpsInput input)
		{
			int mouseButton = 0;
			if (s_mouseInputs.TryGetValue(input, out mouseButton) && Input.GetMouseButtonUp(mouseButton))
			{
				return true;
			}
			if (s_keyboardInputs.TryGetValue(input, out s_keysToCheck))
			{
				foreach (KeyCode key in s_keysToCheck)
				{
					if (Input.GetKeyUp(key))
					{
						return true;
					}
				}
			}
			return false;
		}

		public static Vector2 GetLook ()
		{
			Vector2 input = Vector2.zero;
			float mouseX = Input.GetAxis("Mouse X");
			float mouseY = Input.GetAxis("Mouse Y");
			return new Vector2(mouseX, mouseY);
		}

		/// <summary>Returns a normalized Vector2 movement</summary>
		public static Vector2 GetMove ()
		{
			Vector2 input = Vector2.zero;

			if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
			{
				input += Vector2.up;
			}

			if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
			{
				input += Vector2.down;
			}

			if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
			{
				input += Vector2.left;
			}

			if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
			{
				input += Vector2.right;
			}

			return input.normalized;
		}
	}
}

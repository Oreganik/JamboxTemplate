// JAMBOX
// General purpose game code for Unity
// Copyright (c) 2020 Ted Brown

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jambox
{
	// Maps broad command sets (e.g. primary, move) to multiple input types (e.g. keyboard, mouse, controller)
	// Handles movement input as well

	// MOVE & LOOK
	// - Keyboard + Mouse: Move is WASD or Arrows. Look is mouse movement.
	// - Gamepad: Move is left stick, Look is right stick.

	// ACTIONS
	// - 0 through 3 (0 is "fire" and 1 is "jump")
	// - Menu
	// - Pause
	// - Submit
	// - Cancel
	// - Screenshot

	// This uses the old (total garbage) Input Manager, so YMMV. See posts like this to learn more about how it manages gamepad axes and buttons per platform.
	// https://answers.unity.com/questions/1350081/xbox-one-controller-mapping-solved.html

	// GAMEPAD
	// INPUT			WINDOWS		MAC
	// Right Stick X	Axis 4		Axis 3
	// Right Stick Y	Axis 5		Axis 4
	// D-Pad X			Axis 6*		n/a
	// D-Pad Y			Axis 7*		n/a
	// D-Pad Up			n/a			5
	// D-Pad Down		n/a			6
	// D-Pad Left		n/a			7
	// D-Pad Right		n/a			8
	// Trigger			not used: historically unreliable :(
	// * not implemented in default template

	public class PInput
	{
		private const string JOYSTICK_BUTTON = "joystick button {0}"; // use with string.format
		public static bool IsUsingGamepad
		{
			get
			{
				return s_isUsingGamepad;
			}
		}

		private static bool s_isUsingGamepad;
		private static GamepadButton[] s_gamepadButtons;
		private static KeyCode[] s_keyCodes;

		// Make an easy-to-use mapping for Xbox gamepad buttons
		private static Dictionary<GamepadButton, int> s_gamepadButtonIds = new Dictionary<GamepadButton, int>()
		{
#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
			{ GamepadButton.A, 16 },
			{ GamepadButton.B, 17 },
			{ GamepadButton.X, 18 },
			{ GamepadButton.Y, 19 },
			{ GamepadButton.LeftBumper, 13 },
			{ GamepadButton.RightBumper,14  },
			{ GamepadButton.LeftStick, 11 },
			{ GamepadButton.RightStick, 12 },
			{ GamepadButton.Back, 10 },
			{ GamepadButton.Start, 9 },
#else // Windows default
			{ GamepadButton.A, 0 },
			{ GamepadButton.B, 1 },
			{ GamepadButton.X, 2 },
			{ GamepadButton.Y, 3 },
			{ GamepadButton.LeftBumper, 4 },
			{ GamepadButton.RightBumper, 5 },
			{ GamepadButton.LeftStick, 8 },
			{ GamepadButton.RightStick, 9 },
			{ GamepadButton.Back, 6 },
			{ GamepadButton.Start, 7 },
#endif
		};

		private static Dictionary<PAction, GamepadButton[]> s_gamepadButtonActions = new Dictionary<PAction, GamepadButton[]>()
		{
			// It's ok to overload a button with multiple actions, or use multiple buttons for an action.
			// Remove ones you don't want, or even add some more.
			{ PAction.Action0, new GamepadButton[] { GamepadButton.A }},
			{ PAction.Action1, new GamepadButton[] { GamepadButton.X }},
			{ PAction.Action2, new GamepadButton[] { GamepadButton.B }},
			{ PAction.Action3, new GamepadButton[] { GamepadButton.Y }},
			{ PAction.Menu, new GamepadButton[] { GamepadButton.Start }},
			{ PAction.Pause, new GamepadButton[] { GamepadButton.Back }},
			{ PAction.Submit, new GamepadButton[] { GamepadButton.A, GamepadButton.X, GamepadButton.Start }},
			{ PAction.Cancel, new GamepadButton[] { GamepadButton.B, GamepadButton.Y, GamepadButton.Back }},
		};

		private static Dictionary<PAction, KeyCode[]> s_keyboardActions = new Dictionary<PAction, KeyCode[]>()
		{
			// It's ok to overload a button with multiple actions, or use multiple buttons for an action.
			// Remove ones you don't want, or even add some more.
			{ PAction.Action0, new KeyCode[] { KeyCode.Z, KeyCode.LeftControl }},
			{ PAction.Action1, new KeyCode[] { KeyCode.X, KeyCode.Space }},
			{ PAction.Action2, new KeyCode[] { KeyCode.C }},
			{ PAction.Action3, new KeyCode[] { KeyCode.V }},
			{ PAction.Menu, new KeyCode[] { KeyCode.Escape }},
			{ PAction.Pause, new KeyCode[] { KeyCode.P }},
			{ PAction.Submit, new KeyCode[] { KeyCode.Return }},
			{ PAction.Cancel, new KeyCode[] { KeyCode.Escape }},
			{ PAction.Screenshot, new KeyCode[] { KeyCode.F12, KeyCode.Print }},
		};

		private static Dictionary<PAction, int> s_mouseButtonActions = new Dictionary<PAction, int>()
		{
			{ PAction.Action0, 0 },
			{ PAction.Action1, 1 },
			{ PAction.Action2, 2 }, // middle mouse button
		};

		public static string GetJoystickButtonName(GamepadButton button)
		{
			return string.Format(JOYSTICK_BUTTON, s_gamepadButtonIds[button]);
		}

		public static bool GetInput (PAction command, bool ignoreMouseButtons = false)
		{
			// Keyboard
			if (s_keyboardActions.TryGetValue(command, out s_keyCodes))
			{
				foreach (KeyCode keyCode in s_keyCodes)
				{
					if (Input.GetKey(keyCode))
					{
						s_isUsingGamepad = false;
						return true;
					}
				}
			}

			// Mouse
			if (ignoreMouseButtons == false && s_mouseButtonActions.ContainsKey(command))
			{
				if (Input.GetMouseButton(s_mouseButtonActions[command]))
				{
					s_isUsingGamepad = false;
					return true;
				}
			}

			// Gamepad
			if (s_gamepadButtonActions.TryGetValue(command, out s_gamepadButtons))
			{
				foreach (GamepadButton button in s_gamepadButtons)
				{
					if (Input.GetButton(GetJoystickButtonName(button)))
					{
						s_isUsingGamepad = true;
						return true;
					}
				}
			}

			return false;
		}

		public static bool GetInputDown (PAction command, bool ignoreMouseButtons = false)
		{
			// Keyboard
			if (s_keyboardActions.TryGetValue(command, out s_keyCodes))
			{
				foreach (KeyCode keyCode in s_keyCodes)
				{
					if (Input.GetKeyDown(keyCode))
					{
						s_isUsingGamepad = false;
						return true;
					}
				}
			}

			// Mouse
			if (ignoreMouseButtons == false && s_mouseButtonActions.ContainsKey(command))
			{
				if (Input.GetMouseButtonDown(s_mouseButtonActions[command]))
				{
					s_isUsingGamepad = false;
					return true;
				}
			}

			// Gamepad
			if (s_gamepadButtonActions.TryGetValue(command, out s_gamepadButtons))
			{
				foreach (GamepadButton button in s_gamepadButtons)
				{
					if (Input.GetButtonDown(GetJoystickButtonName(button)))
					{
						s_isUsingGamepad = true;
						return true;
					}
				}
			}

			return false;
		}

		public static bool GetInputUp (PAction command, bool ignoreMouseButtons = false)
		{
			// Keyboard
			if (s_keyboardActions.TryGetValue(command, out s_keyCodes))
			{
				foreach (KeyCode keyCode in s_keyCodes)
				{
					if (Input.GetKeyUp(keyCode))
					{
						s_isUsingGamepad = false;
						return true;
					}
				}
			}

			// Mouse
			if (ignoreMouseButtons == false && s_mouseButtonActions.ContainsKey(command))
			{
				if (Input.GetMouseButtonUp(s_mouseButtonActions[command]))
				{
					s_isUsingGamepad = false;
					return true;
				}
			}

			// Gamepad
			if (s_gamepadButtonActions.TryGetValue(command, out s_gamepadButtons))
			{
				foreach (GamepadButton button in s_gamepadButtons)
				{
					if (Input.GetButtonUp(GetJoystickButtonName(button)))
					{
						s_isUsingGamepad = true;
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

			if (mouseX != 0 || mouseY != 0)
			{
				s_isUsingGamepad = false;
				return new Vector2(mouseX, mouseY);
			}

			// NOTE: This is with the old input manager, and assumes new axes have been added
			// Mac OS
			if (Application.platform == RuntimePlatform.OSXEditor ||			
				Application.platform == RuntimePlatform.OSXPlayer)
			{
				input = new Vector2(Input.GetAxis("Axis 3"), Input.GetAxis("Axis 4"));
			}
			else // windows input axes by default
			{
				input = new Vector2(Input.GetAxis("Axis 4"), Input.GetAxis("Axis 5"));
			}

			if (input.magnitude > 0)
			{
				s_isUsingGamepad = true;
			}

			return input;
		}

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

			if (input.magnitude > 0)
			{
				s_isUsingGamepad = false;
				return input.normalized;
			}

			// NOTE: This is with the old input manager, and assumes the Horizontal and Vertical axes for keyboard have been deleted.
			input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

			if (input.magnitude > 0)
			{
				s_isUsingGamepad = true;
			}

			return input;
		}
	}
}

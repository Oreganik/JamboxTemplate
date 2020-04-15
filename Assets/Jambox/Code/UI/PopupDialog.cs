// JAMBOX
// General purpose game code for Unity
// Copyright (c) 2020 Ted Brown

using Jambox;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Jambox
{
	public class PopupDialog : MonoBehaviour 
	{
		public static bool IsOpen
		{
			get { return s_instance._container.gameObject.activeSelf; }
		}

		public static Action OnDialogClose;
		public static Action OnDialogOpen;

		private static PopupDialog s_instance;

		#pragma warning disable 0649
		[SerializeField] private RectTransform _container;
		[SerializeField] private TMP_Text _title;
		[SerializeField] private TMP_Text _text; 
		[SerializeField] private Button[] _buttons;
		[SerializeField] private TMP_Text[] _options;
		#pragma warning restore 0649

		private Action[] _actions;
		private PAction[] _buttonCommands;
		private Vector3[] _baseButtonPositions;

		public static void Close ()
		{
			s_instance._container.gameObject.SetActive(false);

			if (OnDialogClose != null)
			{
				OnDialogClose();
			}

			s_instance._buttonCommands = null;

			s_instance.enabled = false;
		}

		public static void OpenNoOptions (string title, string text)
		{
			s_instance.ShowDialog(title, text, new string[0], new Action[0]);
		}

		public static void OpenOneOption (string title, string text, string option0, Action action0)
		{
			s_instance.ShowDialog(title, text, new string[] { option0 }, new Action[] { action0 });
		}

		public static void OpenTwoOptions (string title, string text, string option0, Action action0, string option1, Action action1)
		{
			s_instance.ShowDialog(title, text, new string[] { option0, option1 }, new Action[] { action0, action1 });
		}

		public static void SetCommandForOneOption (PAction command)
		{
			s_instance._buttonCommands = new PAction[]
			{
				command
			};
		}

		public static void SetCommandsForTwoOptions (PAction command0, PAction command1)
		{
			s_instance._buttonCommands = new PAction[]
			{
				command0, command1
			};
		}

		public void ClickButton (int id)
		{
			// Close the dialog first in case an option opens another dialog!
			Close();

			if (_actions[id] != null)
			{
				_actions[id]();
			}
			else
			{
				Debug.LogError("Option " + id + " is null!");
			}
		}

		private void ShowDialog (string title, string text, string[] options, Action[] actions)
		{
			Close();

			_title.text = title;
			_text.text = text;

			_actions = new Action[actions.Length];
			Array.Copy(actions, _actions, actions.Length);

			for (int i = 0; i < _buttons.Length; i++)
			{
				_buttons[i].gameObject.SetActive(options.Length > i);

				if (i < options.Length)
				{
					_options[i].text = options[i];
				}
			}

			// TODO: Smarter support for 3 or more buttons
			// TODO: Handle vertical alignment

			// Center button 0 if it's the only option
			if (options.Length == 1)
			{
				Vector3 position = _baseButtonPositions[0];
				position.x = 0;
				_buttons[0].GetComponent<RectTransform>().anchoredPosition = position;
			}
			// Otherwise, if two buttons are visible, reset their positions
			else if (options.Length == 2)
			{
				for (int i = 0; i < _buttons.Length; i++)
				{
					_buttons[i].GetComponent<RectTransform>().anchoredPosition = _baseButtonPositions[i];
				}
			}

			_container.gameObject.SetActive(true);

			if (OnDialogOpen != null)
			{
				OnDialogOpen();
			}

			enabled = true;
		}

		protected void Awake ()
		{
			if (s_instance)
			{
				Debug.LogError("Destroying duplicate PopupDialog on " + gameObject.name);
				Destroy(this);
				return;
			}

			s_instance = this;

			_baseButtonPositions = new Vector3[_buttons.Length];

			for (int i = 0; i < _buttons.Length; i++)
			{
				_baseButtonPositions[i] = _buttons[i].GetComponent<RectTransform>().anchoredPosition;
			}

			Close();
		}

		protected void Update ()
		{
			if (_buttonCommands != null)
			{
				for (int i = 0; i < _buttonCommands.Length; i++)
				{
					if (PInput.GetInputDown(_buttonCommands[i], ignoreMouseButtons: true))
					{
						ClickButton(i);
						break;
					}
				}
			}
		}
	}
}

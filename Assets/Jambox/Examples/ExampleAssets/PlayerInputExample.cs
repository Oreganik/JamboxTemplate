// JAMBOX
// General purpose game code for Unity
// Copyright (c) 2020 Ted Brown

using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Jambox.Examples
{
	public class PlayerInputExample : MonoBehaviour 
	{
		private const int MAX_HISTORY_COUNT = 30;

		public float _moveSpeed = 10;
		public float _lookSpeed = 360;
		public Text _device;
		public Text _history;

		private bool _reachedMaxHistory;
		private List<string> _historyStrings;

		protected void Awake ()
		{
			_historyStrings = new List<string>();
		}

		protected void Update ()
		{
			Vector2 look = PInput.GetLook();
			transform.Rotate(Vector3.up * look.x * _lookSpeed * Time.deltaTime, Space.World);

			Vector2 move = PInput.GetMove();
			Vector3 offset = new Vector3(move.x, 0, move.y) * _moveSpeed * Time.deltaTime;
			transform.position += transform.rotation * offset;

			_device.text = PInput.IsUsingGamepad ? "Using Gamepad" : "Using Keyboard & Mouse";

			bool inputThisFrame = false;

			for (int i = 0; i < System.Enum.GetValues(typeof(PAction)).Length; i++)
			{
				if (PInput.GetInputDown((PAction) i))
				{
					if (_reachedMaxHistory)
					{
						_historyStrings.RemoveAt(0);
					}

					if (inputThisFrame)
					{
						_historyStrings.Add(" (+) " + ((PAction) i).ToString() + "\n");
					}
					else
					{
						_historyStrings.Add(Time.frameCount + " " + ((PAction) i).ToString() + "\n");
					}

					inputThisFrame = true;

					if (_reachedMaxHistory == false)
					{
						_reachedMaxHistory = _historyStrings.Count >= MAX_HISTORY_COUNT;
					}
				}
			}

			if (inputThisFrame)
			{
				StringBuilder historyText = new StringBuilder();
				foreach (string str in _historyStrings)
				{
					historyText.Append(str);
				}
				_history.text = historyText.ToString();
			}
		}
	}
}

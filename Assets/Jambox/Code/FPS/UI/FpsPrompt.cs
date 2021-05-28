// JAMBOX
// General purpose game code for Unity
// Copyright (c) 2021 Ted Brown

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Jambox
{
	/// <summary>
	/// 
	/// </summary>
	public class FpsPrompt : MonoBehaviour 
	{
		public TMP_Text _input;
		public TMP_Text _text;

		public void Hide ()
		{
			gameObject.SetActive(false);
		}

		public void Show (KeyCode key, string text)
		{
			_input.text = key.ToString().ToUpperInvariant();
			_text.text = text;
			gameObject.SetActive(true);
		}
	}
}

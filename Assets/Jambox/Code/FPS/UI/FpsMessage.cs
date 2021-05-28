// JAMBOX
// General purpose game code for Unity
// Copyright (c) 2021 Ted Brown

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Jambox
{
	/// <summary>
	/// 
	/// </summary>
	public class FpsMessage : MonoBehaviour 
	{
		public TMP_Text _text;

		public void Hide ()
		{
			gameObject.SetActive(false);
		}

		public void Show (string text)
		{
			gameObject.SetActive(true);
			_text.text = text;
		}
	}
}

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
	public class FpsHudValue : MonoBehaviour 
	{
		public TMP_Text _title;
		public TMP_Text _value;
		public string _stat;

		private int _currentValue;

		public void SetTitle (string value)
		{
			_title.text = value;
		}

		public void SetValue (int value)
		{
			_currentValue = value;
			_value.text = value.ToString();
		}

		protected void Update ()
		{
			int newValue = FpsHero.Instance.Stats.Get(_stat);
			if (newValue != _currentValue)
			{
				SetValue(newValue);
			}
		}
	}
}

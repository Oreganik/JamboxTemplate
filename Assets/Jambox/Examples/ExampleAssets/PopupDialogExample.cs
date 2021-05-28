// JAMBOX
// General purpose game code for Unity
// Copyright (c) 2020 Ted Brown

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jambox.Examples
{
	public class PopupDialogExample : MonoBehaviour 
	{
		private Color _baseColor;
		
		private void SetBackgroundColor (Color color)
		{
			Camera.main.backgroundColor = color;	
		}

		private void OpenNoOptions ()
		{
			SetBackgroundColor(_baseColor);
			PopupDialog.OpenNoOptions("Color Reset", "Back to normal for you!");
		}

		private void OpenOneOption ()
		{
			PopupDialog.OpenOneOption("Color Change", "All you can do is click Red. Or press Return. Or, I guess you could close the dialog...",
				"Red", () => { SetBackgroundColor(Color.red); }
			);
			PopupDialog.SetCommandForOneOption(PAction.Submit);
		}

		private void OpenTwoOptions ()
		{
			PopupDialog.OpenTwoOptions("Color Switch", "You have some real choices here!",
				"Cyan", () => { SetBackgroundColor(Color.cyan); },
				"Green", () => { SetBackgroundColor(Color.green); }
			);
		}

		protected void Awake ()
		{
			_baseColor = Camera.main.backgroundColor;
		}

		protected void Update ()
		{
			if (Input.GetKeyDown(KeyCode.C)) PopupDialog.Close();
			if (Input.GetKeyDown(KeyCode.Alpha0)) OpenNoOptions();
			if (Input.GetKeyDown(KeyCode.Alpha1)) OpenOneOption();
			if (Input.GetKeyDown(KeyCode.Alpha2)) OpenTwoOptions();
		}
	}
}

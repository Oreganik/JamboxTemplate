// JAMBOX
// General purpose game code for Unity
// Copyright (c) 2020 Ted Brown

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jambox.Examples
{
	public class ScreenFaderExample : MonoBehaviour 
	{
		public GameObject _instructions;

		private bool _dialogVisible;

		private void FadeIn ()
		{
			StartCoroutine(iFadeIn());
		}

		private IEnumerator iFadeIn ()
		{
			_instructions.SetActive(true);
			ScreenFader.Instance.SetCanvasHidden();
			ScreenFader.Instance.FadeInFromColor(Color.black, 2);
			while (ScreenFader.Instance.IsFading)
			{
				yield return null;
			}
			PopupDialog.OpenOneOption("Screen Fader", "", "Fade Out", () => { FadeOut(); } );
			
			// Let the dialog be accepted by pressing the Primary key (default is Return)
			PopupDialog.SetCommandForOneOption(PAction.Action0);
		}

		private void FadeOut ()
		{
			StartCoroutine(iFadeOut());
		}

		private IEnumerator iFadeOut ()
		{
			ScreenFader.Instance.SetCanvasHidden();
			ScreenFader.Instance.FadeOutToColor(Color.black, 2);
			while (ScreenFader.Instance.IsFading)
			{
				yield return null;
			}
			_instructions.SetActive(false);
			ScreenFader.Instance.SetCanvasVisible();
			PopupDialog.OpenOneOption("Screen Fader", "", "Fade In", () => { FadeIn(); } );

			// Let the dialog be accepted by pressing the Primary key (default is Return)
			PopupDialog.SetCommandForOneOption(PAction.Action0);
		}

		protected void Start ()
		{
			FadeIn();
		}		
	}
}

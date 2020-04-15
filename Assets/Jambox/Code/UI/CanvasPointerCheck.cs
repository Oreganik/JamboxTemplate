// JAMBOX
// General purpose game code for Unity
// Source: https://answers.unity.com/questions/1539189/check-if-mouse-is-over-ui.html

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Jambox
{
	[RequireComponent(typeof(EventTrigger))]
	public class CanvasPointerCheck : MonoBehaviour
	{
		public static bool IsOverCanvas;

		private void HandleHoverCanvas()
		{
			IsOverCanvas = true;
		}
		private void HandleUnhoverCanvas()
		{
			IsOverCanvas = false;
		}

		protected void Start()
		{
			EventTrigger eventTrigger = GetComponent<EventTrigger>();

			if (eventTrigger != null)
			{
				EventTrigger.Entry enterUIEntry = new EventTrigger.Entry();
				// Pointer Enter
				enterUIEntry.eventID = EventTriggerType.PointerEnter;
				enterUIEntry.callback.AddListener((eventData) => { HandleHoverCanvas(); });
				eventTrigger.triggers.Add(enterUIEntry);

				//Pointer Exit
				EventTrigger.Entry exitUIEntry = new EventTrigger.Entry();
				exitUIEntry.eventID = EventTriggerType.PointerExit;
				exitUIEntry.callback.AddListener((eventData) => { HandleUnhoverCanvas(); });
				eventTrigger.triggers.Add(exitUIEntry);
			}
		}
	}
}

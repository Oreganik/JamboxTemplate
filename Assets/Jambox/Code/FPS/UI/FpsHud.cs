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
	public class FpsHud : MonoBehaviour 
	{
		public static FpsMessage Message { get; private set; }
		public static FpsNotification Notification { get; private set; }
		public static FpsPrompt Prompt { get; private set; }

		protected void Awake ()
		{
			Message = GetComponentInChildren<FpsMessage>();
			Message.Hide();
			
			Notification = GetComponentInChildren<FpsNotification>();
			Notification.Hide();

			Prompt = GetComponentInChildren<FpsPrompt>();
			Prompt.Hide();
		}
	}
}

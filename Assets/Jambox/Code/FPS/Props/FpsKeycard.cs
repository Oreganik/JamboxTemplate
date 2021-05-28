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
	public class FpsKeycard : MonoBehaviour 
	{
		public string _notification = "Acquired Keycard";
		public string _keycardId;

		private void HandleAcquired ()
		{
			FpsHero.Instance.AddKeycard(_keycardId);
			FpsHud.Notification.Show(_notification);
		}

		protected void Awake ()
		{
			GetComponent<FpsPickup>().OnAcquired += HandleAcquired;
		}
	}
}

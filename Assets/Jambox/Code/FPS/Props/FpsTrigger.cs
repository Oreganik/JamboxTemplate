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
	public class FpsTrigger : MonoBehaviour 
	{
		public Action OnInteract;

		public bool _automaticInteraction;
		public string _requiredKeycardId;
		public string _prompt;
		public string _lockPrompt;

		private bool _hasBeenUsed;
		private LayerMask _heroLayer;

		private void Activate ()
		{
			if (OnInteract != null)
			{
				OnInteract();
			}
			_hasBeenUsed = true;
			FpsHud.Prompt.Hide();
			FpsHud.Message.Hide();
			enabled = false;
		}

		protected void Awake ()
		{
			_heroLayer = LayerMask.NameToLayer("Hero");
			enabled = false;
		}

		protected void OnTriggerEnter (Collider collider)
		{
			if (_hasBeenUsed || collider.gameObject.layer != _heroLayer)
			{
				return;
			}

			if (FpsHero.Instance.HasKeycard(_requiredKeycardId) == false)
			{
				FpsHud.Message.Show(_lockPrompt);
				return;
			}

			if (_automaticInteraction)
			{
				Activate();
			}
			else
			{
				FpsHud.Prompt.Show(FpsControl.GetKeyboardInput(FpsInput.Interact), _prompt);
				enabled = true;
			}
		}

		protected void OnTriggerExit (Collider collider)
		{
			if (collider.gameObject.layer == _heroLayer)
			{
				FpsHud.Message.Hide();
				FpsHud.Prompt.Hide();
				enabled = false;
			}
		}

		protected void Update ()
		{
			if (FpsControl.WasPressedThisFrame(FpsInput.Interact))
			{
				Activate();
			}
		}
	}
}

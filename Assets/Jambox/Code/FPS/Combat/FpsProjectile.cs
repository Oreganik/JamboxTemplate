// GAME JAM PROJECT
// Prototype Application
// Copyright (c) 2020 Ted Brown

using Jambox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jambox
{
	/// <summary>
	/// 
	/// </summary>
	public abstract class FpsProjectile : MonoBehaviour 
	{
		public bool IsReadyToFire { get; private set; }
		public bool IsBusy { get; protected set; }

		protected static RaycastHit s_raycastHit;
		protected static FpsTarget s_target;

		public void Fire (Vector3 position, Quaternion rotation)
		{
			transform.position = position;
			transform.rotation = rotation;
			gameObject.SetActive(true);
			IsReadyToFire = false;
			IsBusy = false;
			HandleFire();
		}

		public virtual void Process (float deltaTime)
		{
			IsBusy = false;
		}

		public void Remove ()
		{
			IsReadyToFire = true;
			gameObject.SetActive(false);
		}

		protected abstract void HandleFire ();

	}
}

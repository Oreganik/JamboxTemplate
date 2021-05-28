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
	public class FpsPickup : MonoBehaviour 
	{
		public Action OnAcquired;

		private bool _isMovingTowardsPlayer;
		private FpsTrigger _trigger;
		private float _speed;
		private float _acceleration = 10;

		private void HandleInteraction ()
		{
			if (_isMovingTowardsPlayer)
			{
				return;
			}

			_trigger.OnInteract -= HandleInteraction;
			foreach (Collider collider in GetComponentsInChildren<Collider>())
			{
				collider.enabled = false;
			}
			_isMovingTowardsPlayer = true;
			enabled = true;
		}

		protected void Awake ()
		{
			_trigger = GetComponentInChildren<FpsTrigger>();
			_trigger.OnInteract += HandleInteraction;
			enabled = false;
		}

		protected void Start ()
		{
			if (_isMovingTowardsPlayer == false)
			{
				enabled = false;
			}
		}

		protected void OnDestroy ()
		{
			if (_trigger)
			{
				_trigger.OnInteract -= HandleInteraction;
			}
		}

		protected void Update ()
		{
			if (_isMovingTowardsPlayer == false) return;
			
			_speed += _acceleration * Time.deltaTime;
			transform.position = Vector3.MoveTowards(transform.position, FpsHero.Instance.transform.position + Vector3.up, _speed * Time.deltaTime);
			if (Vector3.Distance(transform.position, FpsHero.Instance.transform.position + Vector3.up) < 0.1f)
			{
				if (OnAcquired != null)
				{
					OnAcquired();
				}
				Destroy(gameObject);
			}
		}
	}
}

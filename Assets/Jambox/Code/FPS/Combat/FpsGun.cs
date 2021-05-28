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
	public class FpsGun : MonoBehaviour 
	{
		public GameObject _projectilePrefab;
		public float _cooldown;

		private float _lastFiretime;
		private List<FpsProjectile> _projectiles;

		private void Shoot ()
		{
			_lastFiretime = Time.time;

			// Find (or create) a projectile in the pool
			FpsProjectile newProjectile = null;
			foreach (FpsProjectile projectile in _projectiles)
			{
				if (projectile.IsReadyToFire)
				{
					newProjectile = projectile;
					break;
				}
			}

			// Add a new projectile to the pool
			if (newProjectile == null)
			{
				newProjectile = Instantiate(_projectilePrefab).GetComponent<FpsProjectile>();
				_projectiles.Add(newProjectile);
			}

			newProjectile.Fire(transform.position, transform.rotation);
		}

		protected void Awake ()
		{
			_projectiles = new List<FpsProjectile>();
		}

		protected void Update ()
		{
			// Process all projectiles. Return ones that aren't busy to the pool.
			float deltaTime = Time.deltaTime;
			foreach (FpsProjectile projectile in _projectiles)
			{
				if (projectile.IsBusy)
				{
					projectile.Process(deltaTime);
				}
				else
				{
					projectile.Remove();
				}
			}

			if (Time.time > _lastFiretime + _cooldown && FpsControl.IsPressed(FpsInput.Shoot))
			{
				Shoot();
			}
		}
	}
}

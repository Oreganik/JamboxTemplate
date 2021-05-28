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
	public class FpsInstantProjectile : FpsProjectile 
	{
		public int _damage = 1;
		public string _targetStat = "health";
		public GameObject _impactFxPrefab;

		protected override void HandleFire()
		{
			LayerMask ignoreHeroMask = ~(1 << LayerMask.NameToLayer("Hero"));
			if (Physics.Raycast(transform.position, transform.forward, out s_raycastHit, 1000, ignoreHeroMask, QueryTriggerInteraction.Ignore))
			{
				GameObject particles = Instantiate(_impactFxPrefab);
				particles.transform.position = s_raycastHit.point;
				particles.transform.rotation = Quaternion.LookRotation(s_raycastHit.normal, Vector3.up);
				
				s_target = s_raycastHit.collider.gameObject.GetComponentInParent<FpsTarget>();
				if (s_target)
				{
					s_target.Damage(_targetStat, _damage);
				}
			}
		}
	}
}

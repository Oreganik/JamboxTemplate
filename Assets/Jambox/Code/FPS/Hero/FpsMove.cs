// JAMBOX
// General purpose game code for Unity
// Copyright (c) 2020 Ted Brown

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jambox
{
	/// <summary>
	/// 
	/// </summary>
	public class FpsMove : MonoBehaviour 
	{
		public bool UsePhysics = true;

		private float _speed;
		private FpsHero _hero;
		private Rigidbody _rigidbody;
		private Vector3 _velocity;

		public void Process (Transform cameraTransform)
		{
			Vector2 moveInput = FpsControl.GetMove();
			Vector3 forward = cameraTransform.forward;

			// Force walk on level plane
			forward.y = 0;
			forward *= moveInput.y * _speed;

			Vector3 right = cameraTransform.right;
			// Force walk on level plane
			right.y = 0;
			right *= moveInput.x * _speed;

			if (UsePhysics)
			{
				// airborne
				if ((FpsHeroState)_hero.ActiveState.Name == FpsHeroState.Jump)
				{
					Vector3 up = Vector3.up * _rigidbody.velocity.y;
				//	_rigidbody.velocity = right + up + forward;
				}
				else
				{
					_rigidbody.velocity = right + forward;
				}
				_rigidbody.angularVelocity = Vector3.zero;
			}
			else
			{
				transform.position += (forward + right) * Time.deltaTime;
			}
		}

		public void SetSpeed (float value)
		{
			_speed = value;
		}

		protected void Awake ()
		{
			_hero = GetComponent<FpsHero>();
			SetSpeed(0.1f);
			if (UsePhysics)
			{
				_rigidbody = GetComponent<Rigidbody>();
			}
		}
	}
}

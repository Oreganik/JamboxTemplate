// JAMBOX
// General purpose game code for Unity
// Copyright (c) 2020 Ted Brown

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jambox.Examples
{
	public class PostProcessingExample : MonoBehaviour 
	{
		public Transform _spinCube;
		public Vector3 _rotationDirection;
		public float _rotationSpeed = 180;

		protected void Awake ()
		{
			if (_rotationDirection == Vector3.zero)
			{
				_rotationDirection = Random.insideUnitSphere;
			}
		}

		protected void Update ()
		{
			_spinCube.Rotate(_rotationDirection * _rotationSpeed * Time.deltaTime, Space.Self);
		}
	}
}

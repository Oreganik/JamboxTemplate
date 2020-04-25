// JAMBOX
// General purpose game code for Unity
// Copyright (c) 2020 Ted Brown

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jambox
{
	public class FaceCamera : MonoBehaviour 
	{
		public bool _faceAwayFromCamera;

		private Transform _cameraTransform;

		protected void Awake ()
		{
			_cameraTransform = Camera.main.transform;
		}

		protected void Update ()
		{
			Vector3 direction = _faceAwayFromCamera ? 
				(transform.position - _cameraTransform.position).normalized :
				(_cameraTransform.position - transform.position).normalized;

			transform.rotation = Quaternion.LookRotation(direction, _cameraTransform.up);
		}
	}
}

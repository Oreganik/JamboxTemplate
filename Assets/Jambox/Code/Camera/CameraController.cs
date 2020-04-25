// JAMBOX
// General purpose game code for Unity
// Copyright 2020 Ted Brown

using UnityEngine;
using System.Collections;

namespace Jambox
{
	// Does not actually move camera by default. Add inputs to this script or another.
	// TODO: possibly collect all inputs and then use them in proper order
	// TODO: screen shake
	public class CameraController : MonoBehaviour 
	{
		public static CameraController Instance 
		{ 
			get; private set; 
		}

		[Header("ORBIT")]
		public bool invertOrbit;
		public float orbitSpeed = 120.0f;

		[Header("PAN")]
		public float panSpeed = 100;

		[Header("PITCH")]
		public bool invertPitch = true;
		public float pitchSpeed = 120.0f;
		public float pitchMin = -20f;
		public float pitchMax = 80f;
		public float startPitch = 45;
	
		[Header("ZOOM")]
		public float zoomSpeed = 50;
		public float zoomMin = .5f;
		public float zoomMax = 15f;
		public float startZoom = 8;
	
		private float _zoomDistance;
		private Vector3 _eulerRotation;
		private Vector3 _position;
	
		public void Orbit (float input)
		{
			input *= orbitSpeed * Time.deltaTime;
			if (invertOrbit) input *= -1;
			_eulerRotation.y += input;
			_eulerRotation.y = ClampAngle(_eulerRotation.y, -360, 360);
		}

		public void Pan (float inputX, float inputY)
		{
			// oooooooh this is a terrible hack
			Vector3 offset = Quaternion.Euler(_eulerRotation) * new Vector3(inputX, inputY, inputY);
			offset.y = 0;
			offset *= panSpeed * Time.deltaTime;
			_position += offset * Time.deltaTime;
		}

		public void Pitch (float input)
		{
			input *= pitchSpeed * Time.deltaTime;
			if (invertPitch) input *= -1;
			_eulerRotation.x += input; // interesting: tune with distance
			_eulerRotation.x = ClampAngle(_eulerRotation.x, pitchMin, pitchMax);
		}

		public void SetPosition (Vector3 position)
		{
			_position = position;
		}

		public void Zoom (float input)
		{
			input *= Time.deltaTime * zoomSpeed * -1;
			_zoomDistance = Mathf.Clamp(_zoomDistance + input, zoomMin, zoomMax);
		}

		private void Awake () 
		{
			Instance = this;
			_eulerRotation = new Vector3(startPitch, 0, 0);
			_eulerRotation.x = ClampAngle(_eulerRotation.x, pitchMin, pitchMax);
			_zoomDistance = Mathf.Clamp(startZoom, zoomMin, zoomMax);
		}
	
		private void LateUpdate ()
		{
			Quaternion rotation = Quaternion.Euler(_eulerRotation);
			Vector3 negDistance = new Vector3(0.0f, 0.0f, -_zoomDistance);
			transform.rotation = rotation;
			transform.position = rotation * negDistance + _position;;
		}
	
		private static float ClampAngle(float angle, float min, float max)
		{
			while (angle < -360F) angle += 360F;
			while (angle > 360F) angle -= 360F;
			return Mathf.Clamp(angle, min, max);
		}
	}
}

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
	public class FpsLook : MonoBehaviour 
	{
		public const float BASE_HEIGHT = 1.8f;

		public Camera WorldCamera;
		public float _lookSpeedX = 360;
		public float _lookSpeedY = 180;

		// There is a huge jump on OSX the first time the mouse is moved after the cursor is locked.
		// So we wait a few frames before using mouse input.
		// https://issuetracker.unity3d.com/issues/input-dot-getaxis-mouse-first-given-value-is-wrong-after-cursor-dot-lockstate-is-set-to-cursorlockmode-dot-locked
		private int _fixedInitialInputError; 

		public void Process ()
		{
			// Look ------------
			Vector2 lookInput = FpsControl.GetLook();
			
			if (_fixedInitialInputError < 10)
			{
				// skip the first frame of input to avoid a bug that causes things to jump
				if (lookInput.magnitude > 0)
				{
					_fixedInitialInputError++;
				}
			}
			else
			{
				transform.Rotate(Vector3.up * lookInput.x * _lookSpeedX * Time.deltaTime, Space.World);
				WorldCamera.transform.Rotate(Vector3.right * lookInput.y * _lookSpeedY * -1 * Time.deltaTime, Space.Self);
			}

			// Overwrite roll accumulated from rotation with user-defined roll
			Vector3 euler = WorldCamera.transform.rotation.eulerAngles;
			euler.z = 0;
			WorldCamera.transform.rotation = Quaternion.Euler(euler);
		}

		public void LerpToHeight (float value, float t)
		{
			WorldCamera.transform.localPosition = Vector3.up * (Mathf.Lerp(WorldCamera.transform.localPosition.y, value, t));
		}

		public void SetHeight (float value)
		{
			WorldCamera.transform.localPosition = Vector3.up * value;
		}

		protected void Awake ()
		{
			WorldCamera.transform.parent = transform;
			WorldCamera.transform.localPosition = Vector3.up * BASE_HEIGHT;
			WorldCamera.transform.localRotation = Quaternion.identity;
		}
	}
}

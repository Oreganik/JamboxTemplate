// JAMBOX
// General purpose game code for Unity
// Copyright 2020 Ted Brown

using UnityEngine;

namespace Jambox
{
	[DefaultExecutionOrder(100)] // go late in the execution order so it layers on top of camera movement
	public class CameraShake : MonoBehaviour 
	{
		static bool s_usePosX;
		static bool s_usePosY;
		static bool s_usePosZ;
		static bool s_useRotX;
		static bool s_useRotY;
		static bool s_useRotZ;

		private static CameraShake s_instance;

		// magnitude is the total possible offset or rotation, from negative limit to positive limit
		static float s_positionMagnitude;
		static float s_rotationMagnitude;
		static Timer s_timer;

		public static void Shake (float positionMagnitude, float rotationMagnitude, float duration, bool isEpic = false)
		{
			if (duration <= 0) return;
			s_positionMagnitude = positionMagnitude;
			s_rotationMagnitude = rotationMagnitude;
			s_timer = new Timer(duration);
			s_usePosZ = isEpic;
		}

		public static void Stop ()
		{
			s_timer.FinishNow();
		}

		public static void BigShake ()
		{
			Shake(1, 5, 1, isEpic: true);
		}

		public static void LittleShake ()
		{
			Shake(0.2f, 2f, 0.5f, isEpic: false);
		}

		protected void Awake ()
		{
			if (s_instance)
			{
				Debug.LogError("yo! you got too much camera shake");
			}

			s_instance = this;

			s_usePosX = true;
			s_usePosY = true;
			s_usePosZ = false; // epic shakes will make this true
			s_useRotX = false; // rotating x and y axis is weird
			s_useRotY = false;
			s_useRotZ = true;
		}

		protected void LateUpdate ()
		{
			s_timer.Update(Time.deltaTime);
			float t = s_timer.t;

			float posX = s_usePosX ? Random.Range(-s_positionMagnitude, s_positionMagnitude) * 0.5f * (1 - t) : 0;
			float posY = s_usePosY ? Random.Range(-s_positionMagnitude, s_positionMagnitude) * 0.5f * (1 - t) : 0;
			float posZ = s_usePosZ ? Random.Range(-s_positionMagnitude, s_positionMagnitude) * 0.5f * (1 - t) : 0;
			transform.localPosition += new Vector3(posX, posY, posZ);

			float rotX = s_useRotX ? Random.Range(-s_rotationMagnitude, s_rotationMagnitude) * 0.5f * (1 - t) : 0;
			float rotY = s_useRotY ? Random.Range(-s_rotationMagnitude, s_rotationMagnitude) * 0.5f * (1 - t) : 0;
			float rotZ = s_useRotZ ? Random.Range(-s_rotationMagnitude, s_rotationMagnitude) * 0.5f * (1 - t) : 0;
			transform.localRotation *= Quaternion.Euler(new Vector3(rotX, rotY, rotZ));
		}
	}
}

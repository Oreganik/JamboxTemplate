using UnityEngine;

namespace Jambox
{
	public class CameraShake : MonoBehaviour 
	{
		public bool usePosX;
		public bool usePosY;
		public bool usePosZ;
		public bool useRotX;
		public bool useRotY;
		public bool useRotZ;

		float timer;
		float duration;
		// magnitude is the total possible offset or rotation, from negative limit to positive limit
		float positionMagnitude;
		float rotationMagnitude;

		public void BigShake ()
		{
			usePosX = true;
			usePosY = true;
			usePosZ = true;
			useRotX = false;
			useRotY = false;
			useRotZ = true;
			StartShake(1, 5, 1);
		}

		public void LittleShake ()
		{
			usePosX = true;
			usePosY = true;
			usePosZ = false;
			useRotX = false;
			useRotY = false;
			useRotZ = true;
			StartShake(0.2f, 2f, 0.5f);
		}

		// Call this every frame on the camera object
		public void ShakeTransform (Transform targetTransform)
		{
			if (duration <= 0) return;
			timer = Mathf.Clamp(timer + Time.deltaTime, 0, duration);
			float t = timer / duration;
			if (t >= 1) return;

			float posX = usePosX ? Random.Range(-positionMagnitude, positionMagnitude) * 0.5f * (1 - t) : 0;
			float posY = usePosY ? Random.Range(-positionMagnitude, positionMagnitude) * 0.5f * (1 - t) : 0;
			float posZ = usePosZ ? Random.Range(-positionMagnitude, positionMagnitude) * 0.5f * (1 - t) : 0;
			targetTransform.localPosition += new Vector3(posX, posY, posZ);

			float rotX = useRotX ? Random.Range(-rotationMagnitude, rotationMagnitude) * 0.5f * (1 - t) : 0;
			float rotY = useRotY ? Random.Range(-rotationMagnitude, rotationMagnitude) * 0.5f * (1 - t) : 0;
			float rotZ = useRotZ ? Random.Range(-rotationMagnitude, rotationMagnitude) * 0.5f * (1 - t) : 0;
			targetTransform.localRotation *= Quaternion.Euler(new Vector3(rotX, rotY, rotZ));
		}

		public void StartShake (float positionMagnitude, float rotationMagnitude, float duration)
		{
			this.positionMagnitude = positionMagnitude;
			this.rotationMagnitude = rotationMagnitude;
			this.duration = duration;
			timer = 0;
		}

		public void StopShake ()
		{
			timer = duration;
		}
	}
}

using Jambox;
using UnityEngine;

namespace Jambox.Examples 
{
	public class CameraShakeExample : MonoBehaviour 
	{

		CameraShake cameraShake;
		Transform cameraTransform;
		Vector3 basePosition;
		Quaternion baseRotation;

		void Start ()
		{
			cameraShake = GetComponent<CameraShake>();

			if (cameraShake == null)
			{
				Debug.LogError("Missing CameraShake component");
				Destroy(this);
			}

			cameraTransform = Camera.main.transform;
			basePosition = cameraTransform.localPosition;
			baseRotation = cameraTransform.localRotation;
		}

		void Update ()
		{
			if (Input.GetKeyDown(KeyCode.Return))
			{
				cameraShake.LittleShake();
			}
			if (Input.GetKeyDown(KeyCode.Space))
			{
				cameraShake.BigShake();
			}
		}

		void LateUpdate () 
		{
			cameraTransform.localPosition = basePosition;
			cameraTransform.localRotation = baseRotation;
			cameraShake.ShakeTransform(cameraTransform);
		}
	}
}

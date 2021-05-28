using Jambox;
using UnityEngine;

namespace Jambox.Examples 
{
	public class CameraShakeExample : MonoBehaviour 
	{
		private Transform _cameraTransform;
		private Quaternion _baseRotation;
		private Vector3 _basePosition;

		protected void Awake ()
		{
			_cameraTransform = Camera.main.transform;
			_basePosition = _cameraTransform.position;
			_baseRotation = _cameraTransform.rotation;
		}

		void Update ()
		{
			if (Input.GetKeyDown(KeyCode.Return))
			{
				CameraShake.LittleShake();
			}
			else if (Input.GetKeyDown(KeyCode.Space))
			{
				CameraShake.BigShake();
			}
			else
			{
				// CameraShake adds to the current pose of the camera.
				// Therefore, we have to maintain its "base pose" for the example.
				_cameraTransform.position = _basePosition;
				_cameraTransform.rotation = _baseRotation;
			}
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TacCam
// Used to move around a tactics or strategy game playfield

// Controls:
// - RMB to rotate around pivot
// - MMB to pan within bounds
// - MouseWheel to zoom and adjust pitch

// Hierarchy:
// - Pivot is this object's transform. It rotates on Y.
// - Post controls Y height from the Pivot and rotates on X.
// - Boom controls distance from Post on local Z.

namespace Tac 
{
	public class TacCam : MonoBehaviour 
	{

		[Tooltip("Measured from origin. Half total length/width.")]
		public Vector2 bounds;

		[Header("CURRENT VALUES")]
		public float postHeight = 2;
		public float boomDistance = 2;
		public float boomAngle = 30;

		[Header("CONFIGURATION")]
		public float minBoomDistance = 2;
		public float maxBoomDistance = 10;
		public float dragSensitivity = 0.1f;

		private bool isDragActive;
		private bool isYawActive;

		private Transform pivot;
		private Transform post;
		private Transform boom;

		private Vector3 deltaMouse;
		private Vector3 previousMousePosition;

		public void ReleaseMainCamera ()
		{
			Camera.main.transform.parent = null;
		}

		public void TakeMainCamera (float duration = 0)
		{
			Camera.main.transform.parent = boom;
			Camera.main.transform.localPosition = Vector3.zero;
			Camera.main.transform.localRotation = Quaternion.identity;
		}

		private void Awake ()
		{
			pivot = transform;

			GameObject postObject = new GameObject("Post");
			post = postObject.transform;
			post.parent = pivot;
			post.localPosition = Vector3.up * postHeight;
			post.localRotation = Quaternion.Euler(new Vector3(boomAngle, 0, 0));

			GameObject boomObject = new GameObject("Boom");
			boom = boomObject.transform;
			boom.parent = post;
			boom.localPosition = Vector3.back * boomDistance;
			boom.localRotation = Quaternion.identity;
		}

		private void Start ()
		{
			TakeMainCamera();
		}

		private void Update ()
		{
			boomDistance = Mathf.Clamp(boomDistance - Input.GetAxis("Mouse ScrollWheel"), minBoomDistance, maxBoomDistance);
			boom.localPosition = Vector3.back * boomDistance;

			isYawActive = Input.GetKey(KeyCode.Space);

			if (isYawActive == false)
			{
				isDragActive = Input.GetKey(KeyCode.Q);
			}
		}

		private void LateUpdate ()
		{
			deltaMouse = Input.mousePosition - previousMousePosition;

			if (isYawActive)
			{
				pivot.Rotate(new Vector3(0, deltaMouse.x, 0), Space.World);
			}
			else if (isDragActive)
			{
				Vector3 dragOffset = new Vector3(deltaMouse.x, 0, deltaMouse.y) * dragSensitivity;
				dragOffset = pivot.rotation * dragOffset;
				pivot.transform.localPosition += dragOffset;
				pivot.transform.localPosition = new Vector3(
					Mathf.Clamp(pivot.transform.localPosition.x + dragOffset.x, -bounds.x, bounds.x),
					pivot.transform.localPosition.y,
					Mathf.Clamp(pivot.transform.localPosition.z + dragOffset.z, -bounds.y, bounds.y));
			}
			previousMousePosition = Input.mousePosition;
		}

	}
}

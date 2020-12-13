using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jambox
{
	/// <summary>
	/// Used by ContentMover
	/// </summary>
	public class ObjectMoveData 
	{
		public Action OnComplete;

		public bool IsComplete
		{
			get { return timer.IsComplete; }
		}
		
		public Transform transform { get; private set; }
		public Vector3 startPosition { get; private set; }
		public Vector3 endPosition { get; private set; }
		public Vector3 startScale { get; private set; }
		public Vector3 endScale { get; private set; }
		public Quaternion startRotation { get; private set; }
		public Quaternion endRotation { get; private set; }
		public bool rotate { get; private set; }
		public bool scale { get; private set; }
		public bool translate { get; private set; }
		public Timer timer { get; private set; }
		
		private bool _rotateWorldSpace;
		private bool _translateWorldSpace;

		private const float HALF_PI = Mathf.PI / 2;

		public ObjectMoveData (Transform targetTransform, float duration)
		{
			transform = targetTransform;
			timer = new Timer(duration);
		}

		public void FinishNow ()
		{
			timer.FinishNow();
			Process(1); // send a dummy time: the timer is finished so we'll hit max lerp no matter what

			if (OnComplete != null)
			{
				OnComplete();
			}

			// Clear these functions so they won't be called twice
			OnComplete = null;
		}

		public void Rotate (Quaternion start, Quaternion end, bool inWorldSpace = true)
		{
			startRotation = start;
			endRotation = end;
			_rotateWorldSpace = inWorldSpace;

			if (inWorldSpace)
			{
				transform.rotation = start;
			}
			else
			{
				transform.localRotation = start;
			}

			rotate = true;
		}

		public void Scale (Vector3 start, Vector3 end)
		{
			startScale = start;
			endScale = end;
			transform.localScale = start;
			scale = true;
		}

		public void Translate (Vector3 start, Vector3 end, bool inWorldSpace = true)
		{
			startPosition = start;
			endPosition = end;
			_translateWorldSpace = inWorldSpace;

			if (inWorldSpace)
			{
				transform.position = start;
			}
			else
			{
				transform.localPosition = start;
			}

			translate = true;
		}

		public void Process (float deltaTime)
		{
			timer.Update(deltaTime);

			// really rough approximation of a limit using a sin function
			float t = Mathf.Sin(timer.t * HALF_PI);

			if (rotate)
			{
				if (_rotateWorldSpace)
				{
					transform.rotation = Quaternion.Lerp(startRotation, endRotation, t);
				}
				else
				{
					transform.localRotation = Quaternion.Lerp(startRotation, endRotation, t);
				}
			}

			if (scale)
			{
				transform.localScale = Vector3.Lerp(startScale, endScale, t);
			}

			if (translate)
			{
				if (_translateWorldSpace)
				{
					transform.position = Vector3.Lerp(startPosition, endPosition, t);
				}
				else
				{
					transform.localPosition = Vector3.Lerp(startPosition, endPosition, t);
				}
			}
		}
	}
}

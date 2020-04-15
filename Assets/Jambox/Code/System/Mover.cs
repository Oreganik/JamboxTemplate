///////////////////////////////////////////////////////////////////////////////
//
//	Mover.cs
//	Part of the JamBox Plugin for Unity
//
//	Copyright (c) 2015, Oreganik LLC
//	All rights reserved.
//
///////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

namespace Jambox
{
	public class Mover : MonoBehaviour 
	{
		public bool isMoving { get { return this.enabled; } }

		public void MoveToPosition (Vector3 targetPosition, float duration)
		{
			if (duration <= 0)
			{
				transform.position = targetPosition;
				this.enabled = false;
			}
			else
			{
				beginPosition = transform.position;
				endPosition = targetPosition;
				deltaPosition = endPosition - beginPosition;
				moveTimer = 0;
				moveDuration = duration;
				this.enabled = true;
			}
		}

		float moveDuration;
		float moveTimer;

		Vector3 beginPosition;
		Vector3 deltaPosition;
		Vector3 endPosition;

		void Awake ()
		{
			this.enabled = false;
		}

		void Update () 
		{
			if (App.Instance) moveTimer = Mathf.Clamp(moveTimer + App.deltaGameTime, 0, moveDuration);
			else moveTimer = Mathf.Clamp(moveTimer + Time.deltaTime, 0, moveDuration);
			float percentComplete = moveTimer / moveDuration;
			transform.position = beginPosition + deltaPosition * percentComplete;
			if (percentComplete >= 1) this.enabled = false;
		}
	}
}

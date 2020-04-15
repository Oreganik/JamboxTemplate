// JAMBOX
// General purpose game code for Unity
// Author: Ted Brown
// Contains code from http://catlikecoding.com/unity/tutorials/curves-and-splines/

using UnityEngine;

namespace Jambox
{
	// Parent class of components used by PathFollower.
	public abstract class Path : MonoBehaviour
	{
		public float pathDistance { get; protected set; }

		public abstract float GetDistance();

		public abstract Vector3 GetPoint(float t);

		public abstract Vector3 GetPointByLinearDistanceTraveled(float distance);

		public abstract Vector3 GetVelocity(float t);

		public abstract Vector3 GetVelocityByLinearDistanceTraveled(float distance);

		public abstract Vector3 GetDirection(float t);

		public abstract Vector3 GetDirectionByLinearDistanceTraveled(float distance);

		public Vector3 GetStartPoint()
		{
			return GetPoint(0);
		}

		public Vector3 GetStartVelocity()
		{
			return GetVelocity(0);
		}

		public Vector3 GetStartDirection()
		{
			return GetDirection(0);
		}

		public Vector3 GetEndPoint()
		{
			return GetPoint(1);
		}

		public Vector3 GetEndVelocity()
		{
			return GetVelocity(1);
		}

		public Vector3 GetEndDirection()
		{
			return GetDirection(1);
		}
	}
}

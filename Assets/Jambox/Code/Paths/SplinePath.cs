// JAMBOX
// General purpose game code for Unity
// Author: Ted Brown
// Contains code from http://catlikecoding.com/unity/tutorials/curves-and-splines/

using UnityEngine;
using System;

namespace Jambox
{
	/// A path tool which uses quadratic equations to build paths from segments of 4 points.
	public class SplinePath : Path
	{
		[SerializeField]
		public bool linkMidPoint;

		public int ControlPointCount
		{
			get
			{
				return points.Length;
			}
		}

		public void AddCurve()
		{
			Vector3 point = points[points.Length - 1];
			Array.Resize(ref points, points.Length + 3);
			point.x += 1f;
			points[points.Length - 3] = point;
			point.x += 1f;
			points[points.Length - 2] = point;
			point.x += 1f;
			points[points.Length - 1] = point;

			Array.Resize(ref modes, modes.Length + 1);
			modes[modes.Length - 1] = modes[modes.Length - 2];
			EnforceMode(points.Length - 4);
		}

		// To cover the entire spline with a t going from zero to one, we first need to figure out which curve we're on. 
		// We can get the curve's index by multiplying t by the number of curves and then discarding the fraction/
		public int CurveCount
		{
			get
			{
				return (points.Length - 1) / 3;
			}
		}

		// Enforce constraints on BezierControlPointMode
		private void EnforceMode(int index)
		{
			int modeIndex = (index + 1) / 3;
			//  When the mode is set to free, or when we're at the end points of the curve, we can return without doing anything.
			BezierControlPointMode mode = modes[modeIndex];
			if (mode == BezierControlPointMode.Free || modeIndex == 0 || modeIndex == modes.Length - 1)
			{
				return;
			}

			int middleIndex = modeIndex * 3;
			int fixedIndex, enforcedIndex;
			if (index <= middleIndex)
			{
				fixedIndex = middleIndex - 1;
				enforcedIndex = middleIndex + 1;
			}
			else
			{
				fixedIndex = middleIndex + 1;
				enforcedIndex = middleIndex - 1;
			}

			Vector3 middle = points[middleIndex];
			Vector3 enforcedTangent = middle - points[fixedIndex];
			if (mode == BezierControlPointMode.Aligned)
			{
				enforcedTangent = enforcedTangent.normalized * Vector3.Distance(middle, points[enforcedIndex]);
			}
			points[enforcedIndex] = middle + enforcedTangent;
		}

		public Vector3 GetControlPoint(int index)
		{
			return points[index];
		}

		public override float GetDistance()
		{
			return pathDistance;
		}

		// We can reduce t to just the fractional part to get the interpolation value for our curve. 
		// To get to the actual points, we have to multiply the curve index by three.
		// However, this would fail when then original t equals one.In this case we can just set it to the last curve.
		public override Vector3 GetPoint(float t)
		{
			int i;
			if (t >= 1f)
			{
				t = 1f;
				i = points.Length - 4;
			}
			else {
				t = Mathf.Clamp01(t) * CurveCount;
				i = (int)t;
				t -= i;
				i *= 3;
			}
			return transform.TransformPoint(Bezier.GetPoint(
				points[i], points[i + 1], points[i + 2], points[i + 3], t));
		}

		// Given an object that travels at a linear (or even variable) rate and tracks its own distance traveled,
		// where on the spline will it fall when it has traveled a specified distance?
		public override Vector3 GetPointByLinearDistanceTraveled(float distance)
		{
			if(distance <= 0) return GetStartPoint();
			if(distance >= pathDistance) return GetEndPoint();

			int t;
			for (t = 0; t < distanceAtTime.Length; t++)
			{
				if (distanceAtTime[t] > distance)
				{
					break;
				}
			}

			return GetPoint(t / (float)distanceAtTime.Length);
		}

		public override Vector3 GetVelocity(float t)
		{
			int i;
			if (t >= 1f)
			{
				t = 1f;
				i = points.Length - 4;
			}
			else {
				t = Mathf.Clamp01(t) * CurveCount;
				i = (int)t;
				t -= i;
				i *= 3;
			}
			return transform.TransformPoint(Bezier.GetFirstDerivative(
				points[i], points[i + 1], points[i + 2], points[i + 3], t)) - transform.position;
		}

		// Given an object that travels at a linear (or even variable) rate and tracks its own distance traveled,
		// where on the spline will it fall when it has traveled a specified distance?
		public override Vector3 GetVelocityByLinearDistanceTraveled(float distance)
		{
			if(distance <= 0) return GetStartVelocity();
			if(distance >= pathDistance) return GetEndVelocity();

			int t;
			for(t = 0; t < distanceAtTime.Length; t++)
			{
				if(distanceAtTime[t] > distance)
				{
					break;
				}
			}

			return GetVelocity(t / (float)distanceAtTime.Length);
		}

		public BezierControlPointMode GetControlPointMode(int index)
		{
			return modes[(index + 1) / 3];
		}

		public override Vector3 GetDirection(float t)
		{
			return GetVelocity(t).normalized;
		}

		// Given an object that travels at a linear (or even variable) rate and tracks its own distance traveled,
		// where on the spline will it fall when it has traveled a specified distance?
		public override Vector3 GetDirectionByLinearDistanceTraveled(float distance)
		{
			return GetVelocityByLinearDistanceTraveled(distance).normalized;
		}

		void OnDrawGizmos()
		{
			Gizmos.color = Color.yellow;
			for (int i = 0; i < ControlPointCount - 1; i++)
			{
				Gizmos.DrawLine(transform.TransformPoint(points[i]), transform.TransformPoint(points[i + 1]));
			}
		}

		public void RemoveCurve()
		{
			if (points.Length > 4)
			{
				Array.Resize(ref points, points.Length - 3);
				Array.Resize(ref modes, modes.Length - 1);
			}
		}

		public void Reset()
		{
			points = new Vector3[] {
				new Vector3(0f, 0f, 0f),
				new Vector3(1f, 0f, 0f),
				new Vector3(2f, 0f, 0f),
				new Vector3(3f, 0f, 0f)
			};

			modes = new BezierControlPointMode[] {
				BezierControlPointMode.Free,
				BezierControlPointMode.Free
			};
		}

		public void SetControlPoint(int index, Vector3 point)
		{
			// [Tooltip("If true, mid points of curves will equally affect connected points.")]
			if (linkMidPoint)
			{
				if (index % 3 == 0)
				{
					Vector3 delta = point - points[index];
					if (index > 0)
					{
						points[index - 1] += delta;
					}
					if (index + 1 < points.Length)
					{
						points[index + 1] += delta;
					}
				}
			}

			points[index] = point;
			EnforceMode(index);
		}

		public void SetControlPointMode(int index, BezierControlPointMode mode)
		{
			modes[(index + 1) / 3] = mode;
			EnforceMode(index);
		}

		[SerializeField]
		private BezierControlPointMode[] modes;

		[SerializeField]
		private Vector3[] points;

		private float[] distanceAtTime;

		void Awake()
		{
			float sliceCount = 10000 * CurveCount;
			distanceAtTime = new float[(int)sliceCount];
			for (int i = 0; i < sliceCount; i++)
			{
				float a = i / sliceCount;
				float b = (i + 1) / sliceCount;
				pathDistance += Vector3.Distance(GetPoint(a), GetPoint(b));
				distanceAtTime[i] = pathDistance;
			}
		}
	}
}

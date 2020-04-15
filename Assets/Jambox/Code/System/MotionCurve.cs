using UnityEngine;
using System.Collections;

namespace Jambox
{
	public enum MotionCurveType 
	{
		Linear,
		Exponential,
		Limit,
		EaseInOut
	}

	public static class MotionCurve 
	{

		// requires a value between 0 and 1
		public static float GetCurveForPercentComplete (float percentComplete, MotionCurveType motionCurveType)
		{
			// default is linear curve
			float curvedValuePercent = 0;
			float tempPercentComplete;

			switch (motionCurveType)
			{

			case MotionCurveType.Exponential:
				// Start slow, accelerate rapidly, end fast
				curvedValuePercent = percentComplete * percentComplete;

				/* for a slightly different curve, you can take a slice of the sine wave
				// Applies Sine curve from -PI/2 to 0
				float halfPI = Mathf.PI * 0.5f;
				tempPercentComplete = -halfPI + percentComplete * halfPI;
				curvedValuePercent = 1 + Mathf.Sin(tempPercentComplete);
				*/
				break;

			case MotionCurveType.Limit:
				// Start fast, decelerate quickly, end slow
				// Applies Sine curve from 0 to PI/2
				curvedValuePercent = Mathf.Sin(percentComplete * Mathf.PI * 0.5f);
				break;

			case MotionCurveType.Linear:
				// do nothing: linear means we use the unmodified percentage
				curvedValuePercent = percentComplete;
				break;

			case MotionCurveType.EaseInOut:
				// Start slow, accelerate rapidly, decelerate quickly, end slow
				// Applies offset of Sine Curve
				tempPercentComplete = percentComplete;
				tempPercentComplete -= 0.5f;
				tempPercentComplete = Mathf.Sin(tempPercentComplete * Mathf.PI);
				curvedValuePercent = (tempPercentComplete + 1) * 0.5f;
				break;

			default:
				Debug.LogWarning("Did not recognize MotionCurveType." + motionCurveType.ToString());
				break;
			}

			return curvedValuePercent;
		}
	}
}

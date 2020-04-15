// Jambox
// Copyright (c) 2019 Oreganik LLC
// Author: Ted Brown

using UnityEngine;

namespace Jambox
{
	[System.Serializable]
	public struct FloatRange
	{
		public float Min { get { return _min; } }
		public float Max { get { return _max; } }
		public float Random { get { return UnityEngine.Random.Range(_min, _max); } }

		[SerializeField] private float _min, _max;

		public FloatRange (float min, float max)
		{
			_min = min;
			_max = max;
		}

		public float Normalize (float value)
		{
			if (_min == _max)
			{
				Debug.LogError("MinMax has same min and max. Returned normalized value of -1.");
				return -1;
			}
			return Mathf.Clamp01((value - _min) / (_max - _min));
		}

		public string ToString (string format = "")
		{
			return "Min: " + _min.ToString(format) + " / Max: " + _max.ToString(format);
		}
	}
}

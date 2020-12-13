// JAMBOX
// General purpose game code for Unity
// Copyright 2020 Ted Brown

using UnityEngine;

namespace Jambox
{
	[System.Serializable]
	public struct Timer 
	{
		public bool IsComplete
		{
			get
			{
				// An invalid timer (duration of zero or less) is always complete
				if (_duration <= 0) return true;
				return _time >= _duration;
			}
		}

		public float t 
		{
			get
			{
				// An invalid timer (duration of zero or less) is always complete
				if (_duration <= 0) return 1;
				return Mathf.Clamp01(_time / _duration);
			}
		}

		private float _time;
		private float _duration;

		public Timer (float duration)
		{
			_duration = duration;
			_time = 0;
		}

		public void Reset ()
		{
			_time = 0;
		}

		public void FinishNow ()
		{
			_time = _duration;
		}

		public void StartNewTimer (float duration)
		{
			_duration = duration;
			_time = 0;
		}

		public void Update (float deltaTime)
		{
			_time = Mathf.Clamp(_time + deltaTime, 0, _duration);
		}
	}
}

// JAMBOX
// General purpose game code for Unity
// Copyright (c) 2021 Ted Brown

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jambox
{
	/// <summary>
	/// An FpsStat is an integer with a Base, Min, Max, and ID.
	/// It can be constructed with an enum, but to keep it flexible for prototyping, enums are converted to strings.
	/// </summary>
	[Serializable]
	public class FpsStat 
	{
		private const string STRING = "Id {0} is {1}. Base: {2}. Min/Max: {3}/{4}";

		/// <summary>The initial value, as well as its value after being reset.</summary>
		public readonly int Base;
		/// <summary>Can't be less than zero. Can't be more than max.</summary>
		public readonly int Min;
		/// <summary>A max of 0 or less means the value is unlimited.</summary>
		public readonly int Max;
		public readonly string Id;

		public int Value
		{
			get 
			{ 
				return _value; 
			}
			set
			{
				int prev = _value;
				_value = Mathf.Clamp(value, Min, Max);
				//Debug.LogFormat("Adjust {0}: {1} -> {2}", Id, prev, _value);
			}
		}

		private int _value;

		public FpsStat (string id, int baseValue, int max = 0, int min = 0)
		{
			Id = id;
			Base = baseValue;
			Min = Mathf.Max(min, 0);
			Max = (max <= 0) ? int.MaxValue : max;
			Value = Base;
			DebugErrorCheck();
		}

		public FpsStat (Enum id, int baseValue, int max = 0, int min = 0)
		{
			Id = id.ToString();
			Base = baseValue;
			Min = Mathf.Max(min, 0);
			Max = (max <= 0) ? int.MaxValue : max;
			Value = Base;
			DebugErrorCheck();
		}

		public void Reset ()
		{
			Value = Base;
		}

		public override string ToString ()
		{
			return string.Format(STRING, Id, Value, Base, Min, Max);
		}

		private void DebugErrorCheck ()
		{
			Debug.Assert(Base >= Min, "Base is less than Min for FpsStat." + Id);
			Debug.Assert(Base <= Max, "Base is greater than Max for FpsStat." + Id);
			Debug.Assert(Min < Max, "Min is greater than or equal to Max for FpsStat." + Id);
		}
	}
}
